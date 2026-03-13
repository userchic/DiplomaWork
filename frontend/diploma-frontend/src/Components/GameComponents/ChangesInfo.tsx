import type { Change } from "../../Models/Change"
interface Props {
    Changes: { $values: Change[] }
}

export default function ChangesInfo({ Changes }: Props) {
    return (
        <>
            {
                Changes.$values.length > 0 ?
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
                                {Changes.$values.map((Change) => {
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
    )
}