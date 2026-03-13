import { type ChangeEvent, useState } from "react"
import { Register } from "../Services/HomeService"
import { useNavigate } from "react-router"
import useAuth from "../Hooks/useAuth";

export default function Registry() {
    const { isAuthenticated, setAuth } = useAuth();

    const [login, setLogin] = useState('')
    const [password, setPassword] = useState('')
    const [name, setName] = useState('')
    const [surname, setSurname] = useState('')
    const [fatname, setFatname] = useState('')

    const [message, setMessage] = useState('')
    const navigate = useNavigate();
    function callRegistry() {
        let res = Register(name, surname, fatname, login, password);

        res.then(result => {
            if (result.success === 1) {
                setAuth(true)
                localStorage.setItem("authFlag", "true");
                navigate("/Games")
            }
            else
                if (result.message !== undefined)
                    setMessage(result.message)
                else
                    setMessage(Object.values(result.errors)[0])
        })

    }
    function handleLoginChange(event: ChangeEvent<HTMLInputElement>) {
        setLogin(event.target.value)
    }
    function handlePasswordChange(event: ChangeEvent<HTMLInputElement>) {
        setPassword(event.target.value)
    }
    function handleNameChange(event: ChangeEvent<HTMLInputElement>) {
        setName(event.target.value)
    }
    function handleSurnameChange(event: ChangeEvent<HTMLInputElement>) {
        setSurname(event.target.value)
    }
    function handleFatnameChange(event: ChangeEvent<HTMLInputElement>) {
        setFatname(event.target.value)
    }
    return (
        <>
            <div className="centering-parent">
                <div className="block">
                    <h2>Регистрация</h2>
                    <div style={{ display: "inline-block" }}>
                        Имя*
                        <br />
                        Фамилия*
                        <br />
                        Отчество
                        <br />
                        Логин*
                        <br />
                        Пароль*
                    </div>
                    <div style={{ display: "inline-block" }}>
                        <div className="input">
                            <input type="text" id="name" value={name} onChange={handleNameChange} /><br />
                        </div>
                        <div className="input">
                            <input type="text" id="surname" value={surname} onChange={handleSurnameChange} /><br />
                        </div>
                        <div className="input">
                            <input type="text" id="fatname" value={fatname} onChange={handleFatnameChange} /><br />
                        </div>
                        <div className="input">
                            <input type="text" id="login" value={login} onChange={handleLoginChange} /><br />
                        </div>
                        <div className="input">
                            <input type="password" id="password" value={password} onChange={handlePasswordChange} /><br />
                        </div>
                    </div>
                    <div id="message">{message}</div>
                    <input type="button" value="Зарегистрироваться" onClick={() => callRegistry()} />
                </div>
            </div>
        </>
    )
}


