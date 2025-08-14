import { useState, type ChangeEvent } from "react"
import type { Task } from "../../Models/Task"
import { RejectToChallenge } from "../../Services/GameService"
interface Props {
    setState: (newState: string) => void
    Tasks: Task[],
    CallingTeamName: string,
    //команда отказалась вызывыать
    ChallengeCall: (TaskId: number) => Promise<boolean>,
    RejectToChallenge: () => Promise<boolean>
}
export default function ChallengeInterface({ setState, Tasks, CallingTeamName, ChallengeCall, RejectToChallenge }: Props) {

    const [ChosenTaskId, setChosenTaskId] = useState(Tasks.length > 0 ? Tasks[0].Id : null)

    async function handleChallengeCall() {
        if (await ChallengeCall(ChosenTaskId))
            setState("CallAcceptRejection")
    }
    async function handleRejectToChallenge() {
        if (await RejectToChallenge) {
            setState("LastTeamWantsToPerform")
        }
    }
    function handleChosenTaskChange(event: ChangeEvent<HTMLSelectElement>): void {
        setChosenTaskId(parseInt(event.target.value))
    }
    return (
        <>
            <div style={{ display: "inline-block" }}>
                <input type="button" value="Зафиксировать вызов" onClick={handleChallengeCall} />
                <br />
                Вызывающая команда: "{CallingTeamName}"<br />
                Задача:
                <select value={ChosenTaskId} onChange={handleChosenTaskChange} >
                    {Tasks.map((task) => {
                        return (
                            <option value={task.Id}>
                                {task.Text}
                            </option>
                        )
                    })}
                </select>
            </div>
            <br />
            <div style={{ display: "inline-block" }}>
                <input type="button" value="Зафиксировать отказ от вызова" onClick={handleRejectToChallenge} />
            </div>
        </>
    )
}