import type { Round } from "./Round";
import type { Task } from "./Task";

export interface Challenge {
    Id?: number,
    GameId?: number,
    DeclareTime?: Date,
    TaskId?: number,
    RequestingTeamId?: number,
    IsCheckingCorrectness: boolean,
    IsChallengeAccepted: boolean,
    Round?: Round
    Task?: Task
}