import type { Break } from "./Break"
import type { Change } from "./Change"
import type { Mistake } from "./Mistake"
import type { RolesChange } from "./RolesChange"
import type { RoundResults } from "./RoundResults"
import type { Student } from "./Student"
import type { Task } from "./Task"

export interface Round {
    Id?: number,
    RoundNumber: number,
    SpeakerId?: number,
    Speaker?: Student,
    OpponentId?: number,
    Opponent?: Student,
    StartTime: Date,
    NoSolution?: boolean,
    ChallengeId?: number,
    RoundResults?: RoundResults,
    RolesChange?: RolesChange,
    Changes: { $values: Change[] },
    Breaks: { $values: Break[] }
}
export interface StartRoundRequest {
    SpeakerId?: number,
    OpponentId?: number,
    GameId: number
}
export interface EndRoundRequest {
    Team1Points: number,
    Team2Points: number,
    AssessorPoints: number,
    Mistakes: Mistake[]
}