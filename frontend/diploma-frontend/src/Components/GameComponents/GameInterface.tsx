import { useEffect, useState, type ChangeEvent } from "react";
import type { Game } from "../../Models/Game";
import { EndGame, EndRound, GetGame, RejectToChallenge } from "../../Services/GameService";
import { useNavigate, useParams } from "react-router";
import type { EndRoundRequest, Round } from "../../Models/Round";
import type { Task } from "../../Models/Task";
import type { Mistake } from "../../Models/Mistake";
import RoundInterface from "./RoundInterface";
import type { Challenge } from "../../Models/Challenge";

export default function GameInterface() {
    const params = useParams()
    let gameId: number = parseInt(params.gameId)
    const [Challenges, setChallenges] = useState<Challenge[]>([])
    const [Game, setGame] = useState<Game>()
    const [ShowingChallenge, setShowingChallenge] = useState<Challenge>()
    useEffect(() => {
        getGame()
    }, [])
    const getGame = async () => {
        const res: Game = await GetGame(gameId)
        setGame(res)
        if (!res.GameEnded)
            if (res.Challenges.$values.length == 0 || (res.Challenges.$values[res.Challenges.$values.length - 1].Round?.RoundResults !== null && res.Challenges.$values[res.Challenges.$values.length - 1].Round?.RoundResults !== undefined)) {
                res.Challenges.$values.push({
                    IsCheckingCorrectness: false,
                    IsChallengeAccepted: false,
                    GameId: gameId,
                })
            }
        setChallenges(res.Challenges.$values)
    }
    function SwapChallengingTeams() {
        setGame((currentGame) => {
            currentGame.ChallengingTeamId = currentGame.Team1Id + currentGame.Team2Id - currentGame.ChallengingTeamId
            return currentGame
        })
    }
    async function endRound(gameId: number, challenge: Challenge, EndRoundRequest: EndRoundRequest): Promise<boolean> {
        const endRound = async () => {
            const res = await EndRound(gameId, EndRoundRequest)
            if (res.message !== undefined)
                alert(res.message)
            else
                alert(res.errors[0])
            let result: boolean = res.success
            if (res.success) {
                Game.Team1Points += EndRoundRequest.Team1Points
                Game.Team2Points += EndRoundRequest.Team2Points
                let Correctness = true
                if (challenge.IsCheckingCorrectness && challenge.Round?.NoSolution)
                    Correctness = false
                if (Correctness && !Game.TeamRejectedToChallenge)
                    SwapChallengingTeams()
                var changedChallenges = Challenges
                changedChallenges[changedChallenges.length - 1].Round.RoundResults =
                {
                    Correctness: changedChallenges[changedChallenges.length - 1].IsCheckingCorrectness && changedChallenges[changedChallenges.length - 1].Round.NoSolution ? true : false,
                    Mistakes: { $values: EndRoundRequest.Mistakes },
                    Team1Points: EndRoundRequest.Team1Points,
                    Team2Points: EndRoundRequest.Team2Points,
                }
                if (Challenges.length < Game.Tasks.$values.length) {
                    changedChallenges.push({
                        GameId: Game.Id,
                        IsCheckingCorrectness: false,
                        IsChallengeAccepted: false,
                    })
                    setChallenges([...changedChallenges])
                }
                else
                    getGame();
            }
            return result
        }
        return await endRound()
    }
    async function handleEndGame() {
        const res = await EndGame(gameId)
        if (res.message !== undefined)
            alert(res.message)
        else
            alert(Object.values(res.errors)[0])
        if (res.success) {
            getGame()
        }
    }

    return (
        <>
            <center>
                <div className={"block"} style={{ display: "inline-block", padding: "10px" }}>
                    Команда 1: <b>"{Game?.Team1.Name}"</b> <br />
                    Очки: {Game?.Team1Points}
                </div>
                {
                    !Game?.GameEnded ?
                        <input type="button" value="Закончить игру" onClick={handleEndGame} /> :
                        null
                }
                <div className={"block"} style={{ display: "inline-block", padding: "10px" }}>
                    Команда 2: <b>"{Game?.Team2.Name}"</b> <br />
                    Очки: {Game?.Team2Points}
                </div>
                <br />
                {Game?.GameEnded ? "Игра завершена" : null}
            </center>

            <br />
            <div className={"block"} >
                <h2>{Game?.Name}</h2>
                <h4>Математический бой</h4>
                {
                    Game?.Team1.Id === Game?.CaptainsRound.WinnerId ?
                        <div>
                            Команда <b>{Game?.Team1.Name}</b> победитель в <b>капитанском раунде</b>
                        </div>
                        :
                        <div>
                            Команда <b>{Game?.Team2.Name}</b> победитель в <b>капитанском раунде</b>
                        </div>
                }
                <div style={{ display: "flex" }}>
                    <div className={"block"} style={{ flex: 0.1 }}>
                        {
                            Challenges?.map((challenge, index) => {
                                function handleShownRoundChange() {
                                    setShowingChallenge(challenge)
                                }
                                return <>
                                    <input key={index} type="button" onClick={handleShownRoundChange} value={"Раунд " + (index + 1)} /><br />
                                </>
                            }
                            )
                        }
                    </div>
                    <div className={"block"} style={{ flex: 1 }}>
                        {ShowingChallenge !== null && ShowingChallenge !== undefined ?
                            <RoundInterface
                                challenge={ShowingChallenge}
                                Game={Game}
                                EndGame={handleEndGame}
                                EndRound={endRound}
                                RejectToChallenge={async () => {
                                    const res = await RejectToChallenge(gameId)
                                    alert(res.message)
                                    if (res.success) {
                                        if (Game.TeamRejectedToChallenge)
                                            getGame()
                                        else {
                                            Game.TeamRejectedToChallenge = true
                                            SwapChallengingTeams()
                                        }
                                    }
                                    let result: boolean = res.success
                                    return result
                                }}
                            /> : ""}
                    </div>
                </div >
            </div >
        </>
    )
}
