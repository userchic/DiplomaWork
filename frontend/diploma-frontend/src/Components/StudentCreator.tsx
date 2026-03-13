import { useState, type ChangeEvent } from "react"
import { CreateStudent } from "../Services/AdminService"

import type { Student } from "../Models/Student"

export default function StudentCreator() {
    const [name, setName] = useState<string>("")
    const [surname, setSurname] = useState<string>("")
    const [fatname, setFatname] = useState<string>("")
    const [email, setEmail] = useState<string>("")
    const [educationFacility, setEducationFacility] = useState("")

    function Create() {
        let newStudent: Student = {
            Name: name,
            Surname: surname,
            Fatname: fatname,
            Email: email,
            EducationFacility: educationFacility,
            Id: undefined
        }

        let res = CreateStudent(newStudent)
        res.then(result => {
            if (result.message !== undefined)
                alert(result.message)
            else
                alert(Object.values(result.errors)[0])
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
    function handleEducationFacilityChange(event: ChangeEvent<HTMLInputElement>) {
        setEducationFacility(event.target.value)
    }
    return (
        <>
            <h1>Создание студентов</h1>
            <div style={{ display: "inline-block" }}>
                <div className="inputLabel">
                    Имя:<br />
                </div>
                <div className="inputLabel">
                    Фамилия:<br />
                </div>
                <div className="inputLabel">
                    Отчество:<br />
                </div>
                <div className="inputLabel">
                    Email:
                </div>
                <div className="inputLabel">
                    Образовательное учреждение:
                </div>
            </div >
            <div style={{ display: "inline-block" }}>
                <div className="input">
                    <input type="text" value={name} onChange={handleNameChange} /><br />
                </div>
                <div className="input">
                    <input type="text" value={surname} onChange={handleSurnameChange} /><br />
                </div>
                <div className="input">
                    <input type="text" value={fatname} onChange={handleFatnameChange} /><br />
                </div>
                <div className="input">
                    <input type="text" value={email} onChange={handleEmailChange} />
                </div>
                <div className="input">
                    <input type="text" value={educationFacility} onChange={handleEducationFacilityChange} />
                </div>
            </div ><br />
            <input type="button" value="Создать задачу" onClick={Create} />
        </>
    )
}