import { useState } from "react"
import type { Challenge } from "../../Models/Challenge"
import type { Round } from "../../Models/Round"
import type { Team } from "../../Models/Team"

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
            <h3> Описание Раунда</h3>
            Задача:{Challenge.Task?.Text}<br />
            Проверка корректности: {Challenge.IsCheckingCorrectness ? "да" : "нет"}<br />
            {Round !== null && Round !== undefined ? <>
                Изначальный Докладчик:{Round.Speaker?.Surname} {Round?.Speaker?.Name} {Round.Speaker?.Fatname} Email:{Round?.Speaker?.Email}<br />
                Команда Докладчика: {SpeakerTeam?.Name}<br />
                Изначальный Оппонент:{Round?.Opponent?.Surname} {Round?.Opponent?.Name} {Round.Opponent?.Fatname} Email:{Round?.Opponent?.Email}<br />
                Команда Оппонента: {OpponentTeam?.Name}<br />
                <br />
                {
                    Round.RolesChange !== null && Round.RolesChange !== undefined ?
                        <>
                            Произошла {Round.RolesChange.FullRoleChange ? "полная" : "частичная"} перемена ролей<br />
                            Время перемены ролей: {Round.RolesChange.ChangeTime}
                        </>
                        : null
                }<br />
                Вызов был {Round.RoundResults?.Correctness ? "Корректным" : "Некорректным"}
                <table>
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
                Очки первой команды:{Round.RoundResults?.Team1Points}                Очки второй команды:{Round.RoundResults?.Team2Points}<br />
                {
                    Round.Breaks.$values.length > 0 ?
                        <>
                            <h3>Перерывы объявленные в этом раунде</h3>
                            <table>
                                <thead>
                                    <tr>
                                        <th>
                                            Время объявления
                                        </th>
                                        <th>
                                            Идентификатор команды инициализатора
                                        </th>
                                        <th>
                                            Название попросившей перерыв команды
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {Round.Breaks.$values.map((Break) => {
                                        return (
                                            <>
                                                <tr>
                                                    <th>
                                                        {Break.DeclareTime.toString()}
                                                    </th>
                                                    <th>
                                                        {Break.InitiatorTeamId}
                                                    </th>
                                                    <th>
                                                        {Break.InitiatorTeam.Name}
                                                    </th>
                                                </tr>
                                            </>
                                        )
                                    })}
                                </tbody>
                            </table>
                        </>
                        :
                        <>
                            Перерывов не произошло
                        </>
                }
                <br />
                {
                    Round.Changes.$values.length > 0 ?
                        <>
                            <h3>Замены объявленные в этом раунде</h3>
                            <table>
                                <thead>
                                    <tr>
                                        <th>
                                            Время объявления
                                        </th>
                                        <th>
                                            Идентификатор команды инициализатора
                                        </th>
                                        <th>
                                            Название попросившей перерыв команды
                                        </th>
                                        <th>
                                            Идентификатор нового участника от указанной команды
                                        </th>
                                        <th>
                                            Наименование участника
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {Round.Changes.$values.map((Change) => {
                                        return (
                                            <>
                                                <tr>
                                                    <th>
                                                        {Change.DeclareTime.toString()}
                                                    </th>
                                                    <th>
                                                        {Change.InitiatorTeamId}
                                                    </th>
                                                    <th>
                                                        {Change.InitiatorTeam.Name}
                                                    </th>
                                                    <th>
                                                        {Change.NewParticipantId}
                                                    </th>
                                                    <th>
                                                        {Change.NewParticipant.Surname} {Change.NewParticipant.Name} {Change.NewParticipant.Fatname}
                                                    </th>
                                                </tr>
                                            </>
                                        )
                                    })}
                                </tbody>
                            </table>
                        </>
                        :
                        <>
                            Замен не произошло
                        </>
                }
            </>
                : null
            }
        </>
    )
}