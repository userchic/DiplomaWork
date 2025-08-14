import { useNavigate, useParams } from "react-router";
import { ConfirmSolvingStart, ConfirmStart, DownloadTasks, SendTasks } from "../../Services/GameService";

export default function PreStartActions() {
    const params = useParams()
    const gameId = params.gameId
    const navigate = useNavigate();

    function Confirm(): void {
        const confirmSolvingStart = async () => {
            const res = await ConfirmSolvingStart(gameId)
            if (res.success)
                navigate("/Timer/" + gameId)//навигация к игровому процессу
        }
        confirmSolvingStart()
    }

    function EmailSendTasks(): void {
        const send = async () => {
            const tasks = await SendTasks(gameId)
            alert(tasks.message)
        }
        send()
    }

    function PrintDownloadTasks(): void {
        const print = async () => {
            const res = await DownloadTasks(gameId)
        }
        print()
    }

    return (
        <>
            <center><input type="button" onClick={EmailSendTasks} value="Отправить игрокам тексты задач" /></center>
            <center><input type="button" onClick={PrintDownloadTasks} value="Распечатать тексты задач" /></center>
            <center><input type="button" onClick={Confirm} value="Зафиксировать начало решения задач" /></center>
        </>
    )
}