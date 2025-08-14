import { mapJson } from "../JsonRefParser";
import type { CreateGameRequest } from "../Models/CreateGameRequest";
import type { GenerateTaskRequest } from "../Models/GenerateTaskRequest";
import type { Student } from "../Models/Student";
import type { Task } from "../Models/Task";
import { hostName } from "./HostName";
let controllerName = "Admin"


export const GetGames = async () => {
  let requestLine = hostName + "/" + controllerName + "/GetGames"
  let response = await fetch(requestLine, {
    credentials: "include",
  })
  return await response.json().then((data) => mapJson(JSON.parse(data).$values))
}

export const GetTasks = async () => {
  let requestLine = hostName + "/" + controllerName + "/GetTasks"
  let response = await fetch(requestLine, {
    credentials: "include",
  })
  return await response.json().then((data) => JSON.parse(data))
}
export const GetTask = async (id: number) => {
  let requestLine = hostName + "/" + controllerName + "/GetTask" + "?id=" + id
  let response = await fetch(requestLine, {
    credentials: "include",
  })
  return await response.json().then((data) => data)
}
export const GetStudents = async () => {
  let requestLine = hostName + "/" + controllerName + "/GetStudents"
  let response = await fetch(requestLine, {
    credentials: "include",
  })
  return await response.json().then((data) => JSON.parse(data))
}
export const GetStudent = async (id: number) => {
  let requestLine = hostName + "/" + controllerName + "/GetStudent" + "?id=" + id
  let response = await fetch(requestLine, {
    credentials: "include",
  })
  return await response.json().then((data) => data)
}
export const GenerateTask = async (request: GenerateTaskRequest) => {
  let requestLine = hostName + "/" + controllerName + "/GenerateTask"
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
export const CreateGame = async (game: CreateGameRequest) => {
  let requestLine = hostName + "/" + controllerName + "/CreateGame"
  let response = await fetch(requestLine, {
    method: "POST",
    credentials: "include",
    headers: {
      "Content-type": "application/json"
    },
    body: JSON.stringify(game)
  })
  return await response.json().then((data) => data)
}
export const CreateStudent = async (student: Student) => {
  let requestLine = hostName + "/" + controllerName + "/CreateStudent"
  let response = await fetch(requestLine, {
    method: "POST",
    credentials: "include",
    headers: {
      "Content-type": "application/json"
    },
    body: JSON.stringify(student)
  })
  return await response.json().then((data) => data)
}
export const CreateTask = async (task: Task) => {
  let requestLine = hostName + "/" + controllerName + "/CreateTask"
  let response = await fetch(requestLine, {
    method: "POST",
    credentials: "include",
    headers: {
      "Content-type": "application/json"
    },
    body: JSON.stringify(task)
  })
  return await response.json().then((data) => data)
}

export const UpdateStudent = async (student: Student) => {
  let requestLine = hostName + "/" + controllerName + "/UpdateStudent"
  let response = await fetch(requestLine, {
    method: "PUT",
    credentials: "include",
    headers: {
      "Content-type": "application/json"
    },
    body: JSON.stringify(student)
  })
  return await response.json().then((data) => data)
}
export const UpdateTask = async (task: Task) => {
  let requestLine = hostName + "/" + controllerName + "/UpdateTask"

  let response = await fetch(requestLine, {
    method: "PUT",
    credentials: "include",
    headers: {
      "Content-type": "application/json"
    },
    body: JSON.stringify(task)
  })
  return await response.json().then((data) => data)
}
export const DeleteStudent = async (id: number) => {
  let requestLine = hostName + "/" + controllerName + "/DeleteStudent" + "/" + id
  let response = await fetch(requestLine, {
    method: "DELETE",
    credentials: "include",
  })
  return await response.json().then((data) => data)
}
export const DeleteTask = async (id: number) => {
  let requestLine = hostName + "/" + controllerName + "/DeleteTask" + "/" + id
  let response = await fetch(requestLine, {
    method: "DELETE",
    credentials: "include",
  })
  return await response.json().then((data) => data)
}