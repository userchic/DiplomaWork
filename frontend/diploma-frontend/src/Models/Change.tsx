import type { Student } from "./Student";
import type { Team } from "./Team";

export interface Change {
    Id?: number,
    DeclareTime: Date,
    RoundId?: number,
    RoleId: number,
    NewParticipantId?: number,
    NewParticipant?: Student,
    InitiatorTeamId?: number,
    InitiatorTeam?: Team
}