import { useNavigate } from "react-router";
import useAuth from "../Hooks/useAuth";
import { Login } from "../Services/HomeService"
import { type ChangeEvent, useState } from "react"
export default function LoginSection() {
    const { isAuthenticated, setAuth } = useAuth();
    const [login, setLogin] = useState('')
    const [password, setPassword] = useState('')
    const [message, setMessage] = useState('')

    const navigate = useNavigate();

    function callLogin() {
        let res = Login(login, password);

        res.then(result => {
            alert(result.message)
            if (result.success === 1) {
                setAuth(true)
                navigate("/Games")
            }
        })

    }
    function handleLoginChange(event: ChangeEvent<HTMLInputElement>) {
        setLogin(event.target.value)
    }
    function handlePasswordChange(event: ChangeEvent<HTMLInputElement>) {
        setPassword(event.target.value)
    }
    return (
        <>
            <h2>Вход в систему</h2>
            <div style={{ display: "inline-block" }}>
                Логин<br />
                Пароль
            </div>
            <div style={{ display: "inline-block" }}>
                <input type="text" id="login" value={login} onChange={handleLoginChange} /><br />
                <input type="password" id="password" value={password} onChange={handlePasswordChange} /><br />
            </div>
            <div id="message">{message}</div>
            <input type="button" value="Войти" onClick={() => callLogin()} />
        </>
    )
}