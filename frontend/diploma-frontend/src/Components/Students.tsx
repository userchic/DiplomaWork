import { Link } from "react-router"
import StudentCard from "./StudentCard"
import { useEffect, useState } from "react"
import { GetStudents } from "../Services/AdminService"
import {type Student} from "../Models/Student"
export default function Students()
{
    const [Loading,setLoading] = useState(true)
    const [students,setStudents] = useState<Student[]>([])
    useEffect (() => {
        const getStudents = async () => {
        const students=await GetStudents()
        setStudents(students)
        setLoading(false)
        }
        getStudents()
    },[])
    function RemoveStudentCard(id:number)
    {
        let newList=students.filter(student=>student.Id!=id)
        setStudents(newList)
    }
    return (
    <>
        <h1>Список студентов</h1>
        <Link to="/StudentCreate">Создать студентов</Link><br></br>
        
        {Loading ?
             <>Загрузка студентов</>
             :
             <>
             <table>
                <thead>
                    <tr>
                        <th>
                            Имя
                        </th>
                        <th>
                            Фамилия
                        </th>
                        <th>
                            Отчество
                        </th>
                        <th>
                            Email
                        </th>
                        <th>
                            
                        </th>
                        <th>
                            
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {students.map((student:Student)=>(
                        <tr>
                            <StudentCard student={student} handleDelete={RemoveStudentCard} key={student.Id} />
                        </tr>
                    ))}
                    
                </tbody>
            </table>
            </>
        }
        </>
    )
}