import { useState, type ChangeEvent } from "react"
import { CreateTask } from "../Services/AdminService"
import {type Task} from "../Models/Task"
import { useNavigate } from "react-router";

export default function TaskCreator()
{
    const [text,setText] = useState<string>("")
    const navigate = useNavigate();
    function Create()
    {
        let newTask:Task= {text:text,Id:undefined}

        let res=CreateTask(newTask)
        res.then(result=>{
            if (result.success===1)
                alert(result.message)
        })
    }
    function handleTextChange(event:ChangeEvent<HTMLInputElement>)
    {
        setText(event.target.value)
    }
    let style={height: '100px', width: '400px'}
    function NavToTaskGeneration(): void {
        navigate("/TaskGenerator")
    }

    return(
        <>
            <h1>Создание задачи</h1>
        <div>
            Текст:<input type="text" style={ style} value={text} onChange={handleTextChange}/><br/>
            <input style={{flex:1,alignSelf:"flex-start"}} type="button" value="Создать задачу" onClick={Create}/>
            <input  style={{flex:1,marginLeft:"140px"}} type="button" value="Перейти к генерации задач" onClick={NavToTaskGeneration}/>
        </div>
        </>
    )
}