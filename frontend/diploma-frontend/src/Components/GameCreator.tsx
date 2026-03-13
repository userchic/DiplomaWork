import { useEffect, useState, type ChangeEvent } from "react"
import type { Task } from "../Models/Task"
import type { Student } from "../Models/Student"
import { CreateGame, GetStudents, GetTasks } from "../Services/AdminService"

interface StudentChoice {
    Student: Student,
    role: StudentRole
}
interface StudentRole {
    isTeam1: boolean,
    isTeam2: boolean,
    isTeam1Lead: boolean,
    isSemiTeam1Lead: boolean,
    isTeam2Lead: boolean,
    isSemiTeam2Lead: boolean
}

export default function GameCreator() {
    const [Name, setName] = useState("")
    const [SolvingTime, setSolvingTime] = useState(0)
    const [CaptainsRoundFormat, setCaptainsRoundFormat] = useState("Обычный раунд")
    const [Team1Name, setTeam1Name] = useState("")
    const [Team2Name, setTeam2Name] = useState("")

    const [Tasks, setTasks] = useState<{ Task: Task, chosen: boolean }[]>([])
    const [Students, setStudents] = useState<StudentChoice[]>([])
    const [StudPage, setStudPage] = useState(1)
    const [TaskPage, setTaskPage] = useState(1)
    const [EventPlace, setEventPlace] = useState("")
    const [PlannedStartTime, setPlannedStartTime] = useState("")
    const getTasks = async () => {
        const tasks = await GetTasks(TaskPage)
        setTaskPage(TaskPage + 1);
        const taskWithState = tasks.map((task) => {
            return { Task: task, chosen: false }

        })
        setTasks(Tasks.concat(taskWithState))
    }
    const getStudents = async () => {
        const students = await GetStudents(StudPage)
        setStudPage(StudPage + 1);
        let studentWithState: StudentChoice[] = students.map((student) => {
            return {
                Student: student, role: {
                    isTeam1: false, isTeam2: false, isTeam1Lead: false,
                    isSemiTeam1Lead: false, isTeam2Lead: false, isSemiTeam2Lead: false
                }
            }

        })
        setStudents(Students.concat(studentWithState))
    }
    useEffect(() => {
        getTasks()
        getStudents()
    }, [])
    function GetStudentRoleAndStudents(student: Student): { StudentChoices: StudentChoice[], role: StudentRole } {
        let currentStudents = Students
        let editedStudent = currentStudents?.filter((stud) => {
            if (stud.Student.Id == student.Id)
                return stud
        }).pop()
        let role = editedStudent.role
        return {
            StudentChoices: currentStudents,
            role
        }
    }

    async function handleisTeam1Change(student: Student) {
        let res = GetStudentRoleAndStudents(student)
        let currentStudents = res.StudentChoices
        let role = res.role
        if (role.isTeam1) {
            role.isTeam1 = false
            role.isTeam1Lead = false
            role.isSemiTeam1Lead = false
            role.isTeam2 = false
            role.isTeam2Lead = false
            role.isSemiTeam2Lead = false
        }
        else {
            role.isTeam1 = true;
            role.isTeam2 = false
            role.isTeam2Lead = false
            role.isSemiTeam2Lead = false
        }
        await setStudents([...currentStudents])
    }
    function handleisTeam1LeadChange(student: Student) {
        let res = GetStudentRoleAndStudents(student)
        let currentStudents = res.StudentChoices
        let role = res.role
        if (role.isTeam1Lead) {
            role.isTeam1 = true;
            role.isTeam1Lead = false
            role.isSemiTeam1Lead = false
            role.isTeam2 = false
            role.isTeam2Lead = false
            role.isSemiTeam2Lead = false
        }
        else {
            currentStudents = currentStudents.map((student) => {
                student.role.isTeam1Lead = false
                return student
            })
            role.isTeam1 = true;
            role.isTeam1Lead = true
            role.isSemiTeam1Lead = false
            role.isTeam2 = false
            role.isTeam2Lead = false
            role.isSemiTeam2Lead = false
        }

        setStudents([...currentStudents])
    }
    function handleisSemiTeam1LeadChange(student: Student) {
        let res = GetStudentRoleAndStudents(student)
        let currentStudents = res.StudentChoices
        let role = res.role
        if (role.isSemiTeam1Lead) {
            role.isTeam1 = true;
            role.isTeam1Lead = false
            role.isSemiTeam1Lead = false
            role.isTeam2 = false
            role.isTeam2Lead = false
            role.isSemiTeam2Lead = false
        }
        else {
            currentStudents = currentStudents.map((student) => {
                student.role.isSemiTeam1Lead = false
                return student
            })
            role.isTeam1 = true;
            role.isTeam1Lead = false
            role.isSemiTeam1Lead = true
            role.isTeam2 = false
            role.isTeam2Lead = false
            role.isSemiTeam2Lead = false
        }
        setStudents([...currentStudents])
    }
    async function handleisTeam2Change(student: Student) {
        let res = GetStudentRoleAndStudents(student)
        let currentStudents = res.StudentChoices
        let role = res.role
        if (role.isTeam2) {
            role.isTeam1 = false
            role.isTeam1Lead = false
            role.isSemiTeam1Lead = false
            role.isTeam2 = false
            role.isTeam2Lead = false
            role.isSemiTeam2Lead = false
        }
        else {
            role.isTeam2 = true;
            role.isTeam1 = false
            role.isTeam1Lead = false
            role.isSemiTeam1Lead = false
        }
        await setStudents([...currentStudents])
    }
    function handleisTeam2LeadChange(student: Student) {
        let res = GetStudentRoleAndStudents(student)
        let currentStudents = res.StudentChoices
        let role = res.role
        if (role.isTeam2Lead) {
            role.isTeam2 = true;
            role.isTeam2Lead = false
            role.isSemiTeam2Lead = false
            role.isTeam1 = false
            role.isTeam1Lead = false
            role.isSemiTeam1Lead = false
        }
        else {
            currentStudents = currentStudents.map((student) => {
                student.role.isTeam2Lead = false
                return student
            })
            role.isTeam2 = true;
            role.isTeam2Lead = true
            role.isSemiTeam2Lead = false
            role.isTeam1 = false
            role.isTeam1Lead = false
            role.isSemiTeam1Lead = false
        }
        setStudents([...currentStudents])
    }
    function handleisSemiTeam2LeadChange(student: Student) {
        let res = GetStudentRoleAndStudents(student)
        let currentStudents = res.StudentChoices
        let role = res.role
        if (role.isSemiTeam2Lead) {
            role.isTeam2 = true;
            role.isTeam2Lead = false
            role.isSemiTeam2Lead = false
            role.isTeam1 = false
            role.isTeam1Lead = false
            role.isSemiTeam1Lead = false
        }
        else {
            currentStudents = currentStudents.map((student) => {
                student.role.isSemiTeam2Lead = false
                return student
            })
            role.isTeam2 = true;
            role.isTeam2Lead = false
            role.isSemiTeam2Lead = true
            role.isTeam1 = false
            role.isTeam1Lead = false
            role.isSemiTeam1Lead = false
        }
        setStudents([...currentStudents])
    }
    function handleNameChange(event: ChangeEvent<HTMLInputElement>): void {
        setName(event.target.value)
    }

    function handleCaptainsRoundFormatChange(event: ChangeEvent<HTMLInputElement>): void {
        setCaptainsRoundFormat(event.target.value)
    }
    function handleSolvingTimeChange(event: ChangeEvent<HTMLInputElement>): void {
        setSolvingTime(parseInt(event.target.value))
    }
    function handleTeam1NameChange(event: ChangeEvent<HTMLInputElement>): void {
        setTeam1Name(event.target.value)
    }
    function handleTeam2NameChange(event: ChangeEvent<HTMLInputElement>): void {
        setTeam2Name(event.target.value)
    }
    function handleEventPlaceChange(event: ChangeEvent<HTMLInputElement>) {
        setEventPlace(event.target.value)
    }
    function handlePlannedStartTimeChange(event: ChangeEvent<HTMLInputElement>) {
        setPlannedStartTime(event.target.value)
    }
    function Create(): void {
        let chosenTasksIds = Tasks?.filter((task) => {
            if (task.chosen)
                return true
            return false;
        }).map((task) => {

            return task.Task.Id
        })
        let studentsTeam1Ids = Students?.filter((student) => {
            if (student.role.isTeam1)
                return true
            return false
        }).map((student) => {
            return student.Student.Id
        })
        let studentsTeam2Ids = Students?.filter((student) => {
            if (student.role.isTeam2)
                return true
            return false
        }).map((student) => {
            return student.Student.Id
        })
        let team1CaptainId = Students.filter((student) => {
            if (student.role.isTeam1Lead)
                return true
            return false
        }).pop()?.Student.Id
        let team1SemiCaptainId = Students.filter((student) => {
            if (student.role.isSemiTeam1Lead)
                return true
            return false
        }).pop()?.Student.Id
        let team2CaptainId = Students.filter((student) => {
            if (student.role.isTeam2Lead)
                return true
            return false
        }).pop()?.Student.Id
        let team2SemiCaptainId = Students.filter((student) => {
            if (student.role.isSemiTeam2Lead)
                return true
            return false
        }).pop()?.Student.Id
        let res = CreateGame({
            SolvingTime: SolvingTime,
            PlannedStartTime: new Date(PlannedStartTime),
            Name: Name,
            captainsRoundFormat: CaptainsRoundFormat,
            chosenTasksIds: chosenTasksIds,
            studentsTeam1: studentsTeam1Ids,
            team1Name: Team1Name,
            team1CaptainId: team1CaptainId,
            team1ViceCaptainId: team1SemiCaptainId,
            studentsTeam2: studentsTeam2Ids,
            team2Name: Team2Name,
            team2CaptainId: team2CaptainId,
            team2ViceCaptainId: team2SemiCaptainId,
            EventPlace: EventPlace
        })
        res.then(result => {
            if (result.success !== undefined)
                alert(result.message)
            else {
                if (Object.keys(result.errors)[0] == "$id")
                    alert(Object.values(result.errors)[1])
                else
                    alert(Object.values(result.errors)[0])
            }
        })
    }

    return (
        <>
            <h2>Запланирование игры</h2>
            <div className="block">
                <div style={{ display: "inline-block", width: "20%" }}>
                    <div className="inputLabel">
                        Название игры
                    </div>
                    <div className="inputLabel">
                        Формат капитанского раунда<br />
                    </div>
                    <div className="inputLabel">
                        Мероприятие/Место проведения<br />
                    </div>
                    <div className="inputLabel">
                        Время решения задач(мин)
                    </div>
                    <div className="inputLabel">
                        Запланированное время начала боя
                    </div>
                </div>
                <div style={{ display: "inline-block", width: "80%" }}>
                    <div className="input">
                        <input type="text" style={{ width: "100%" }} value={Name} onChange={handleNameChange} />
                    </div>
                    <div className="input">
                        <input type="text" style={{ width: "100%" }} value={CaptainsRoundFormat} onChange={handleCaptainsRoundFormatChange} />
                    </div>
                    <div className="input">
                        <input type="text" style={{ width: "100%" }} value={EventPlace} onChange={handleEventPlaceChange} />
                    </div>
                    <div className="input">
                        <input type="number" value={SolvingTime} onChange={handleSolvingTimeChange} />
                    </div>
                    <div className="input">
                        <input type="datetime-local" value={PlannedStartTime} onChange={handlePlannedStartTimeChange} />
                    </div>
                </div>
            </div>
            <br />
            <div style={{ display: "inline-block" }}>

                Выберите задачи для игры <input type="button" value="Создать игру" onClick={Create} /><br />
            </div>
            <br />
            <div className="block" style={{ display: "flex" }}>
                <div style={{ flex: 1 }}>
                    Название 1 команды<input type="text" value={Team1Name} onChange={handleTeam1NameChange} />
                </div>
                <div style={{ flex: 1 }}>
                    Название 2 команды<input type="text" value={Team2Name} onChange={handleTeam2NameChange} />
                </div>
            </div>
            <div style={{ display: "flex", overflow: "auto" }}>
                <div className="block scrollable flex-grow-1" style={{ flex: 1 }}>
                    <h3>Выберите задачи</h3>
                    <div style={{ height: "400px", overflow: "auto" }}>
                        <table class="table flex-grow-1"  >
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
                                </tr>
                            </thead>
                            <tbody>
                                {Tasks?.map((task) => {
                                    function handleTaskChoiceChange(task: { Task: Task; chosen: boolean }): void {
                                        let currentTasks = Tasks
                                        let chosenTask = currentTasks?.filter((t) => {
                                            if (t.Task.Id == task.Task.Id)
                                                return true
                                            return false
                                        }).pop()
                                        if (chosenTask !== undefined)
                                            chosenTask.chosen = !chosenTask.chosen
                                        if (currentTasks !== undefined)
                                            setTasks([...currentTasks])
                                    }

                                    return (
                                        <>
                                            <tr key={task.Task.Id}>
                                                <td>
                                                    {task.Task.Id}
                                                </td>
                                                <td>
                                                    {task.Task.Text}
                                                </td>
                                                <td>
                                                    <input type="checkbox" checked={task.chosen} onChange={() => handleTaskChoiceChange(task)} key={task.Task.Id} /> Выбрать
                                                </td>
                                            </tr>
                                        </>
                                    )
                                }
                                )}
                            </tbody>
                        </table>
                    </div>
                    <input type="button" value="Ещё" onClick={getTasks} />
                </div>
                <div className="block scrollable flex-grow-1" style={{ flex: 1, height: "50%" }} >
                    <h3>Выберите студентов</h3>
                    <div style={{ height: "400px", overflow: "auto" }}>
                        <table border="1px" class="flex-grow-1 table table-bordered" width="50%">
                            <thead>
                                <tr>
                                    <th>
                                        Id
                                    </th>
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

                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                {Students?.map((student) => {
                                    return (
                                        <tr key={student.Student.Id}>
                                            <td>
                                                {student.Student.Id}
                                            </td>
                                            <td>
                                                {student.Student.Name}
                                            </td>
                                            <td>
                                                {student.Student.Surname}
                                            </td>
                                            <td>
                                                {student.Student.Fatname}
                                            </td>
                                            <td>
                                                {student.Student.Email}
                                            </td>
                                            <td>
                                                <input type="checkbox" checked={student.role.isTeam1} onChange={() => {
                                                    handleisTeam1Change(student.Student)
                                                }} />Команда 1
                                                <input type="checkbox" checked={student.role.isTeam1Lead} onChange={() => {
                                                    handleisTeam1LeadChange(student.Student)
                                                }} />Капитан 1 команды
                                                <input type="checkbox" checked={student.role.isSemiTeam1Lead} onChange={() => {
                                                    handleisSemiTeam1LeadChange(student.Student)
                                                }} />Заместитель капитана 1 команды<br />
                                                <input type="checkbox" checked={student.role.isTeam2} onChange={() => {
                                                    handleisTeam2Change(student.Student)
                                                }} />Команда 2
                                                <input type="checkbox" checked={student.role.isTeam2Lead} onChange={() => {
                                                    handleisTeam2LeadChange(student.Student)
                                                }} />Капитан 2 команды
                                                <input type="checkbox" checked={student.role.isSemiTeam2Lead} onChange={() => {
                                                    handleisSemiTeam2LeadChange(student.Student)
                                                }} />Заместитель капитана 2 команды
                                            </td>
                                        </tr>

                                    )
                                })}
                            </tbody>
                        </table>
                    </div>
                    <input type="button" value="Ещё" onClick={getStudents} />
                </div>
            </div >
        </>
    )
}