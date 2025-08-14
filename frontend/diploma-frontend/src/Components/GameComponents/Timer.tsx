import { useEffect, useState } from "react"
import { GetGame } from "../../Services/GameService"
import type { Game } from "../../Models/Game"
import { useNavigate, useParams } from "react-router";

export default function Timer() {
    const params = useParams()
    const gameId = params.gameId
    let Minutes = 0
    let Seconds = 0
    let timerIndex: number
    const [minutes, setMinutes] = useState(0);
    const [seconds, setSeconds] = useState(0);
    const navigate = useNavigate();
    useEffect(() => {
        const getSolvingLength = async () => {
            const res: Game = await GetGame(gameId)
            Minutes = res.SolvingTime
            setMinutes(res.SolvingTime)
            clearInterval(timerIndex)
            timerIndex = setInterval(() => {
                if (Seconds > 0) {
                    Seconds = Seconds - 1
                    setSeconds(Seconds)
                }
                else
                    if (Minutes > 0) {
                        Minutes = Minutes - 1
                        Seconds = 59
                        setMinutes(Minutes)
                        setSeconds(59)
                    }
                    else {
                        clearInterval(timerIndex)
                        navigate("/CaptainsCompetetion/" + gameId)
                    }
            }, 1000)

        }
        getSolvingLength()
        return () => clearInterval(timerIndex)
    }, [])
    return (
        <>
            <center>
                Таймер отсчета времени до начала выступлений:<br />
                {minutes} : {seconds}
            </center>
        </>
    )
}