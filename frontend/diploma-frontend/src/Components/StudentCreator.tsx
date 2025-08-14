import { useState, type ChangeEvent } from "react"
import { CreateStudent } from "../Services/AdminService"
import { type Task } from "../Models/Task"
import type { Student } from "../Models/Student"

export default function StudentCreator() {
    const [name, setName] = useState<string>("")
    const [surname, setSurname] = useState<string>("")
    const [fatname, setFatname] = useState<string>("")
    const [email, setEmail] = useState<string>("")

    function Create() {
        let newStudent: Student = {
            Name: name,
            Surname: surname,
            Fatname: fatname,
            Email: email,
            Id: undefined
        }

        let res = CreateStudent(newStudent)
        res.then(result => {
            alert(result.message)
        })
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
    function handleEmailChange(event: ChangeEvent<HTMLInputElement>) {
        setEmail(event.target.value)
    }
    return (
        <>
            <h1>Создание задачи</h1>
            Имя:<input type="text" value={name} onChange={handleNameChange} /><br />
            Фамилия:<input type="text" value={surname} onChange={handleSurnameChange} /><br />
            Отчество:<input type="text" value={fatname} onChange={handleFatnameChange} /><br />
            Email:<input type="text" value={email} onChange={handleEmailChange} /><br />
            <input type="button" value="Создать задачу" onClick={Create} />
        </>
    )
}