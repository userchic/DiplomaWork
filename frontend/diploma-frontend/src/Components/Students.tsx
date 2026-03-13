import { Link } from "react-router"
import StudentCard from "./StudentCard"
import { useEffect, useState } from "react"
import { GetStudents } from "../Services/AdminService"
import { type Student } from "../Models/Student"
export default function Students() {
    const [Loading, setLoading] = useState(true)
    const [students, setStudents] = useState<Student[]>([])
    const [Page, setPage] = useState(1)
    const getStudents = async () => {
        const newStudents = await GetStudents(Page)
        setStudents(students.concat(newStudents))
        setLoading(false)
        setPage(Page + 1)
    }

    useEffect(() => {
        getStudents()
    }, [])
    function RemoveStudentCard(id: number) {
        let newList = students.filter(student => student.Id != id)
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
                    <table border="1px" class="table">
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
                                    Обр. учреждение
                                </th>
                                <th>

                                </th>
                                <th>

                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {students.map((student: Student) => (
                                <tr>
                                    <StudentCard student={student} handleDelete={RemoveStudentCard} key={student.Id} />
                                </tr>
                            ))}

                        </tbody>
                    </table>
                    <input type="button" value="Ещё" onClick={getStudents} />
                </>
            }
        </>
    )
}