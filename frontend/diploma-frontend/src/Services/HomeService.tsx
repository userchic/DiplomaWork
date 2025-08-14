import { hostName } from "./HostName";
let controllerName="Home"

export const Login = async (Login: string, Password: string) => {
  let requestLine=hostName +"/" +  controllerName + "/Login"
  let response =await fetch(requestLine, {
    credentials: "include",
    method: "POST",
    headers: { 
      "Content-Type": "application/json;",
    },
    body: JSON.stringify({Login:Login,Password:Password}),
  })
  return response.json().then(data=>data);
}
export const Register = async (  Name:string,  Surname:string,  Fatname:string,  Login:string,  Password:string) =>{
    let requestLine=hostName +"/" +  controllerName +  "/Registry"
    const response = await fetch(requestLine, {
    credentials: "include",
    method: "POST",
    headers: { 
      "Content-Type": "application/json;",
    },
    body: JSON.stringify({Name:Name,Surname:Surname,Fatname:Fatname,Login:Login,Password:Password}),
    })
    return response.json().then(data=>data);
}
export const Logout= () =>  {
  let requestLine=hostName + "/" + controllerName + "/Logout"
  fetch(requestLine,{
    credentials: "include",
    method: "POST"
  })
}
