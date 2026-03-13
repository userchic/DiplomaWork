import { useState, type ChangeEvent } from "react"
import type { Team } from "../../Models/Team"
import type { Mistake } from "../../Models/Mistake"
import type { Challenge } from "../../Models/Challenge"
import ChangesInfo from "./ChangesInfo"
import BreaksInfo from "./BreaksInfo"

interface Props {
    setState: (newState: string) => void,
    challenge: Challenge,
    Team1: Team,
    Team2: Team,
    speakerId: number,
    opponentId: number,
    ChallengingTeamId: number,
    isCheckingCorrectness: boolean,
    ConfirmNoSolution: () => Promise<boolean>,
    DeclareRolesChange: (isFullRoleChange: boolean) => Promise<boolean>,
    DeclareBreak: (BreakRequestingTeamId: number) => Promise<boolean>,
    DeclareChange: (ChangeRequestingTeamId: number, NewParticipantId: number) => Promise<boolean>,
    EndRound: (Team1Points: number, Team2Points: number, AssessorsPoints: number, Mistakes: Mistake[]) => Promise<boolean>
}
export default function MainPartInterface({ setState, challenge, Team1, Team2, ChallengingTeamId, speakerId, opponentId, isCheckingCorrectness, ConfirmNoSolution, DeclareRolesChange, DeclareBreak, DeclareChange, EndRound }: Props) {
    const [BreakRequestingTeamId, setBreakRequestingTeamId] = useState(Team1.Id)
    const [ChangeRequestingTeamId, setChangeRequestingTeamId] = useState(Team1.Id)
    const [NewParticipantId, setNewParticipantId] = useState(Team1.Students.$values[0].Id)
    const [Team1Points, setTeam1Points] = useState(0)
    const [Team2Points, setTeam2Points] = useState(0)
    const [AssessorPoints, setAssessorPoints] = useState(0)
    const [Mistakes, setMistakes] = useState<Mistake[]>([])

    const [Challenge] = useState(challenge)
    const [Round, setRound] = useState(Challenge.Round)

    const [CallingTeam] = useState(Team1.Id == ChallengingTeamId ? Team1 : Team2)
    const [CalledTeam] = useState(Team1.Id == ChallengingTeamId ? Team2 : Team1)

    const [SpeakerTeam] = useState(isCheckingCorrectness ? CallingTeam : CalledTeam)
    const [OpponentTeam] = useState(isCheckingCorrectness ? CalledTeam : CallingTeam)

    const [SpeakerId] = useState(speakerId)
    const [OpponentId] = useState(opponentId)

    const [Speaker] = useState(SpeakerTeam.Students.$values.find((student) => student.Id == SpeakerId))
    const [Opponent] = useState(OpponentTeam.Students.$values.find((student) => student.Id == OpponentId))
    async function handlePartialRolesChangeFixation() {
        let res = await DeclareRolesChange(false)
        if (res) {
            setRound((round) => {
                round.RolesChange = {
                    ChangeTime: new Date(),
                    FullRoleChange: false,
                    RoundId: Round?.Id
                }
                return Object.create(round)
            })
        }
    }

    async function handleFullRolesChangeFixation() {
        let res = await DeclareRolesChange(true)
        if (res) {
            setRound((round) => {
                round.RolesChange = {
                    ChangeTime: new Date(),
                    FullRoleChange: true,
                    RoundId: Round?.Id
                }
                return Object.create(round)
            })
        }
    }

    async function handleBreakDeclaration() {
        let res = await DeclareBreak(BreakRequestingTeamId)
        if (res) {
            setRound((round) => {
                round.Breaks.$values.concat({
                    DeclareTime: new Date(),
                    InitiatorTeamId: ChangeRequestingTeamId,
                    InitiatorTeam: SpeakerTeam.Id === ChangeRequestingTeamId ? SpeakerTeam : OpponentTeam,
                    RoundId: round.Id
                })
                return Object.create(round)
            })
        }
    }

    async function handleChangeDeclaration() {
        let res = await DeclareChange(ChangeRequestingTeamId, NewParticipantId)
        if (res) {
            setRound((round) => {
                let newChange = {

                    DeclareTime: new Date(),
                    RoundId: round.Id,
                    RoleId: SpeakerTeam.Id === ChangeRequestingTeamId ? SpeakerTeam.Id : OpponentTeam.Id,
                    NewParticipantId: NewParticipantId,
                    NewParticipant: SpeakerTeam.Id === ChangeRequestingTeamId ? SpeakerTeam.Students.$values.find((stud) => stud.Id == NewParticipantId) : OpponentTeam.Students.$values.find((stud) => stud.Id == NewParticipantId),
                    InitiatorTeamId: ChangeRequestingTeamId,
                    InitiatorTeam: SpeakerTeam.Id === ChangeRequestingTeamId ? SpeakerTeam : OpponentTeam
                }
                round.Changes.$values = round?.Changes.$values.concat(newChange)
                return Object.create(round)
            })
        }
    }

    function handleMistakeCreation(): void {
        setMistakes((currentMistakes) => {
            currentMistakes.push({
                OpponentsCost: 0,
                JureCost: 0,
                Text: ""
            })
            return [...currentMistakes]
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

    async function handleNoSolutionFixation() {
        let res = await ConfirmNoSolution()
        if (res) {
            setRound((round) => {
                round.NoSolution = true;
                return Object.create(round)
            })
        }
    }

    return (
        <>
            <h3>Выступление</h3>
            <b>Задача:</b> {Challenge.Task?.Text}<br />
            <h3>Выступающие</h3>
            <table style={{ width: "50%" }} className="table">
                <thead>
                    <tr>
                        <th>
                            Роль
                        </th>
                        <th>
                            Выступающий
                        </th>
                        <th>
                            Email
                        </th>
                        <th>
                            Команда
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            Докладчик
                        </td>
                        <td>
                            {Round?.Speaker?.Surname} {Round?.Speaker?.Name} {Round?.Speaker?.Fatname}
                        </td>
                        <td>
                            {Round?.Speaker?.Email}
                        </td>
                        <td>
                            {SpeakerTeam?.Name}
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Оппонент
                        </td>
                        <td>
                            {Round?.Opponent?.Surname} {Round?.Opponent?.Name} {Round.Opponent?.Fatname}
                        </td>
                        <td>
                            {Round?.Opponent?.Email}
                        </td>
                        <td>
                            {OpponentTeam?.Name}
                        </td>
                    </tr>
                </tbody>
            </table>
            <br />
            <br />
            <h3>Ошибки</h3>
            <input type="button" value="Создать ошибку" onClick={handleMistakeCreation} />
            <table className="table" style={{ width: "40%" }}>
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
                                return [...currentMistakes]
                            })
                        }

                        function handleMistakeJureCostChange(event: ChangeEvent<HTMLInputElement>): void {
                            setMistakes((currentMistakes) => {
                                currentMistakes[index].JureCost = parseInt(event.target.value)
                                return [...currentMistakes]
                            })
                        }

                        function handleMistakeOpponentCostChange(event: ChangeEvent<HTMLInputElement>): void {
                            setMistakes((currentMistakes) => {
                                currentMistakes[index].OpponentsCost = parseInt(event.target.value)
                                return [...currentMistakes]
                            })
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

            <h3>Баллы команд</h3>
            <div style={{ display: "inline-block" }}>
                Очки команды "{Team1.Name}":<input style={{ display: "inline-block" }} type="number" value={Team1Points} onChange={handleTeam1PointsChange} />
            </div>
            <div style={{ display: "inline-block" }}>
                Очки команды "{Team2.Name}":<input style={{ display: "inline-block" }} type="number" value={Team2Points} onChange={handleTeam2PointsChange} />
            </div>
            <div style={{ display: "inline-block" }}>
                Очки жюри:<input style={{ display: "inline-block" }} type="number" value={AssessorPoints} onChange={handleAssessorPointsChange} />
            </div>
            <input style={{ display: "inline-block", background: "black", color: "white" }} type="button" value="Зафиксировать результаты и закончить раунд" onClick={handleRoundEnd} />
            <h3>События</h3>
            <input style={{ display: "inline-block" }} type="button" value="Зафиксировать частичную перемену ролей" onClick={handlePartialRolesChangeFixation} />
            <input style={{ display: "inline-block" }} type="button" value="Зафиксировать полную перемену ролей" onClick={handleFullRolesChangeFixation} /><br />
            {
                Challenge.Round.RolesChange !== null && Round.RolesChange !== undefined ?
                    "Произошла ".concat(
                        Round.RolesChange?.FullRoleChange ? "полная" : "частичная").concat(
                            " перемена ролей") :
                    "Не было перемены ролей"
            }
            <br /><br />
            <input style={{ display: "inline-block" }} type="button" value="Зафиксировать отсутствие решения" onClick={handleNoSolutionFixation} /><br />
            {
                Challenge.Round.NoSolution ?
                    "Зафиксировано отсутствие решения" :
                    "Решение присутствует"
            }
            <br />

            <br />
            <input style={{ display: "inline-block" }} type="button" value="Зафиксировать перерыв" onClick={handleBreakDeclaration} />
            Команда запрашивающая перерыв:<select value={BreakRequestingTeamId} onChange={HandleBreakRequestingTeamIdChange}>
                <option value={Team1.Id}>
                    Команда "{Team1.Name}"
                </option>
                <option value={Team2.Id}>
                    Команда "{Team2.Name}"
                </option>
            </select>
            <BreaksInfo Breaks={Round?.Breaks} /><br /><br />
            <div >
                <input style={{ display: "inline-block" }} type="button" value="Зафиксировать замену" onClick={handleChangeDeclaration} />
                Команда запрашивающая замену:<select value={ChangeRequestingTeamId} onChange={HandleChangeRequestingTeamIdChange}>
                    <option value={Team1.Id}>
                        Команда "{Team1.Name}"
                    </option>
                    <option value={Team2.Id}>
                        Команда "{Team2.Name}"
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
                <ChangesInfo Changes={Round?.Changes} />
            </div>
        </>
    )
}