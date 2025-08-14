import { useEffect, useState, type ChangeEvent } from "react"
import { GetTask, UpdateTask } from "../Services/AdminService"
import {type Task} from "../Models/Task"
import TaskCard from "./TaskCard"
import { useParams } from "react-router"
export default function Task()
{
    const [Loading,setLoading] = useState(true)
    const [text,setText] = useState<string>()
    let params=useParams()
    useEffect (() => {
        if(typeof params.id == "string")
        {
            let id:number=parseInt(params['id'])
            const getGames = async () => {
                const task=await GetTask(id)
                setText(task.text)
                setLoading(false)
            }
            getGames()
        }
    },[])
    function EditTask ()
    {
        if (typeof text =='string')
        {
            let res=UpdateTask({
                Id:Number(params.id),
                text:text
            });
            res.then(result=>{
                alert(result.message)
            })
        }
    }
    function handleTextChange(event:ChangeEvent<HTMLInputElement>)
    {
        setText(event.target.value)
    }
        
    return (
    <>
        <h1>Просмотр задачи</h1>
        
        {Loading ?
             <>
             Загрузка задачи
             </>
             :
            <>
                Текст: <input type="text" value={text} onChange={handleTextChange}/>
                <input type="button" value="Отредактировать задачу" onClick={EditTask}/>
            </>
        }
        </>
    )
}