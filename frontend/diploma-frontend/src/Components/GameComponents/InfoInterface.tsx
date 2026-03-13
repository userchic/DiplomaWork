import { useState } from "react"
import type { Challenge } from "../../Models/Challenge"
import type { Round } from "../../Models/Round"
import type { Team } from "../../Models/Team"
import ChangesInfo from "./ChangesInfo"
import BreaksInfo from "./BreaksInfo"

interface Props {
    Challenge: Challenge,
    Team1: Team,
    Team2: Team,
}
export default function infoInterface({ Challenge, Team1, Team2 }: Props) {

    let Round = Challenge.Round

    const [SpeakerId] = useState(Round?.SpeakerId)
    const [OpponentId] = useState(Round?.OpponentId)

    const [SpeakerTeam] = useState(SpeakerId == undefined ? null : Team1.Students.$values.find(x => x.Id == SpeakerId) === undefined ? Team2 : Team1)
    const [OpponentTeam] = useState(OpponentId == undefined ? null : Team1.Students.$values.find(x => x.Id == OpponentId) === undefined ? Team2 : Team1)

    return (
        <>
            <h3> Описание хода раунда</h3>
            <b>Задача:</b> {Challenge.Task?.Text}<br />
            <b>Проверка корректности:</b> {Challenge.IsCheckingCorrectness ? "да" : "нет"}<br />
            Вызов был <b>{Round?.RoundResults?.Correctness ?
                "Корректным" :
                "Некорректным"}</b>
            {Round !== null && Round !== undefined ?
                <>

                    <h3>Выступающие</h3>
                    <table style={{ width: "40%" }} className="table">
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
                                    {Round?.Opponent?.Surname} {Round?.Opponent?.Name} {Round?.Opponent?.Fatname}
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
                    <div style={{ display: "flex" }}>
                        <div style={{ flex: "1" }}>
                            <h3>Баллы команд</h3>
                            <table style={{ width: "40%" }} className="table">
                                <thead >
                                    <tr>
                                        <th>
                                            Команда "{Team1.Name}"
                                        </th>
                                        <th>
                                            Команда "{Team2.Name}"
                                        </th>
                                    </tr>
                                </thead>
                                <tbody >
                                    <td >
                                        {Round.RoundResults?.Team1Points}
                                    </td>
                                    <td>
                                        {Round.RoundResults?.Team2Points}
                                    </td>
                                </tbody>
                            </table>

                            <br />
                        </div>
                        <div style={{ flex: "1" }}>
                            <h3>Ошибки</h3>
                            <table className="table" style={{ width: "40%" }}>
                                <thead>
                                    <tr>
                                        <th>
                                            Описание ошибки
                                        </th>
                                        <th>
                                            Баллы жюри
                                        </th>
                                        <th>
                                            Баллы оппонента
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {Round.RoundResults?.Mistakes.$values.map((mistake) => {
                                        return (
                                            <tr>
                                                <td>
                                                    {mistake.Text}
                                                </td>
                                                <td>
                                                    {mistake.JureCost}
                                                </td>
                                                <td>
                                                    {mistake.OpponentsCost}
                                                </td>
                                            </tr>
                                        )
                                    })}
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <h3>События</h3>
                    {
                        Round.NoSolution ?
                            "Зафиксировано отсутствие решения" :
                            "Решение присутствует"
                    }
                    <br /><br />
                    {
                        Round.RolesChange !== null && Round.RolesChange !== undefined ?
                            <>
                                Произошла <b>{Round.RolesChange.FullRoleChange ? "полная" : "частичная"} перемена ролей </b><br />
                                Время перемены ролей: {Round.RolesChange.ChangeTime}
                            </>
                            : "Не было перемены ролей"
                    }
                    <br /><br />
                    <BreaksInfo Breaks={Round.Breaks} />
                    <br /><br />
                    <ChangesInfo Changes={Round.Changes} />
                </>
                : null
            }
        </>
    )
}