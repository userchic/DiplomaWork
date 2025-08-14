import { useEffect, useState, type ChangeEvent } from "react"
import { GetStudent, UpdateStudent } from "../Services/AdminService"
import type { Student } from "../Models/Student"
import { useParams } from "react-router"

export default function Student()
{
    let params=useParams()
    const [name,setName] = useState<string>("")
    const [surname,setSurname] = useState<string>("")
    const [fatname,setFatname] = useState<string>("")
    const [email,setEmail] = useState<string>("")
    
    useEffect (() => {
        if(typeof params.id == "string")
        {
            let id:number=parseInt(params['id'])
            const getStudent = async () => {
                const student=await GetStudent(id)
                setName(student.name)
                setSurname(student.surname)
                setFatname(student.fatname)
                setEmail(student.email)
            }
            getStudent()
        }
    },[])
    function Edit()
    {
        let updatedStudent:Student= {
            Name:name,
            Surname:surname,
            Fatname:fatname,
            Email:email,
            Id:params.id
        }

        let res=UpdateStudent(updatedStudent)
        res.then(result=>{
            if (result.success===1)
                alert(result.message)
        })
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
    function handleEmailChange(event:ChangeEvent<HTMLInputElement>)
    {
        setEmail(event.target.value)
    }
    return(
        <>
            <h1>Редактирование студента</h1>
            Имя:<input type="text" value={name} onChange={handleNameChange}/><br />
            Фамилия:<input type="text" value={surname} onChange={handleSurnameChange}/><br />
            Отчество:<input type="text" value={fatname} onChange={handleFatnameChange}/><br />
            Email:<input type="text" value={email} onChange={handleEmailChange}/><br />
            <input type="button" value="Отредактировать задачу" onClick={Edit}/>
        </>
    )
}