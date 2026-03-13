
import type { Game } from "./Game"
import type { Student } from "./Student"
export interface CaptainsRound {
    GameId: number
    Game: Game
    Participant1Id: number
    Participant1: Student
    Participant2Id: number
    Participant2: Student
    WinnerId: number
}
