
import type { Challenge } from "./Challenge"
import type { Task } from "./Task"
import { type Team } from "./Team"
export interface Game {
    Id: number,
    Name: String,
    StartTime: Date,
    TaskSolvingStartTime: Date,
    SolvingTime: number
    Team1Id: number,
    Team2Id: number,
    AssessorPoints: number,
    Team1Points: number,
    Team2Points: number,
    CaptainsRoundFormat: string,
    AssessorId: number,
    GameEnded: Boolean,
    ChallengingTeamId: number,
    TeamRejectedToChallenge: boolean,
    CaptainsRound: Object,
    Team1: Team,
    Team2: Team,
    Tasks: { $values: Task[] }
    Challenges: { $values: Challenge[] }
}
