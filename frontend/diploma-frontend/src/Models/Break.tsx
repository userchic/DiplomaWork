import type { Team } from "./Team";

export interface Break {
    Id?: number,
    RoundId?: number,
    InitiatorTeamId: number,
    InitiatorTeam: Team,
    DeclareTime: Date
}