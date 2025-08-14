import { useEffect, useState, type ChangeEvent } from "react";
import type { EndRoundRequest, Round } from "../../Models/Round";
import type { Game } from "../../Models/Game";
import ChallengeInterface from "./ChallengeInterface";
import { ConfirmChallengeAcceptance, ConfirmCorrectnessCheck, ConfirmNoSolution, DeclareBreak, DeclareChange, DeclareRoleChange, EndGame, FixateChallenge, StartNewRound } from "../../Services/GameService";
import ChallengeAcceptanceInterface from "./ChallengeAcceptanceInterface";
import ParticipantsChoosingInterface from "./ParticipantChoosingInterface";
import MainPartInterface from "./MainPartInterface";
import InfoInterface from "./InfoInterface";
import StartOneTeamPerformance from "./StartOneTeamPerformance";
import type { Challenge } from "../../Models/Challenge";
interface Props {
    challenge: Challenge,
    Game: Game,
    RejectToChallenge: () => Promise<boolean>,
    EndGame: () => void,
    EndRound: (gameId: number, challenge: Challenge, endRoundRecord: EndRoundRequest) => Promise<boolean>
}
export default function RoundInterface({ challenge, Game, EndRound, RejectToChallenge }: Props) {

    const [State, setState] = useState("")
    useEffect(() => {
        if (Game.GameEnded) {
            setState("info")
        }
        else {
            if (challenge.Round === null || challenge.Round === undefined) {
                if (challenge.DeclareTime !== undefined && challenge.DeclareTime !== null) {
                    if (challenge.IsChallengeAccepted || challenge.IsCheckingCorrectness) {
                        setState("ParticipantChoosing")
                        return
                    }
                    else {
                        setState("CallAcceptRejection")
                        return
                    }
                }
                else {
                    setState("Challenge")
                    return
                }
            }
            else {
                if (challenge.Round.RoundResults !== null && challenge.Round.RoundResults !== undefined) {
                    setState("info")
                    return
                }
                else {
                    setState("MainPart")
                    return
                }
            }
        }
    }, []
    )

    function ChallengeCall(TaskId: number) {
        const fixateChallenge = async () => {
            const res = await FixateChallenge(Game.Id, TaskId)
            alert(res.message)
            if (res.success) {
                challenge.TaskId = TaskId
                challenge.RequestingTeamId = Game.ChallengingTeamId
                challenge.DeclareTime = new Date()
                return true
            }
            return false
        }
        return fixateChallenge()
    }

    function confirmCorrectnessCheck() {
        const confirmCorrectness = async () => {
            const res = await ConfirmCorrectnessCheck(Game.Id)
            alert(res.message)
            if (res.success) {
                challenge.IsCheckingCorrectness = true
                return true
            }
            return false
        }

        return confirmCorrectness
    }
    async function confirmChallengeAcceptance() {
        const confirmAcceptance = async () => {
            const res = await ConfirmChallengeAcceptance(Game.Id)
            alert(res.message)
            if (res.success) {
                challenge.IsChallengeAccepted = true
                return true
            }
            return false
        }
        return confirmAcceptance()
    }
    async function StartRound(SpeakerId: number, OpponentId: number): Promise<boolean> {
        const startRound = async () => {
            const res = await StartNewRound({
                GameId: Game.Id,
                OpponentId: OpponentId,
                SpeakerId: SpeakerId,
            })
            let result: boolean = res.success
            if (result) {
                challenge.Round = {
                    SpeakerId: SpeakerId,
                    OpponentId: OpponentId,
                    Breaks: { $values: [] },
                    Changes: { $values: [] },
                    RoundNumber: Game.Challenges.$values.length,
                    StartTime: new Date()
                }
            }
            return result
        }
        if (await startRound()) {
            challenge.Round =
            {
                SpeakerId: SpeakerId,
                OpponentId: OpponentId,
                Breaks: { $values: [] },
                Changes: { $values: [] },
                RoundNumber: Game.Challenges.$values.length,
                StartTime: new Date(),
            }

            return true
        }
        return false
    }

    return (
        <>
            <h2>Раунд{Game.Challenges.$values.indexOf(challenge) + 1}</h2>
            {
                State == "Challenge" ?
                    <>
                        <ChallengeInterface
                            setState={setState}
                            Tasks={Game.Tasks.$values}
                            CallingTeamName={Game.Team1Id == Game.ChallengingTeamId ? Game.Team1.Name : Game.Team2.Name}
                            ChallengeCall={(TaskId: number) => ChallengeCall(TaskId)}
                            RejectToChallenge={RejectToChallenge}
                        />
                    </>
                    : null
            }
            {
                State == "CallAcceptRejection" ?
                    <ChallengeAcceptanceInterface
                        setState={setState}
                        ConfirmCorrectnessCheck={confirmCorrectnessCheck()}
                        ChallengeAccept={
                            confirmChallengeAcceptance
                        }
                    />
                    : null
            }
            {
                State == "ParticipantChoosing" ?
                    <ParticipantsChoosingInterface
                        setState={setState}
                        Team1={Game.Team1}
                        Team2={Game.Team2}
                        isCheckingCorrectness={challenge.IsCheckingCorrectness}
                        ChallengingTeamId={Game.ChallengingTeamId}
                        StartRound={StartRound}
                    />
                    : null
            }
            {
                State == "LastTeamWantsToPerform" ?
                    <StartOneTeamPerformance
                        setState={setState}
                        RejectToChallenge={RejectToChallenge}
                    />
                    : null
            }
            {
                State == "MainPart" ?
                    <MainPartInterface
                        setState={setState}
                        Team1={Game.Team1}
                        Team2={Game.Team2}
                        speakerId={challenge.Round?.SpeakerId}
                        opponentId={challenge.Round?.OpponentId}
                        ChallengingTeamId={Game.ChallengingTeamId}
                        isCheckingCorrectness={challenge.IsCheckingCorrectness}
                        ConfirmNoSolution={async () => {
                            const res = await ConfirmNoSolution(Game.Id)
                            alert(res.message)
                            if (res.success) {
                                challenge.Round.NoSolution = true
                            }
                            return res.success
                        }
                        }
                        DeclareRolesChange={async (isFullChange) => {
                            const res = await DeclareRoleChange(Game.Id, isFullChange)
                            alert(res.message)
                            if (res.success) {
                                challenge.Round.RolesChange = {
                                    ChangeTime: new Date(),
                                    FullRoleChange: isFullChange
                                }
                            }
                        }
                        }
                        EndRound={
                            async (Team1Points, Team2Points, AssessorPoints, Mistakes) => {

                                let res = EndRound(Game.Id, challenge, {
                                    AssessorPoints: AssessorPoints,
                                    Team1Points: Team1Points,
                                    Team2Points: Team2Points,
                                    Mistakes: Mistakes
                                })
                                if (await res) {
                                    challenge.Round.RoundResults = {
                                        Team1Points: Team1Points,
                                        Team2Points: Team2Points,
                                        Correctness: true,
                                        Mistakes: {
                                            $values: Mistakes
                                        }
                                    }
                                    if (challenge.IsCheckingCorrectness && challenge.Round?.NoSolution)
                                        challenge.Round.RoundResults.Correctness = false
                                }
                                return await res
                            }
                        }
                        DeclareChange={async (RequestingTeamId, NewParticipantId) => {
                            let res = await DeclareChange(Game.Id, RequestingTeamId, NewParticipantId)
                            alert(res.message)
                            if (res.success) {
                                let initiatorTeam = Game.Team1Id == RequestingTeamId ? Game.Team1 : Game.Team2
                                let currentParticipantId = initiatorTeam.Students.$values.find((student) => student.Id == challenge.Round.OpponentId) === undefined ? challenge.Round.SpeakerId : challenge.Round.OpponentId
                                let currentParticipantRole = currentParticipantId == challenge.Round.OpponentId ? 1 : 2
                                challenge.Round.Changes.$values.push({
                                    DeclareTime: new Date(),
                                    RoleId: currentParticipantRole,
                                    NewParticipantId: NewParticipantId,
                                    InitiatorTeamId: RequestingTeamId,
                                })
                            }
                        }}
                        DeclareBreak={async (RequestingTeamId) => {
                            let res = await DeclareBreak(Game.Id, RequestingTeamId)
                            alert(res.message)
                            if (res.success) {
                                challenge.Round.Breaks.$values.push({
                                    InitiatorTeamId: RequestingTeamId,
                                    InitiatorTeam: Game.Team1Id == RequestingTeamId ? Game.Team1 : Game.Team2,
                                    DeclareTime: new Date()
                                })
                            }
                        }}
                    />
                    : null
            }
            {
                State == "info" ?
                    <InfoInterface
                        ChallengingTeamId={Game.ChallengingTeamId}
                        Team1={Game.Team1}
                        Team2={Game.Team2}
                        Challenge={challenge} />
                    : null
            }
        </>
    )
}