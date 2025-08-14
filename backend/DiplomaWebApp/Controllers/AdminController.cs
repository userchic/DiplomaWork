using DiplomaWebApp.Abstractions;
using DiplomaWebApp.Models;
using DiplomaWebApp.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.IdentityModel.Tokens;
using MistralAPIBasedTextGenerator;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiplomaWebApp.Controllers
{
    [Authorize(AuthenticationSchemes ="Cookies")]
    [ApiController]
    [Route("/[controller]/[action]")]
    [EnableCors("AllowOneOrigin")]
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
        public IActionResult GetGames()
        {
            List<Game> games = gameRep.GetGames().ToList();
            string res = JsonSerializer.Serialize(games, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            return Json(res);
        }
        [HttpGet]
        public IActionResult GetTasks()
        {
            List<Problem> tasks = taskRep.GetTasks().ToList();
            return Json(JsonSerializer.Serialize(tasks));
        }
        [HttpGet]
        public IActionResult GetTask(int id)
        {
            Problem task = taskRep.GetTask(id);
            if (task is null)
                NotFound();
            return Json(task);
        }
        [HttpGet]
        public IActionResult GetStudents()
        {
            List<Student> students = studentRep.GetStudents().ToList();
            return Json(JsonSerializer.Serialize(students));
        }
        [HttpGet]
        public IActionResult GetStudent(int id)
        {
            Student student = studentRep.GetStudent(id);
            if (student is null)
                NotFound();
            return Json(student);
        }
        #endregion
        [HttpPost]
        public IActionResult CreateGame(
            GameRecord game)
        {
            if(!ModelState.IsValid)
            {
                return JsonFirstError();
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
                CaptainsRoundFormat = game.captainsRoundFormat,
                Tasks=tasks,
                Team1=team1,
                Team2=team2,
            };
            gameRep.AddGame(newGame);
            gameRep.Save();
            return Json(new {success=1,message="Игра успешно создана"});
        }
        [HttpPost]
        public IActionResult CreateStudent(StudentRecord newStudent)
        {
            if (!ModelState.IsValid)
            {
                return JsonFirstError();
            }
            studentRep.AddStudent(new Student(newStudent));
            studentRep.Save();
            return Json(new {success=1,message="Студент создан успешно"});
        }
        [HttpPost]
        public IActionResult CreateTask(TaskRecord newTask)
        {
            if (!ModelState.IsValid)
            {
                return JsonFirstError();
            }
            taskRep.AddTask(new Problem(newTask.Text));
            taskRep.Save();
            return Json(new {success=1,message="Успешно создана задача"});
        }
        [HttpPost]
        public IActionResult GenerateTask(TaskGenerateRequestRecord newTask)
        {
            //нужно написать ключ перед использованием
            string ApiKey = "";
            try
            {
                MistralAPIBasedTextGenerator.MistralAPIBasedTextGenerator.InitializeClient(ApiKey);
                string generatedText = MistralAPIBasedTextGenerator.MistralAPIBasedTextGenerator.ExecuteGenerateTextRequest(newTask.TaskAmount, newTask.Subject, newTask.Topic, newTask.TaskSize, newTask.QuestionsAmount, newTask.AnswerSize);
                return Json(new { success = 1, message = generatedText });
            }
            catch
            {
                return Json(new { success = 0, message = "Произошла ошибка генератора текста" });
            }
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
        [HttpDelete("{id}")]
        public IActionResult DeleteTask( int id)
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
        [HttpDelete("{id}")]
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
        [HttpGet]
        public IActionResult CheckTasksExist(List<int> ids)
        {
            List<Problem> tasks=taskRep.GetTasksRange(ids).ToList();
            if (tasks.Count == ids.Count)
                return Json(true);
            return Json(false);
        }
        [HttpGet]
        public IActionResult CheckTaskExist(int id)
        {
            Problem task = taskRep.GetTask(id);
            if (task is not null)
                return Json(true);
            return Json(false);
        }
        [HttpGet]
        public IActionResult CheckStudentsExist(List<int> ids)
        {
            List<Student> student = studentRep.GetStudentsRange(ids).ToList();
            if (student.Count == ids.Count)
                return Json(true);
            return Json(false);
        }
        [HttpGet]
        public IActionResult CheckStudentExist(int id)
        {
            Student student = studentRep.GetStudent(id);
            if (student is not null) 
                return Json(true);
            return Json(false);
        }
        [HttpGet]
        public IActionResult CheckGameExist(int id)
        {
            Game game = gameRep.GetGame(id);
            if (game is not null)
                return Json(true);
            return Json(false);
        }
        [HttpGet]
        public IActionResult CheckTeamExist(int id)
        {
            Team team = teamRep.GetTeam(id);
            if (team is not null)
                return Json(true);
            return Json(false);
        }
        private IActionResult JsonFirstError()
        {
            return Json(new { success = 0, message = ModelState.First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage });
        }
    }
}
