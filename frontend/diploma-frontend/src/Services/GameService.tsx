import { mapJson } from "../JsonRefParser";
import type { EndRoundRequest, StartRoundRequest } from "../Models/Round";
import { hostName } from "./HostName";
let controllerName = "Game"
export const GetGame = async (gameId: number) => {
    let requestLine = hostName + "/" + controllerName + "/GetGame?gameId=" + gameId
    let response = await fetch(requestLine, {
        credentials: "include",
    })
    return await response.json().then((data) => mapJson(JSON.parse(data)))
}
export const ConfirmStart = async (gameId: number) => {
    let requestLine = hostName + "/" + controllerName + "/ConfirmStart?gameId=" + gameId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify({ id: gameId })
    })
    return await response.json().then((data) => data)
}
export const DownloadTasks = async (gameId: number) => {
    let requestLine = hostName + "/" + controllerName + "/DownloadTasks?gameId=" + gameId
    let response = await fetch(requestLine, {
        method: "GET",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        }
    })
    if (response.ok) {
        const blob = await response.blob()
        const blobUrl = URL.createObjectURL(blob);
        saveFile(blobUrl, "Задачи.docx")
        URL.revokeObjectURL(blobUrl);
    }
}
export const SendTasks = async (gameId: number) => {
    let requestLine = hostName + "/" + controllerName + "/SendTasks?gameId=" + gameId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify({ id: gameId })
    })
    return await response.json().then((data) => data)
}
export const SetCaptainsRoundWinner = async (gameId: number, winnerTeam: number) => {
    let requestLine = hostName + "/" + controllerName + "/SetCaptainsRoundWinner?gameId=" + gameId + "&winnerTeamId=" + winnerTeam
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify({
            winnerTeam: winnerTeam
        })
    })
    return await response.json().then((data) => data)
}
export const ConfirmSolvingStart = async (gameId: number) => {
    let requestLine = hostName + "/" + controllerName + "/ConfirmSolvingStart?gameId=" + gameId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
    })
    return await response.json().then((data) => data)
}
export const FixateChallenge = async (gameId: number, taskId: number) => {
    let requestLine = hostName + "/" + controllerName + "/FixateChallenge?gameId=" + gameId + "&taskId=" + taskId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
    })
    return await response.json().then((data) => data)
}
export const RejectToChallenge = async (gameId: number) => {
    let requestLine = hostName + "/" + controllerName + "/RejectToChallenge?gameId=" + gameId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
    })
    return await response.json().then((data) => data)
}
export const ConfirmCorrectnessCheck = async (gameId: number) => {
    let requestLine = hostName + "/" + controllerName + "/ConfirmCorrectnessCheck?gameId=" + gameId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
    })
    return await response.json().then((data) => data)
}
export const ConfirmChallengeAcceptance = async (gameId: number) => {
    let requestLine = hostName + "/" + controllerName + "/ConfirmChallengeAcceptance?gameId=" + gameId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
    })
    return await response.json().then((data) => data)
}
export const StartNewRound = async (request: StartRoundRequest) => {
    let requestLine = hostName + "/" + controllerName + "/StartNewRound"
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify(request)
    })
    return await response.json().then((data) => data)
}
export const EndRound = async (gameId: number, endRoundRecord: EndRoundRequest) => {
    let requestLine = hostName + "/" + controllerName + "/EndRound?gameId=" + gameId
    let JsonString = JSON.stringify(endRoundRecord)
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
        body: JsonString
    })
    return await response.json().then((data) => data)
}
export const EndGame = async (gameId: number) => {
    let requestLine = hostName + "/" + controllerName + "/EndGame?gameId=" + gameId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
    })
    return await response.json().then((data) => data)
}
export const DeclareBreak = async (gameId: number, initiatorTeamId: number) => {
    let requestLine = hostName + "/" + controllerName + "/DeclareBreak?gameId=" + gameId + "&initiatorTeamId=" + initiatorTeamId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
    })
    return await response.json().then((data) => data)
}
export const DeclareChange = async (gameId: number, initiatorTeamId: number, newParticipantId: number) => {
    let requestLine = hostName + "/" + controllerName + "/DeclareChange?gameId=" + gameId + "&initiatorTeamId=" + initiatorTeamId + "&newParticipantId=" + newParticipantId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
    })
    return await response.json().then((data) => data)
}
export const DeclareRoleChange = async (gameId: number, isFullRoleChange: boolean) => {
    let requestLine = hostName + "/" + controllerName + "/DeclareRoleChange?gameId=" + gameId + "&isFullRoleChange=" + isFullRoleChange ? 1 : 0
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify({
            isFullRoleChange: isFullRoleChange
        })
    })
    return await response.json().then((data) => data)
}
export const ConfirmNoSolution = async (gameId: number) => {
    let requestLine = hostName + "/" + controllerName + "/ConfirmNoSolution?gameId=" + gameId
    let response = await fetch(requestLine, {
        method: "POST",
        credentials: "include",
        headers: {
            "Content-type": "application/json"
        },
    })
    return await response.json().then((data) => data)
}
function saveFile(url: string, filename: string) {
    const a = document.createElement("a");
    a.href = url;
    a.download = filename || "file-name";
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}