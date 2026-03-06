import { useEffect, useState } from "react";
import { Link } from "react-router";
import GameCard from "./GameCard";
import { GetGames } from "../Services/AdminService";
import type { Game } from "../Models/Game";

export default function Games() {
    const [Loading, setLoading] = useState(true)
    const [games, setGames] = useState<Game[]>([])
    const [IsEnd, setIsEnd] = useState(false)
    const [Page, setPage] = useState(1)
    const getGames = async () => {
        const newGames: Game[] = await GetGames(Page)
        if (newGames.length == 0)
            setIsEnd(true)
        setGames([...games.concat(newGames)])
        setLoading(false)
        setPage(Page + 1)
    }
    useEffect(() => {

        getGames()
    }, [])

    return (
        <>
            <h1>Список игр</h1>
            <Link to="/GameCreate">Создать игры</Link><br></br>

            {Loading ?
                <>Загрузка игр</>
                :
                <>
                    <table border="1px" class="table">
                        <thead>
                            <tr>
                                <th>
                                    Название игры
                                </th>
                                <th>
                                    Кол-во участников
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {games.map((game: Game) => (
                                <tr>
                                    <GameCard game={game} key={game.Id} />
                                </tr>
                            ))}

                        </tbody>
                    </table>
                    {IsEnd ? null
                        :
                        <input type="button" value="Ещё" onClick={getGames} />}
                </>
            }
        </>
    )
}