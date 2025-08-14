import { useEffect, useState, type ChangeEvent } from "react"
import { subjectTopics, type ISubject } from "../Models/SubjectTopics"

export default function TaskGenerator()
{
    const [GeneratedText,setGeneratedText] = useState("")
    
    const [TaskAmount,setTaskAmount] = useState(0)
    const [Subject,setSubject] = useState("")
    const [Topic,setTopic] = useState("")
    const [TaskSize,setTaskSize] = useState("")
    const [QuestionsAmount,setQuestionsAmount] = useState(0)
    const [Answer,setAnswer] = useState("")
    const [currentSubject,setCurrentSubject] = useState<ISubject>(subjectTopics[0])

    function handleTaskAmountChange(event: ChangeEvent<HTMLInputElement>): void {
        setTaskAmount(parseInt(event.target.value))
    }

    function handleSubjectChange(event: ChangeEvent<HTMLSelectElement>): void {
        let subject= subjectTopics.filter((subject)=>subject.Subject==event.target.value).pop()
        if (subject != null)
        {
        setCurrentSubject(subject)
        setSubject(subject.Subject)
        }
        else
        {
            setCurrentSubject({Subject:"",
                Topics:[]
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

    return(
        <>
            Сгенерированный текст: {GeneratedText}<br/>
            Кол-во задач: <input type="number" value={TaskAmount} onChange={handleTaskAmountChange}/><br />
            Предмет:<select value={Subject} onChange={handleSubjectChange}>
                <option value=""/>
                {
                subjectTopics.map(subject=> {
                    return (
                        <option key={subject.Subject} value={subject.Subject}>{subject.Subject}</option>
                    )
                })
                
                }
            </select><br/>
            Тема:<select value={Topic} onChange={handleTopicChange}>
                <option value=""/>
                {
                    currentSubject.Topics.map(topicName=>{
                        return(
                            <option value={topicName}>{topicName}</option>
                        )
                    })
                }
                
            </select><br/>
            Размер задачи:<select value={TaskSize} onChange={handleTaskSizeChange}>
                    <option value=""/>
                    <option value="Большой">Большой</option>
                    <option value="Средний">Средний</option>
                    <option value="Маленький">Маленький</option>
                </select><br/>
            Количество вопросов:<input type="number" value={QuestionsAmount} onChange={handleQuestionsAmountChange}/><br />
            Ответ:<select value={Answer} onChange={handleAnswerChange}>
                    <option value=""/>
                    <option value="С красивым значением">С красивым значением</option>
                    <option value="С некрасивым значением">С некрасивым значением</option>
                </select>
        </>
    )
}