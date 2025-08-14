import { useNavigate } from "react-router"
import { type Task } from "../Models/Task"
import { DeleteTask } from "../Services/AdminService";
interface Props {
    task: Task;
    handleDelete: (id: number) => void;
}
export default function TaskCard({ task, handleDelete }: Props) {
    const navigate = useNavigate()

    function ShowTask() {
        navigate("/Task/" + task.Id)
    }
    function Delete() {
        let res = DeleteTask(task.Id);

        res.then(result => {
            if (result.success === 1)
                alert(result.message)
            handleDelete(task.Id)
        })
    }
    return (
        <>
            <td>
                {task.Id}
            </td>
            <td>
                {task.Text}
            </td>
            <td>
                <input type="button" value="Подробнее" onClick={ShowTask} />
            </td>
            <td>
                <input type="button" value="Удалить" onClick={() => Delete()} />
            </td>
        </>
    )
}