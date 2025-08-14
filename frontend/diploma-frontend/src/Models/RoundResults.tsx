import type { Mistake } from "./Mistake";

export interface RoundResults {
    Id?: number,
    RoundId?: number,
    Team1Points: number,
    Team2Points: number,
    Correctness: boolean,
    Mistakes: { $values: Mistake[] }
}