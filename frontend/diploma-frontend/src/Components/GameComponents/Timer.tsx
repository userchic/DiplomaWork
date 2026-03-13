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
            let endDate = new Date(res.TaskSolvingStartTime)
            endDate.setMinutes(endDate.getMinutes() + res.SolvingTime)
            let currentTime = new Date()
            let timer = -(currentTime.getTime() - endDate.getTime()) / 1000
            if (timer > 0) {
                Minutes = Math.floor(timer / 60)
                setMinutes(Math.floor(timer / 60))
                Seconds = Math.floor(timer % 60)
                setSeconds(Math.floor(timer % 60))
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
            else
                navigate("/CaptainsCompetetion/" + gameId)
        }
        getSolvingLength()
        return () => clearInterval(timerIndex)
    }, [])
    return (
        <>
            <center>
                <h3>Время решения задач</h3>
                Таймер отсчета времени до начала выступлений:<br />
                {minutes} : {seconds.toString().padStart(2, '0')}
            </center>
        </>
    )
}