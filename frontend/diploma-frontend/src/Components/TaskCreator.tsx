import { useState, type ChangeEvent } from "react"
import { CreateTask } from "../Services/AdminService"
import { type Task } from "../Models/Task"
import { useNavigate } from "react-router";
import { subjectTopics, type ISubject } from "../Models/SubjectTopics"
import { GenerateTask } from "../Services/AdminService"

export default function TaskCreator() {
    const [text, setText] = useState<string>("")
    const navigate = useNavigate();
    const [TaskAmount, setTaskAmount] = useState(0)
    const [Subject, setSubject] = useState("")
    const [Topic, setTopic] = useState("")
    const [TaskSize, setTaskSize] = useState("")
    const [QuestionsAmount, setQuestionsAmount] = useState(0)
    const [Answer, setAnswer] = useState("")
    const [currentSubject, setCurrentSubject] = useState<ISubject>(subjectTopics[0])

    function handleTaskAmountChange(event: ChangeEvent<HTMLInputElement>): void {
        setTaskAmount(parseInt(event.target.value))
    }

    function handleSubjectChange(event: ChangeEvent<HTMLSelectElement>): void {
        let subject = subjectTopics.filter((subject) => subject.Subject == event.target.value).pop()
        if (subject != null) {
            setCurrentSubject(subject)
            setSubject(subject.Subject)
        }
        else {
            setCurrentSubject({
                Subject: "",
                Topics: []
            })
            setSubject("")
        }
    }

    function handleTopicChange(event: ChangeEvent<HTMLSelectElement>): void {
        setTopic(event.target.value)
    }

    function handleAnswerChange(event: ChangeEvent<HTMLSelectElement>): void {
        setAnswer(event.target.value)
    }
    function handleTaskSizeChange(event: ChangeEvent<HTMLSelectElement>): void {
        setTaskSize(event.target.value)
    }
    function handleQuestionsAmountChange(event: ChangeEvent<HTMLInputElement>): void {
        setQuestionsAmount(parseInt(event.target.value))
    }

    function handleTaskGenerate(): void {
        let res = GenerateTask({
            AnswerSize: Answer,
            QuestionsAmount: QuestionsAmount,
            Subject: Subject,
            TaskAmount: TaskAmount,
            TaskSize: TaskSize,
            Topic: Topic,
        })
        res.then(result => setText(result.message))
    }
    function Create() {
        let newTask: Task = { text: text, Id: undefined }

        let res = CreateTask(newTask)
        res.then(result => {
            if (result.success !== undefined)
                alert(result.message)
            else
                alert(Object.values(result.errors)[0]);
        })
    }
    function handleTextChange(event: ChangeEvent<HTMLInputElement>) {
        setText(event.target.value)
    }
    let style = { height: '100px', width: '400px' }
    function NavToTaskGeneration(): void {
        navigate("/TaskGenerator")
    }

    return (
        <>
            <h1>Создание задач</h1>
            <div>
                Текст:<input type="text" style={style} value={text} onChange={handleTextChange} /><br />
                <input style={{ flex: 1, alignSelf: "flex-start" }} type="button" value="Создать задачу" onClick={Create} />
                <input style={{ flex: 1, marginLeft: "140px" }} type="button" value="Перейти к генерации задач" onClick={NavToTaskGeneration} />
            </div>

            Кол-во задач: <input type="number" value={TaskAmount} onChange={handleTaskAmountChange} /><br />
            Предмет:<select value={Subject} onChange={handleSubjectChange}>
                <option value="" />
                {
                    subjectTopics.map(subject => {
                        return (
                            <option key={subject.Subject} value={subject.Subject}>{subject.Subject}</option>
                        )
                    })

                }
            </select><br />
            Тема:<select value={Topic} onChange={handleTopicChange}>
                <option value="" />
                {
                    currentSubject.Topics.map(topicName => {
                        return (
                            <option value={topicName}>{topicName}</option>
                        )
                    })
                }

            </select><br />
            Размер задачи:<select value={TaskSize} onChange={handleTaskSizeChange}>
                <option value="" />
                <option value="Большой">Большой</option>
                <option value="Средний">Средний</option>
                <option value="Маленький">Маленький</option>
            </select><br />
            Количество вопросов:<input type="number" value={QuestionsAmount} onChange={handleQuestionsAmountChange} /><br />
            Ответ:<select value={Answer} onChange={handleAnswerChange}>
                <option value="" />
                <option value="С красивым значением">С красивым значением</option>
                <option value="С некрасивым значением">С некрасивым значением</option>
            </select>
            <input type="button" value="Сгенерировать текст задачи" onClick={handleTaskGenerate} />
        </>
    )
}