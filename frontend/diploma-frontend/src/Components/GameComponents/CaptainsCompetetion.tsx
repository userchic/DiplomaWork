import { useEffect, useState } from "react"
import { GetGame, SetCaptainsRoundWinner } from "../../Services/GameService";
import { useNavigate, useParams } from "react-router";
import type { Game } from "../../Models/Game";
import type { Student } from "../../Models/Student";

export default function CaptainsCompetetion() {

    const params = useParams()
    const gameId = params.gameId

    const [Team1Name, setTeam1Name] = useState("")
    const [Team2Name, setTeam2Name] = useState("")
    const [Team1Id, setTeam1Id] = useState(0)
    const [Team2Id, setTeam2Id] = useState(0)
    const [Team1ParticipantName, setTeam1ParticipantName] = useState("")
    const [Team2ParticipantName, setTeam2ParticipantName] = useState("")
    const [RoundFormat, setRoundFormat] = useState("")
    const navigate = useNavigate()
    useEffect(() => {
        const getGame = async () => {
            const res: Game = await GetGame(gameId)
            setTeam1Name(res.Team1.Name)
            setTeam2Name(res.Team2.Name)
            setTeam1Id(res.Team1Id)
            setTeam2Id(res.Team2Id)
            setRoundFormat(res.CaptainsRoundFormat)
            let team1Stud: Student = res.Team1.Students.$values.find((student) =>
                student.Id === res.Team1.CaptainId
            )
            setTeam1ParticipantName(team1Stud.Name + " " + team1Stud.Surname + " " + team1Stud.Fatname)
            let team2Stud: Student = res.Team2.Students.$values.find((student) =>
                student.Id === res.Team2.CaptainId
            )
            setTeam2ParticipantName(team2Stud.Name + " " + team2Stud.Surname + " " + team2Stud.Fatname)

        }
        getGame()
    }, [])
    function ChooseTeam1(): void {
        const setWinner = async () => {
            const res = await SetCaptainsRoundWinner(gameId, Team1Id)
            if (res.message !== undefined)
                alert(res.message)
            else
                alert(Object.values(res.errors)[0])
            if (res.success)
                navigate("/GameInterface/" + gameId)
        }
        setWinner()
    }

    function ChooseTeam2(): void {
        const setWinner = async () => {
            const res = await SetCaptainsRoundWinner(gameId, Team2Id)
            if (res.message !== undefined)
                alert(res.message)
            else
                alert(Object.values(res.Errors)[0])
            if (res.success)
                navigate("/GameInterface/" + gameId)
        }
        setWinner()

    }

    return (
        <>
            <center><h2>Капитанский раунд</h2></center>
            <div className="block">
                <center>Формат раунда:{RoundFormat}</center><br /><br />
                <div style={{ display: "flex" }}>
                    <div style={{ flex: "1", display: "inline-block" }} className="block">
                        Команда 1: <b>{Team1Name}</b> <br />
                        Участник от команды 1: {Team1ParticipantName}<br />
                        <input type="button" value="Назначить победителем" onClick={ChooseTeam1} />
                    </div>
                    <div style={{ flex: "0.5", display: "inline-block", margin: "auto" }} className="block">
                        Назначить победителя нужно по правилам
                    </div>
                    <div style={{ flex: "1", display: "inline-block" }} className="block">
                        Команда 2: <b>{Team2Name}</b> <br />
                        Участник от команды 2: {Team2ParticipantName}<br />
                        <input type="button" value="Назначить победителем" onClick={ChooseTeam2} />
                    </div>
                </div>
            </div>
        </>
    )
}