import { useEffect } from "react"
import { type Game } from "../Models/Game"
import { useNavigate } from "react-router"
export default function GameCard(game: Game) {
    const navigate = useNavigate()
    function JoinGame() {
        if (game.game.StartTime === undefined || game.game.StartTime === null) {
            navigate("/GameStartConfirmation/" + game.game.Id)
        }
        else
            if (game.game.TaskSolvingStartTime === undefined || game.game.TaskSolvingStartTime === null) {
                navigate("/PreStartActions/" + game.game.Id)
            }
            else {
                let endDate = new Date(game.game.StartTime)
                endDate.setMinutes(endDate.getMinutes() + game.game.SolvingTime)
                if (endDate > new Date()) {
                    navigate("/Timer/" + game.game.Id)
                }
                else {
                    if (game.game.CaptainsRound === undefined || game.game.CaptainsRound === null) {
                        navigate("/CaptainsCompetetion/" + game.game.Id)
                    }
                    else
                        navigate("/GameInterface/" + game.game.Id)
                }
            }
    }

    return (
        <>
            <td>
                {game.game.Name}
            </td>
            <td>
                {game.game.StartTime === null || game.game.StartTime === undefined ? "Бой запланирован" :
                    game.game.GameEnded ? "Бой закончен" : "Бой идет"
                }
            </td>
            <td>
                {game.game.Assessor !== null && game.game.Assessor !== undefined ?
                    <>
                        {game.game.Assessor.Name}  {game.game.Assessor.Surname} {game.game.Assessor.Fatname}
                    </>
                    :
                    "Член жюри еще не начал бой"
                }
            </td>
            <td>
                {game.game.Tasks.$values.length}
            </td>
            <td>
                {game.game.EventPlace}
            </td>
            <td>
                {game.game.Team1.Students.$values.length} - {game.game.Team2.Students.$values.length}
            </td>
            <td>
                <input type="button" value="Присоединиться" onClick={JoinGame} />
            </td>
        </>
    )
}