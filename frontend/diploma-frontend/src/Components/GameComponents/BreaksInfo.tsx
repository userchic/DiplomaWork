import type { Break } from "../../Models/Break"

interface Props {
    Breaks: { $values: Break[] }
}

export default function BreaksInfo({ Breaks }: Props) {
    return (
        <>
            {
                Breaks.$values.length > 0 ?
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
                                {Breaks.$values.map((Break) => {
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
        </>
    )
}