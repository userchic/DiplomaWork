export interface CreateGameRequest
{
    SolvingTime:number,
    Name:string,
    captainsRoundFormat:string,
    chosenTasksIds:number[],
    studentsTeam1:number[],
    team1Name:string,
    team1CaptainId:number,
    team1ViceCaptainId?:number,
    studentsTeam2:number[],
    team2Name:string,
    team2CaptainId:number,
    team2ViceCaptainId?:number
}