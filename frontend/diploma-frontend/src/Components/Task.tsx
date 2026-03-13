import { useEffect, useState, type ChangeEvent } from "react"
import { GetTaskInfo, UpdateTask } from "../Services/AdminService"
import { type Task } from "../Models/Task"
import TaskCard from "./TaskCard"
import { useParams } from "react-router"
import TextArea from "antd/es/input/TextArea"
export default function Task() {
    const [Loading, setLoading] = useState(true)
    const [text, setText] = useState<string>()
    let params = useParams()
    useEffect(() => {
        if (typeof params.id == "string") {
            let id: number = parseInt(params['id'])
            const getTasks = async () => {
                const task = await GetTaskInfo(id)
                setText(task.text)
                setLoading(false)
            }
            getTasks()
        }
    }, [])
    function EditTask() {
        if (typeof text == 'string') {
            let res = UpdateTask({
                Id: Number(params.id),
                text: text
            });
            res.then(result => {
                if (result.message !== undefined)
                    alert(result.message)
                else
                    alert(Object.values(result.errors)[0])
            })
        }
    }
    function handleTextChange(event: ChangeEvent<HTMLInputElement>) {
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
                    <div style={{ display: "flex" }}>
                        Текст: <TextArea rows={5} cols={100} value={text} onChange={handleTextChange} />
                    </div>
                    <input type="button" value="Отредактировать задачу" onClick={EditTask} />
                </>
            }
        </>
    )
}