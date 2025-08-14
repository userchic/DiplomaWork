import { useEffect, useState } from "react";
import {Link} from "react-router";
import GameCard from "./GameCard";
import { GetGames } from "../Services/AdminService";
import type { Game } from "../Models/Game";

export default function Games()
{
    const [Loading,setLoading] = useState(true)
    const [games,setGames] = useState<Game[]>([])
    useEffect (() => {
        const getGames = async () => {
        const games=await GetGames()
        setGames(games)
        setLoading(false)
        }
        getGames()
    },[])
    return (
    <>
        <h1>Список игр</h1>
        <Link to="/GameCreate">Создать игры</Link><br></br>
        
        {Loading ?
             <>Загрузка игр</>
             :
             <>
             <table>
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
                    {games.map((game:Game)=>(
                        <tr>
                            <GameCard game={game} key={game.Id}/>
                        </tr>
                    ))}
                    
                </tbody>
            </table>
            </>
        }
        </>
    )
}