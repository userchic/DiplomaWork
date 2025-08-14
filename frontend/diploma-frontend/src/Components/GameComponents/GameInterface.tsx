import { useEffect, useState, type ChangeEvent } from "react";
import type { Game } from "../../Models/Game";
import { EndGame, EndRound, GetGame, RejectToChallenge } from "../../Services/GameService";
import { useNavigate, useParams } from "react-router";
import type { EndRoundRequest, Round } from "../../Models/Round";
import type { Task } from "../../Models/Task";
import type { Mistake } from "../../Models/Mistake";
import RoundInterface from "./RoundInterface";
import type { Challenge } from "../../Models/Challenge";

export default function GameInterface() {
    const params = useParams()
    let gameId: number = parseInt(params.gameId)
    const [Challenges, setChallenges] = useState<Challenge[]>([])
    const [Game, setGame] = useState<Game>()
    useEffect(() => {
        getGame()
    }, [])
    const getGame = async () => {
        const res: Game = await GetGame(gameId)
        setGame(res)
        if (!res.GameEnded)
            if (res.Challenges.$values.length == 0 || (res.Challenges.$values[res.Challenges.$values.length - 1].Round?.RoundResults !== null && res.Challenges.$values[res.Challenges.$values.length - 1].Round?.RoundResults !== undefined)) {
                res.Challenges.$values.push({
                    IsCheckingCorrectness: false,
                    IsChallengeAccepted: false,
                    GameId: gameId,
                })
            }
        setChallenges(res.Challenges.$values)
    }
    function SwapChallengingTeams() {
        setGame((currentGame) => {
            currentGame.ChallengingTeamId = currentGame.Team1Id + currentGame.Team2Id - currentGame.ChallengingTeamId
            return currentGame
        })
    }
    async function endRound(gameId: number, challenge: Challenge, EndRoundRequest: EndRoundRequest): Promise<boolean> {
        const endRound = async () => {
            const res = await EndRound(gameId, EndRoundRequest)
            alert(res.message)
            let result: boolean = res.success
            if (res.success) {
                Game.Team1Points += EndRoundRequest.Team1Points
                Game.Team2Points += EndRoundRequest.Team2Points
                let Correctness = true
                if (challenge.IsCheckingCorrectness && challenge.Round?.NoSolution)
                    Correctness = false
                if (Correctness && !Game.TeamRejectedToChallenge)
                    SwapChallengingTeams()
                Challenges[Challenges.length - 1].Round.RoundResults =
                {
                    Correctness: Challenges[Challenges.length - 1].IsCheckingCorrectness && Challenges[Challenges.length - 1].Round.NoSolution ? true : false,
                    Mistakes: { $values: EndRoundRequest.Mistakes },
                    Team1Points: EndRoundRequest.Team1Points,
                    Team2Points: EndRoundRequest.Team2Points,
                }
                if (Challenges.length < Game.Tasks.$values.length)
                    setChallenges((currentChallenges) => {
                        currentChallenges.push({
                            GameId: Game.Id,
                            IsCheckingCorrectness: false,
                            IsChallengeAccepted: false
                        })
                        return currentChallenges
                    })
            }
            return result
        }
        return await endRound()
    }
    async function handleEndGame() {
        const res = await EndGame(gameId)
        alert(res.message)
        if (res.success) {
            getGame()
        }
    }

    return (
        <>
            <center>
                <div style={{ display: "inline-block" }}>
                    Команда 1: {Game?.Team1.Name} <br />
                    Очки: {Game?.Team1Points}
                </div>
                {
                    !Game?.GameEnded ?
                        <input type="button" value="Закончить игру" onClick={handleEndGame} /> :
                        null
                }
                <div style={{ display: "inline-block" }}>
                    Команда 2: {Game?.Team2.Name} <br />
                    Очки: {Game?.Team2Points}
                </div>
            </center>
            {
                Challenges?.map((round) =>
                    <RoundInterface
                        challenge={round}
                        Game={Game}
                        EndGame={handleEndGame}
                        EndRound={endRound}
                        RejectToChallenge={async () => {
                            const res = await RejectToChallenge(gameId)
                            alert(res.message)
                            if (res.success) {
                                if (Game.TeamRejectedToChallenge)
                                    getGame()
                                else {
                                    Game.TeamRejectedToChallenge = true
                                    SwapChallengingTeams()
                                }
                            }
                            let result: boolean = res.success
                            return result
                        }}
                    />
                )
            }
        </>
    )
}
