import { useEffect, useState, type ChangeEvent } from "react"
import { GetStudentInfo, UpdateStudent } from "../Services/AdminService"
import type { Student } from "../Models/Student"
import { useParams } from "react-router"

export default function Student() {
    let params = useParams()
    const [name, setName] = useState<string>("")
    const [surname, setSurname] = useState<string>("")
    const [fatname, setFatname] = useState<string>("")
    const [email, setEmail] = useState<string>("")
    const [educationFacility, setEducationFacility] = useState("")

    useEffect(() => {
        if (typeof params.id == "string") {
            let id: number = parseInt(params['id'])
            const getStudent = async () => {
                const student = await GetStudentInfo(id)
                setName(student.name)
                setSurname(student.surname)
                setFatname(student.fatname)
                setEmail(student.email)
                setEducationFacility(student.EducationFacility)
            }
            getStudent()
        }
    }, [])
    function Edit() {
        let updatedStudent: Student = {
            Name: name,
            Surname: surname,
            Fatname: fatname,
            Email: email,
            EducationFacility: educationFacility,
            Id: params.id
        }

        let res = UpdateStudent(updatedStudent)
        res.then(result => {
            if (result.success !== undefined)
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
            <h1>Редактирование студента</h1>
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
                    <input type="text" value={email} onChange={handleEmailChange} /><br />
                </div>
                <div className="input">
                    <input type="text" value={educationFacility} onChange={handleEducationFacilityChange} /><br />
                </div>
            </div ><br />
            <input type="button" value="Отредактировать задачу" onClick={Edit} />
        </>
    )
}