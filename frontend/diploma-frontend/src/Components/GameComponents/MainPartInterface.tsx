import { useState, type ChangeEvent } from "react"
import type { Team } from "../../Models/Team"
import type { Mistake } from "../../Models/Mistake"

interface Props {
    setState: (newState: string) => void,
    Team1: Team,
    Team2: Team,
    speakerId: number,
    opponentId: number,
    ChallengingTeamId: number,
    isCheckingCorrectness: boolean,
    ConfirmNoSolution: () => void,
    DeclareRolesChange: (isFullRoleChange: boolean) => void,
    DeclareBreak: (BreakRequestingTeamId: number) => void,
    DeclareChange: (ChangeRequestingTeamId: number, NewParticipantId: number) => void,
    EndRound: (Team1Points: number, Team2Points: number, AssessorsPoints: number, Mistakes: Mistake[]) => Promise<boolean>
}
export default function MainPartInterface({ setState, Team1, Team2, ChallengingTeamId, speakerId, opponentId, isCheckingCorrectness, ConfirmNoSolution, DeclareRolesChange, DeclareBreak, DeclareChange, EndRound }: Props) {
    const [BreakRequestingTeamId, setBreakRequestingTeamId] = useState(Team1.Id)
    const [ChangeRequestingTeamId, setChangeRequestingTeamId] = useState(Team1.Id)
    const [NewParticipantId, setNewParticipantId] = useState(Team1.Students.$values[0].Id)
    const [Team1Points, setTeam1Points] = useState(0)
    const [Team2Points, setTeam2Points] = useState(0)
    const [AssessorPoints, setAssessorPoints] = useState(0)
    const [Mistakes, setMistakes] = useState<Mistake[]>([])

    const [CallingTeam] = useState(Team1.Id == ChallengingTeamId ? Team1 : Team2)
    const [CalledTeam] = useState(Team1.Id == ChallengingTeamId ? Team2 : Team1)

    const [SpeakerTeam] = useState(isCheckingCorrectness ? CallingTeam : CalledTeam)
    const [OpponentTeam] = useState(isCheckingCorrectness ? CalledTeam : CallingTeam)

    const [SpeakerId] = useState(speakerId)
    const [OpponentId] = useState(opponentId)

    const [Speaker] = useState(SpeakerTeam.Students.$values.find((student) => student.Id == SpeakerId))
    const [Opponent] = useState(OpponentTeam.Students.$values.find((student) => student.Id == OpponentId))
    function handlePartialRolesChangeFixation(): void {
        DeclareRolesChange(false)
    }

    function handleFullRolesChangeFixation(): void {
        DeclareRolesChange(true)
    }

    function handleBreakDeclaration(): void {
        DeclareBreak(BreakRequestingTeamId)
    }

    function handleChangeDeclaration(): void {
        DeclareChange(ChangeRequestingTeamId, NewParticipantId)
    }

    function handleMistakeCreation(): void {
        setMistakes((currentMistakes) => {
            currentMistakes.push({
                OpponentsCost: 0,
                JureCost: 0,
                Text: ""
            })
            return currentMistakes
        })
    }
    async function handleRoundEnd() {
        if (await EndRound(Team1Points, Team2Points, AssessorPoints, Mistakes))
            setState("info")
    }
    function handleTeam1PointsChange(event: ChangeEvent<HTMLInputElement>): void {
        setTeam1Points(parseInt(event.target.value))
    }

    function handleTeam2PointsChange(event: ChangeEvent<HTMLInputElement>): void {
        setTeam2Points(parseInt(event.target.value))
    }
    function HandleChangeRequestingTeamIdChange(event: ChangeEvent<HTMLSelectElement>): void {
        setChangeRequestingTeamId(parseInt(event.target.value))
    }

    function HandleNewParticipantIdChange(event: ChangeEvent<HTMLSelectElement>): void {
        setNewParticipantId(parseInt(event.target.value))
    }
    function HandleBreakRequestingTeamIdChange(event: ChangeEvent<HTMLSelectElement>): void {
        setBreakRequestingTeamId(parseInt(event.target.value))
    }
    function handleAssessorPointsChange(event: ChangeEvent<HTMLInputElement>): void {
        setAssessorPoints(parseInt(event.target.value))
    }

    function handleNoSolutionFixation(): void {
        ConfirmNoSolution
    }

    return (
        <>
            Изначальный Докладчика: {Speaker.Surname} {Speaker.Name} {Speaker.Fatname}<br />
            Команда Докладчика: {SpeakerTeam.Name}<br />
            Изначальный Оппонент: {Opponent.Surname} {Opponent.Name} {Opponent.Fatname}<br />
            Команда Оппонента: {OpponentTeam.Name}<br />
            <input style={{ display: "inline-block" }} type="button" value="Зафиксировать частичную перемену ролей" onClick={handlePartialRolesChangeFixation} />
            <input style={{ display: "inline-block" }} type="button" value="Зафиксировать полную перемену ролей" onClick={handleFullRolesChangeFixation} /><br />
            <input style={{ display: "inline-block" }} type="button" value="Зафиксировать отсутствие решения" onClick={handleNoSolutionFixation} /><br />
            <br />
            <input style={{ display: "inline-block" }} type="button" value="Зафиксировать перерыв" onClick={handleBreakDeclaration} />
            Команда запросившая перерыв:<select value={BreakRequestingTeamId} onChange={HandleBreakRequestingTeamIdChange}>
                <option value={Team1.Id}>
                    Команда {Team1.Name}
                </option>
                <option value={Team2.Id}>
                    Команда {Team2.Name}
                </option>
            </select>
            <br />
            <input style={{ display: "inline-block" }} type="button" value="Зафиксировать замену" onClick={handleChangeDeclaration} />
            Команда запросившая замену:<select value={ChangeRequestingTeamId} onChange={HandleChangeRequestingTeamIdChange}>
                <option value={Team1.Id}>
                    Команда {Team1.Name}
                </option>
                <option value={Team2.Id}>
                    Команда {Team2.Name}
                </option>
            </select>
            <br />
            Новый выступающий:<select value={NewParticipantId} onChange={HandleNewParticipantIdChange}>
                {ChangeRequestingTeamId == Team1.Id ?
                    Team1.Students.$values.map((student) => {
                        return (
                            <option value={student.Id}>
                                {student.Name} {student.Surname} {student.Fatname}
                            </option>
                        )
                    })
                    : Team2.Students.$values.map((student) => {
                        return (
                            <option value={student.Id}>
                                {student.Name} {student.Surname} {student.Fatname}
                            </option>
                        )
                    })
                }
            </select>
            <br />
            <br />
            <input type="button" value="Создать ошибку" onClick={handleMistakeCreation} />
            <table>
                <thead>
                    <tr>
                        <th>
                            Описание ошибки
                        </th>
                        <th>
                            Очки жюри
                        </th>
                        <th>
                            Очки оппонента
                        </th>
                        <th>

                        </th>
                    </tr>
                </thead>
                <tbody>
                    {Mistakes.map((mistake, index) => {
                        function handleMistakeDeletion(): void {
                            setMistakes(Mistakes.filter((mist, index) => index != Mistakes.indexOf(mistake)))
                        }
                        function handleMistakeTextChange(event: ChangeEvent<HTMLInputElement>): void {
                            setMistakes((currentMistakes) => {
                                currentMistakes[index].Text = event.target.value
                                return currentMistakes
                            })
                            mistake.Text = event.target.value
                        }

                        function handleMistakeJureCostChange(event: ChangeEvent<HTMLInputElement>): void {
                            setMistakes((currentMistakes) => {
                                currentMistakes[index].JureCost = parseInt(event.target.value)
                                return currentMistakes
                            })
                            mistake.JureCost = parseInt(event.target.value)
                        }

                        function handleMistakeOpponentCostChange(event: ChangeEvent<HTMLInputElement>): void {
                            setMistakes((currentMistakes) => {
                                currentMistakes[index].OpponentsCost = parseInt(event.target.value)
                                return currentMistakes
                            })
                            mistake.OpponentsCost = parseInt(event.target.value)
                        }

                        return (
                            <tr>
                                <td>
                                    <input type="text" value={mistake.Text} onChange={handleMistakeTextChange} />
                                </td>
                                <td>
                                    <input type="number" value={mistake.JureCost} onChange={handleMistakeJureCostChange} />
                                </td>
                                <td>
                                    <input type="number" value={mistake.OpponentsCost} onChange={handleMistakeOpponentCostChange} />
                                </td>
                                <td>
                                    <input type="button" value="Удалить ошибку" onClick={handleMistakeDeletion} />
                                </td>
                            </tr>
                        )
                    })
                    }
                </tbody>
            </table>
            Очки 1 команды:<input style={{ display: "inline-block" }} type="number" value={Team1Points} onChange={handleTeam1PointsChange} />
            Очки 2 команды:<input style={{ display: "inline-block" }} type="number" value={Team2Points} onChange={handleTeam2PointsChange} />
            Очки жюри:<input style={{ display: "inline-block" }} type="number" value={AssessorPoints} onChange={handleAssessorPointsChange} />
            <input style={{ display: "inline-block" }} type="button" value="Зафиксировать результаты и закончить раунд" onClick={handleRoundEnd} />
        </>
    )
}