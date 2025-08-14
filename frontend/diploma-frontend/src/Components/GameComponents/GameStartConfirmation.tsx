import { useNavigate, useParams } from "react-router";
import { ConfirmStart } from "../../Services/GameService"

export default function GameStartConfirmation() {
    const params = useParams()
    const gameId = params.gameId
    const navigate = useNavigate();
    function Confirm(): void {
        const confirmStart = async () => {
            const res = await ConfirmStart(gameId)
            alert(res.message)
            if (res.success)
                navigate("/PreStartActions/" + gameId)
        }
        confirmStart()
    }

    return (
        <>
            <center><input type="button" onClick={Confirm} value="Подтвердить начало игры" /></center>
        </>
    )
}