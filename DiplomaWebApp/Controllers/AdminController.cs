using Microsoft.AspNetCore.Authorization;
using DiplomaWebApp.Abstractions;
using DiplomaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using DiplomaWebApp.Records;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace DiplomaWebApp.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class AdminController : Controller
    {
        IGameRepository gameRep;
        ITaskRepository taskRep;
        ITeamRepository teamRep;
        IStudentRepository studentRep;
        public AdminController(IGameRepository gamerep, ITaskRepository taskrep, IStudentRepository studrep,ITeamRepository teamrep)
        {
            gameRep = gamerep;
            taskRep = taskrep;
            studentRep = studrep;
            teamRep = teamrep;
        }
        #region Get Запросы
        [HttpGet]
        public IActionResult Games()
        {
            List<Game> games = gameRep.GetGames().ToList();
            return View(games);
        }
        [HttpGet]
        public IActionResult Tasks()
        {
            List<Problem> tasks = taskRep.GetTasks().ToList();
            return View(tasks);
        }
        [HttpGet]
        public IActionResult Task(int id)
        {
            Problem task = taskRep.GetTask(id);
            if (task is null)
                NotFound();
            return View(new TaskRecord() 
            {
                Text = task.Text,
                Id = task.Id 
            });
        }
        [HttpGet]
        public IActionResult Student(int id)
        {
            Student student = studentRep.GetStudent(id);
            if (student is null)
                NotFound();
            return View(student);
        }
        [HttpGet]
        public IActionResult Students()
        {
            List<Student> student = studentRep.GetStudents().ToList();
            return View(student);
        }
        [HttpGet]
        public IActionResult CreateGame()
        {
            CreateGameModel model = new CreateGameModel() { Tasks = taskRep.GetTasks().ToList(), Students = studentRep.GetStudents().ToList() };
            return View(model);
        }
        [HttpGet]
        public IActionResult CreateStudent()
        {
            return View("CreateStudent");
        }
        [HttpGet]
        public IActionResult CreateTask()
        {
            return View("CreateTask");
        }
        #endregion
        [HttpPost]
        public IActionResult CreateGame(
            GameRecord game)
        {
            if(!ModelState.IsValid)
            {
                return ShowFirstError("CreateGame");
            }
            List<Problem> tasks= taskRep.GetTasksRange(game.chosenTasksIds).ToList();
            Team team1 = new Team()
            {
                Students = studentRep.GetStudentsRange(game.studentsTeam1),
                Captain = studentRep.GetStudent(game.team1CaptainId),
                CaptainId = studentRep.GetStudent(game.team1CaptainId).Id,
                Name = game.team1Name,
                ViceCaptainId = game.team1ViceCaptainId is not null ? studentRep.GetStudent((int)game.team1ViceCaptainId).Id : null
            };
            Team team2 = new Team()
            {
                Students = studentRep.GetStudentsRange(game.studentsTeam2),
                Captain = studentRep.GetStudent(game.team2CaptainId),
                CaptainId = studentRep.GetStudent(game.team2CaptainId).Id,
                Name = game.team2Name,
                ViceCaptainId = game.team2ViceCaptainId is not null ? studentRep.GetStudent((int)game.team2ViceCaptainId).Id : null
            };
            teamRep.AddTeam(team1);
            teamRep.AddTeam(team2);
            teamRep.Save();
            Game newGame = new Game()
            {
                Name= game.Name,
                SolvingTime= game.SolvingTime,
                StartTime= game.StartTime,
                CaptainsRoundFormat = game.captainsRoundFormat,
                Tasks=tasks,
                Team1=team1,
                Team2=team2,
            };
            gameRep.AddGame(newGame);
            gameRep.Save();
            return Games();
        }
        [HttpPost]
        public IActionResult CreateStudent(StudentRecord newStudent)
        {
            if(!ModelState.IsValid)
            {
                return ShowFirstError("CreateStudent");
            }
            studentRep.AddStudent(new Student(newStudent));
            studentRep.Save();
            return ViewWithMessage("Студент создан успешно", "CreateStudent");
        }
        [HttpPost]
        public IActionResult CreateTask(TaskRecord newTask)
        {
            if (!ModelState.IsValid)
            {
                return ShowFirstError("CreateTask");
            }
            taskRep.AddTask(new Problem(newTask.Text));
            taskRep.Save();
            return ViewWithMessage("Задача создана успешно", "CreateTask");
        }
        [HttpPut]
        public IActionResult UpdateTask([FromBody]TaskRecord changedTask)
        {
            if (!ModelState.IsValid)
            {
                return JsonFirstError();
            }
            Problem dbTask = taskRep.GetTask(changedTask.Id);
            if (dbTask is null)
            {
                return NotFound();
            }
            dbTask.Text = changedTask.Text;
            taskRep.Save();
            return Json(new { success = 1, message = "Успешно отредактирована задача" });
        }
        [HttpPut]
        public IActionResult UpdateStudent([FromBody] StudentRecord changedStudent)
        {
            Student dbStudent = studentRep.GetStudent(changedStudent.Id);
            if (dbStudent is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return JsonFirstError();
            }
            dbStudent.Name = changedStudent.Name;
            dbStudent.Surname = changedStudent.Surname;
            dbStudent.Fatname = changedStudent.Fatname;
            dbStudent.Email = changedStudent.Email;
            //studentRep.UpdateStudent(dbStudent);
            studentRep.Save();
            return Json(new { success = 1, message = "Успешно отредактирован студент" });
        }
        [HttpDelete]
        public IActionResult DeleteTask(int id)
        {
            Problem taskToBeDeleted = taskRep.GetTask(id);
            if (taskToBeDeleted is null)
            {
                return NotFound();
            }
            taskRep.RemoveTask(taskToBeDeleted);
            taskRep.Save();
            return Json(new { success = 1, message = "Успешно удален студент" });
        }
        [HttpDelete]
        public IActionResult DeleteStudent(int id)
        {
            Student studentToBeDeleted = studentRep.GetStudent(id);
            if (studentToBeDeleted is null)
            {
                return NotFound();
            }
            studentRep.RemoveStudent(studentToBeDeleted);
            studentRep.Save(); 
            return Json(new { success = 1, message = "Успешно удален студент" });
        }
        public IActionResult CheckTasksExist(List<int> ids)
        {
            List<Problem> tasks=taskRep.GetTasksRange(ids).ToList();
            if (tasks.Count == ids.Count)
                return Json(true);
            return Json(false);
        }
        public IActionResult CheckStudentsExist(List<int> ids)
        {
            List<Student> student = studentRep.GetStudentsRange(ids).ToList();
            if (student.Count == ids.Count)
                return Json(true);
            return Json(false);
        }
        public IActionResult CheckStudentsExist(int id)
        {
            Student student = studentRep.GetStudent(id);
            if (student is not null) 
                return Json(true);
            return Json(false);
        }
        private IActionResult ViewWithMessage(string message,string viewName)
        {
            ViewBag.Message = message;
            return View(viewName);
        }
        private IActionResult ShowFirstError(string viewName)
        {
            return ViewWithMessage(ModelState.First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage, viewName);
        }
        private IActionResult JsonFirstError()
        {
            return Json(new { success = 0, message = ModelState.First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage });
        }
    }
}
