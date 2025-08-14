import { Link } from "react-router"
import TaskCard from "./TaskCard"
import { useEffect, useState } from "react"
import { GetTasks } from "../Services/AdminService"
import {type Task} from "../Models/Task"
export default function Tasks()
{
    const [Loading,setLoading] = useState(true)
    const [tasks,setTasks] = useState<Task[]>([])
    useEffect (() => {
        const getTasks = async () => {
        const tasks=await GetTasks()
        setTasks(tasks)
        setLoading(false)
        }
        getTasks()
    },[])
    function RemoveTaskCard(id:number)
    {
        let newList=tasks.filter(task=>task.Id!=id)
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
             <table>
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
                    {tasks.map((task:Task)=>(
                        <tr>
                            <TaskCard task={task} handleDelete={RemoveTaskCard} key={task.Id} />
                        </tr>
                    ))}
                    
                </tbody>
            </table>
            </>
        }
        </>
    )
}