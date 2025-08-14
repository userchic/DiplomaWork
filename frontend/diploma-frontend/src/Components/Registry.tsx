import { type ChangeEvent, useState } from "react"
import { Register } from "../Services/HomeService"
import { useNavigate } from "react-router"
import useAuth from "../Hooks/useAuth";

export default function Registry()
{
    const {isAuthenticated,setAuth}=useAuth();

    const[login,setLogin] = useState('')
    const[password,setPassword] = useState('')
    const[name,setName] = useState('')
    const[surname,setSurname] = useState('')
    const[fatname,setFatname] = useState('')

    const [message,setMessage] = useState('')
    const navigate = useNavigate();
    function callRegistry()
    {
        let res=Register(name,surname,fatname,login,  password);

        res.then(result=>{
            if (result.success===1)
            {
                setAuth(true)
                navigate("/Games")
            }
            else
                setMessage(result.message)
        })

    }
    function handleLoginChange(event:ChangeEvent<HTMLInputElement>)
    {
        setLogin(event.target.value)
    }
    function handlePasswordChange(event:ChangeEvent<HTMLInputElement>)
    {
        setPassword(event.target.value)
    }
    function handleNameChange(event:ChangeEvent<HTMLInputElement>)
    {
        setName(event.target.value)
    }
    function handleSurnameChange(event:ChangeEvent<HTMLInputElement>)
    {
        setSurname(event.target.value)
    }
    function handleFatnameChange(event:ChangeEvent<HTMLInputElement>)
    {
        setFatname(event.target.value)
    }
    return (
        <>
            Имя <input type="text"        id="name" value={name} onChange={handleNameChange}/><br/>

            Фамилия <input type="text"    id="surname" value={surname} onChange={handleSurnameChange}/><br/>

            Отчество <input type="text"   id="fatname" value={fatname} onChange={handleFatnameChange}/><br/>

            Логин <input type="text"      id="login" value={login} onChange={handleLoginChange}/><br/>

            Пароль <input type="password" id="password" value={password} onChange={handlePasswordChange}/><br/>
            <div id="message">{message}</div>
            <input type="button" value="Зарегистрироваться" onClick={()=>callRegistry()}/>
        </>
    )
}


