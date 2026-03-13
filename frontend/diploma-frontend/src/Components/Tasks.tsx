import { Link } from "react-router"
import TaskCard from "./TaskCard"
import { useEffect, useState } from "react"
import { GetTasks } from "../Services/AdminService"
import { type Task } from "../Models/Task"
export default function Tasks() {
    const [Loading, setLoading] = useState(true)
    const [tasks, setTasks] = useState<Task[]>([])
    const [Page, setPage] = useState(1)
    const getTasks = async () => {
        const newTasks = await GetTasks(Page)
        setTasks(tasks.concat(newTasks))
        setLoading(false)
        setPage(Page + 1)
    }
    useEffect(() => {

        getTasks()
    }, [])
    function RemoveTaskCard(id: number) {
        let newList = tasks.filter(task => task.Id != id)
        setTasks(newList)
    }
    return (
        <>
            <h1>Список задач</h1>
            <Link to="/TaskCreate">Создать задачи</Link><br></br>

            {Loading ?
                <>Загрузка задач</>
                :
                <>
                    <table border="1px" class="table">
                        <thead>
                            <tr>
                                <th>
                                    Id
                                </th>
                                <th>
                                    Текст задачи
                                </th>
                                <th>

                                </th>
                                <th>

                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {tasks.map((task: Task) => (
                                <tr>
                                    <TaskCard task={task} handleDelete={RemoveTaskCard} key={task.Id} />
                                </tr>
                            ))}

                        </tbody>
                    </table>
                    <input type="button" value="Ещё" onClick={getTasks} />
                </>
            }
        </>
    )
}