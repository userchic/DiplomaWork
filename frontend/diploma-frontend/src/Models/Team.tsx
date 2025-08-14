import type { Student } from "./Student";

export interface Team {
    Id: number,
    Name: string,
    CaptainId: number,
    ViceCaptainId: number,
    Students: { $values: Student[] }
}