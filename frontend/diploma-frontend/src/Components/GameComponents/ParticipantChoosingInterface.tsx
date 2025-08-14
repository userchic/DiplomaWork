import { useState, type ChangeEvent } from "react";
import type { Team } from "../../Models/Team";

interface Props {
    setState: (newState: string) => void,
    Team1: Team,
    Team2: Team,
    isCheckingCorrectness: boolean,
    ChallengingTeamId: number
    StartRound: (SpeakerId: number, OpponentId: number) => Promise<boolean>
}
export default function ParticipantsChoosingInterface({ setState, Team1, Team2, isCheckingCorrectness, ChallengingTeamId, StartRound }: Props) {

    let CallingTeam = Team1.Id == ChallengingTeamId ? Team1 : Team2
    let CalledTeam = Team1.Id == ChallengingTeamId ? Team2 : Team1

    let SpeakerTeam = isCheckingCorrectness ? CallingTeam : CalledTeam
    let OpponentTeam = isCheckingCorrectness ? CalledTeam : CallingTeam

    const [SpeakerId, setSpeakerId] = useState<number>(SpeakerTeam.Students.$values[0].Id)
    const [OpponentId, setOpponentId] = useState<number>(OpponentTeam.Students.$values[0].Id)

    function handleSpeakerIdChange(event: ChangeEvent<HTMLSelectElement>): void {
        setSpeakerId(parseInt(event.target.value))
    }

    function handleOpponentIdChange(event: ChangeEvent<HTMLSelectElement>): void {
        setOpponentId(parseInt(event.target.value))
    }

    async function handleRoundStart() {
        if (SpeakerId !== undefined && OpponentId !== undefined) {
            if (await StartRound(SpeakerId, OpponentId))
                setState("MainPart")
        }
    }

    return (
        <>
            Изначальный докладчик: <select value={SpeakerId} onChange={handleSpeakerIdChange}>
                {
                    SpeakerTeam.Students.$values.map((student) => {

                        return (
                            <option value={student.Id}>
                                {student.Surname} {student.Name} {student.Fatname}
                            </option>
                        )
                    })
                }
            </select>
            Изначальный оппонент: <select value={OpponentId} onChange={handleOpponentIdChange}>
                {
                    OpponentTeam.Students.$values.map((student) => {

                        return (
                            <option value={student.Id}>
                                {student.Surname} {student.Name} {student.Fatname}
                            </option>
                        )
                    })
                }
            </select>
            <input type="button" value="Начать раунд" onClick={handleRoundStart} />
        </>
    )
}