import { Navigate, useNavigate } from "react-router"
import {type Student} from "../Models/Student"
import { DeleteStudent } from "../Services/AdminService";
interface Props{
    student:Student;
    handleDelete:(id:number)=>void;
}
export default function TaskCard({student,handleDelete}:Props)
{
    const navigate=useNavigate()
    
    function ShowStudent() {
        navigate("/Student/"+student.Id)
    }
    function Delete()
    {
        let res=DeleteStudent(student.Id);
        
        res.then(result=>{
            if (result.success===1)
                alert(result.message)
            handleDelete(student.Id)
        })
    }
    return (
        <>
            <td>
                {student.Name}
            </td>
            <td>
                {student.Surname}
            </td>
            <td>
                {student.Fatname}
            </td>
            <td>
                {student.Email}
            </td>
            <td>
                <input type="button" value="Подробнее" onClick={ShowStudent}/>
            </td>
            <td>
                <input type="button" value="Удалить" onClick={()=>Delete()}/>
            </td>
        </>
    )
}