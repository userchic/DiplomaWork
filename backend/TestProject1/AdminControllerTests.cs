using DiplomaWebApp.Abstractions;
using DiplomaWebApp.Controllers;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;
using DiplomaWebApp.Records;
using DiplomaWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestProject1
{
    public class AdminControllerTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(10)]
        public void GetGames_FromEmptyDB_EmptyResult(int page)
        {
            ImplyControllerTest((context, controller) =>
            {

                // Act: Perform the action to be tested
                JsonResult res = (JsonResult)controller.GetGames(page);

                // Assert: Verify the result
                string expectedResult = "{\"$id\":\"1\",\"$values\":[]}";
                Assert.Equal(JsonSerializer.Serialize(expectedResult), JsonSerializer.Serialize(res.Value));
            });
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(10)]
        public void GetTasks_FromEmptyDB_EmptyResult(int page)
        {
            ImplyControllerTest((context, controller) =>
            {

                // Act: Perform the action to be tested
                JsonResult res = (JsonResult)controller.GetTasks(page);

                // Assert: Verify the result
                string expectedResult = "[]";
                Assert.Equal(JsonSerializer.Serialize(expectedResult), JsonSerializer.Serialize(res.Value));
            });
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(-1)]
        [InlineData(-2)]
        public void GetTask_NonExistingTasks_NotFound(int id)
        {
            ImplyControllerTest((context, controller) =>
            {

                // Act: Perform the action to be tested
                NotFoundResult res = (NotFoundResult)controller.GetTask(id);

                // Assert: Verify the result
                NotFoundResult expectedResult = new NotFoundResult();
                Assert.Equal(expectedResult.ToString(), res.ToString());
            });
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        public void GetStudent_NonExistingStudents_NotFound(int id)
        {
            ImplyControllerTest((context, controller) =>
            {

                // Act: Perform the action to be tested
                NotFoundResult res = (NotFoundResult)controller.GetStudent(id);

                // Assert: Verify the result
                NotFoundResult expectedResult = new NotFoundResult();
                Assert.Equal(expectedResult.StatusCode.ToString(), res.StatusCode.ToString());
            });
        }
        [Theory]
        [InlineData("Супер","Крутой","Студент","","Школа")]
        public void CreateStudent_ValidStudent_SuccessAndHaveItInDB(string name,string surname,string fatname,string email,string educationFacility)
        {
            ImplyControllerTest((context, controller) =>
            {

                // Act: Perform the action to be tested
                StudentRecord request = new StudentRecord()
                {
                    Name = name,
                    Surname = surname,
                    Fatname = fatname,
                    Email = email,
                    EducationFacility = educationFacility
                };
                JsonResult res1 = (JsonResult)controller.CreateStudent(request);
                int expectedId = context.Students.FirstOrDefault(x => x.Name == name && x.Surname == surname && x.Fatname == fatname && x.EducationFacility == educationFacility && x.Email == "").Id;
                JsonResult res2 = (JsonResult)controller.GetStudent(expectedId);
                // Assert: Verify the result
                JsonResult expectedResult1 = controller.Json(new { success = 1, message = "Студент создан успешно" });
                JsonResult expectedResult2 = controller.Json(new Student() { Id = expectedId, Name = "Супер", Surname = "Крутой", Fatname = "Студент", Email = "", EducationFacility = "Школа" });
                Assert.Equal(JsonSerializer.Serialize(expectedResult1.Value), JsonSerializer.Serialize(res1.Value));
                Assert.Equal(JsonSerializer.Serialize(expectedResult2.Value), JsonSerializer.Serialize(res2.Value));
            });
        }
        [Theory]
        [InlineData(["Студент","Новый","Человек","Школа2",
                     "Суперстудент","Умный","чел","Школа3"])]
        public void UpdateStudent_ValidTask_SuccessAndHaveItInDB(
            string name1, string surname1, string fatname1, string educationFacility1,
            string name2, string surname2, string fatname2, string educationFacility2)
        {
            ImplyControllerTest((context, controller) =>
            {
                StudentRecord request = new StudentRecord()
                {
                    Name = name1,
                    Surname = surname1,
                    Fatname = fatname1,
                    EducationFacility = educationFacility1,
                    Email = ""
                };


                // Act: Perform the action to be tested
                JsonResult res1 = (JsonResult)controller.CreateStudent(request);
                int expectedId = context.Students.FirstOrDefault(x => x.Name == name1 && x.Surname == surname1 && x.Fatname == fatname1 && x.EducationFacility == educationFacility1 && x.Email == "").Id;
                StudentRecord changedStudent = new StudentRecord()
                {
                    Name = name2,
                    Surname = surname2,
                    Fatname = fatname2,
                    EducationFacility = educationFacility2,
                    Email = "",
                    Id = expectedId
                };
                JsonResult res2 = (JsonResult)controller.UpdateStudent(changedStudent);
                JsonResult res3 = (JsonResult)controller.GetStudent(expectedId);
                JsonResult expectedResult1 = controller.Json(new { success = 1, message = "Студент создан успешно" });
                JsonResult expectedResult2 = controller.Json(new { success = 1, message = "Успешно отредактирован студент" });
                JsonResult expectedResult3 = controller.Json(new Student(changedStudent));

                // Assert: Verify the result
                Assert.Equal(JsonSerializer.Serialize(expectedResult1.Value), JsonSerializer.Serialize(res1.Value));
                Assert.Equal(JsonSerializer.Serialize(expectedResult2.Value), JsonSerializer.Serialize(res2.Value));
                Assert.Equal(JsonSerializer.Serialize(expectedResult3.Value), JsonSerializer.Serialize(res3.Value));
            });
        }
        [Theory]
        [InlineData(["Не","Настоящий","Студент","Ненастоящая школа"])]
        public void UpdateStudent_NotCreatedStudent_NotFound(
            
            string name, string surname, string fatname, string educationFacility)
        {
            ImplyControllerTest((context, controller) =>
            {
                StudentRecord request = new StudentRecord()
                {
                    Name = name,
                    Surname = surname,
                    Fatname = fatname,
                    EducationFacility = educationFacility,
                    Email = "",
                    Id=-1
                };


                // Act: Perform the action to be tested
                
                NotFoundResult res = (NotFoundResult)controller.UpdateStudent(request);
                NotFoundResult expeectedResult = new NotFoundResult();

                // Assert: Verify the result
                Assert.Equal(expeectedResult.StatusCode, res.StatusCode);
            });
        }
        [Fact]
        public void DeleteStudent_ExistingStudent_SuccessAndNotHaveItInDB()
        {
            ImplyControllerTest((context, controller) =>
            {
                StudentRecord studentToBeDeleted = new StudentRecord()
                {
                    Name = "Студент",
                    Surname = "Студент",
                    Fatname = "Студент",
                    EducationFacility = "Школа",
                    Email = ""
                };


                // Act: Perform the action to be tested
                JsonResult res1 = (JsonResult)controller.CreateStudent(studentToBeDeleted);
                int expectedId = context.Students.FirstOrDefault(x => x.Name == studentToBeDeleted.Name && x.Surname == studentToBeDeleted.Surname && x.Fatname == studentToBeDeleted.Fatname && x.EducationFacility == studentToBeDeleted.EducationFacility && x.Email == studentToBeDeleted.Email).Id;

                JsonResult res2 = (JsonResult)controller.DeleteStudent(expectedId);
                NotFoundResult res3 = (NotFoundResult)controller.GetStudent(expectedId);
                JsonResult expectedResult1 = controller.Json(new { success = 1, message = "Студент создан успешно" });
                JsonResult expectedResult2 = controller.Json(new { success = 1, message = "Успешно удален студент" });
                NotFoundResult expectedResult3 = new NotFoundResult();

                // Assert: Verify the result
                Assert.Equal(JsonSerializer.Serialize(expectedResult1.Value), JsonSerializer.Serialize(res1.Value));
                Assert.Equal(JsonSerializer.Serialize(expectedResult2.Value), JsonSerializer.Serialize(res2.Value));
                Assert.Equal(expectedResult3.StatusCode, res3.StatusCode);
            });
        }
        [Fact]
        public void DeleteStudent_NotExistingStudent_NotFound()
        {
            ImplyControllerTest((context, controller) =>
            {
                // Act: Perform the action to be tested
                NotFoundResult res = (NotFoundResult)controller.DeleteStudent(-1);
                NotFoundResult expectedResult = new NotFoundResult();

                // Assert: Verify the result
                Assert.Equal(JsonSerializer.Serialize(expectedResult.StatusCode), JsonSerializer.Serialize(res.StatusCode));
            });
        }
        [Theory]
        [InlineData("Сложная задача: 1+1=?")]
        public void CreateTask_ValidTask_SuccessAndHaveItInDB(string text)
        {
            ImplyControllerTest((context, controller) =>
            {
                // Act: Perform the action to be tested
                TaskRecord request = new TaskRecord()
                {
                    Text = text
                };

                JsonResult res1 = (JsonResult)controller.CreateTask(request);
                int expectedId = context.Tasks.FirstOrDefault(x=>x.Text==text).Id;
                JsonResult res2 = (JsonResult)controller.GetTask(expectedId);
                // Assert: Verify the result
                JsonResult expectedResult1 = controller.Json(new { success = 1, message = "Успешно создана задача" });
                JsonResult expectedResult2 = controller.Json(new Problem() { Id = expectedId, Text = text });
                Assert.Equal(JsonSerializer.Serialize(expectedResult1.Value), JsonSerializer.Serialize(res1.Value));
                Assert.Equal(JsonSerializer.Serialize(expectedResult2.Value), JsonSerializer.Serialize(res2.Value));
            });
        }
        [Theory]
        [InlineData(["Новая задача","Измененная новая задача"])]
        public void UpdateTask_ValidTask_SuccessAndHaveItInDB(string text1,string text2)
        {
            ImplyControllerTest((context, controller) =>
            {
                TaskRecord request = new TaskRecord()
                {
                    Text = text1
                };
                

                // Act: Perform the action to be tested
                JsonResult res1 = (JsonResult)controller.CreateTask(request);
                int expectedId = context.Tasks.FirstOrDefault(x => x.Text == text1).Id;
                TaskRecord changedTask = new TaskRecord()
                {
                    Text = text2,
                    Id= expectedId
                };
                JsonResult res2 = (JsonResult)controller.UpdateTask(changedTask);
                JsonResult res3 = (JsonResult)controller.GetTask(expectedId);
                JsonResult expectedResult1 = controller.Json(new { success = 1, message = "Успешно создана задача" });
                JsonResult expectedResult2 = controller.Json(new { success = 1, message = "Успешно отредактирована задача" });
                JsonResult expectedResult3 = controller.Json(new Problem(changedTask));

                // Assert: Verify the result
                Assert.Equal(JsonSerializer.Serialize(expectedResult1.Value), JsonSerializer.Serialize(res1.Value));
                Assert.Equal(JsonSerializer.Serialize(expectedResult2.Value), JsonSerializer.Serialize(res2.Value));
                Assert.Equal(JsonSerializer.Serialize(expectedResult3.Value), JsonSerializer.Serialize(res3.Value));
            });
        }
        [Theory]
        [InlineData(["Несуществующая задача"])]
        public void UpdateTask_NotCreatedTask_NotFound(string text)
        {
            ImplyControllerTest((context, controller) =>
            {
                TaskRecord request = new TaskRecord()
                {
                    Text=text,
                    Id=-1
                };


                // Act: Perform the action to be tested

                NotFoundResult res = (NotFoundResult)controller.UpdateTask(request);
                NotFoundResult expeectedResult = new NotFoundResult();

                // Assert: Verify the result
                Assert.Equal(expeectedResult.StatusCode, res.StatusCode);
            });
        }
        public void DeleteTask_ExistingTask_SuccessAndNotHaveItInDB()
        {
            ImplyControllerTest((context, controller) =>
            {
                TaskRecord taskToBeDeleted = new TaskRecord()
                {
                    Text="Текст временной задачи"
                };


                // Act: Perform the action to be tested
                JsonResult res1 = (JsonResult)controller.CreateTask(taskToBeDeleted);
                int expectedId = context.Tasks.FirstOrDefault(x => x.Text==taskToBeDeleted.Text).Id;

                JsonResult res2 = (JsonResult)controller.DeleteTask(expectedId);
                NotFoundResult res3 = (NotFoundResult)controller.GetTask(expectedId);
                JsonResult expectedResult1 = controller.Json(new { success = 1, message = "Студент создан успешно" });
                JsonResult expectedResult2 = controller.Json(new { success = 1, message = "Успешно удален студент" });
                NotFoundResult expectedResult3 = new NotFoundResult();

                // Assert: Verify the result
                Assert.Equal(JsonSerializer.Serialize(expectedResult1.Value), JsonSerializer.Serialize(res1.Value));
                Assert.Equal(JsonSerializer.Serialize(expectedResult2.Value), JsonSerializer.Serialize(res2.Value));
                Assert.Equal(expectedResult3.StatusCode, res3.StatusCode);
            });
        }
        [Fact]
        public void DeleteTask_NotExistingTask_NotFound()
        {
            ImplyControllerTest((context, controller) =>
            {
                // Act: Perform the action to be tested
                NotFoundResult res = (NotFoundResult)controller.DeleteTask(-1);
                NotFoundResult expectedResult = new NotFoundResult();

                // Assert: Verify the result
                Assert.Equal(JsonSerializer.Serialize(expectedResult.StatusCode), JsonSerializer.Serialize(res.StatusCode));
            });
        }
        [Fact]
        public void CreateGame_ValidGame_SuccessAndHaveItInDB()
        {
            ImplyControllerTest((context, controller) =>
            {
                // Act: Perform the action to be tested
                GameRecord request = new GameRecord()
                {
                    captainsRoundFormat="Normal Round",
                    chosenTasksIds = [1,2],
                    EventPlace="Школа",
                    Name="Тестовый бой",
                    PlannedStartTime=DateTime.Today.AddMonths(1),
                    SolvingTime=10,
                    studentsTeam1 = [1,2],
                    studentsTeam2 = [3,4],
                    team1CaptainId=1,
                    team2CaptainId=3,
                    team1Name="Team1",
                    team2Name="Team2",
                    team1ViceCaptainId=2,
                    team2ViceCaptainId=4,
                };
                TaskRecord task1 = new TaskRecord()
                {
                    Text = "Задача1"
                };
                TaskRecord task2 = new TaskRecord()
                {
                    Text = "Задача2"
                };

                controller.CreateTask(task1);
                controller.CreateTask(task2);
                JsonResult res = (JsonResult)controller.CreateGame(request);
                // Assert: Verify the result
                JsonResult expectedResult = controller.Json(new { success = 1, message = "Игра успешно создана" });
                Assert.Equal(JsonSerializer.Serialize(expectedResult.Value), JsonSerializer.Serialize(res.Value));
            });
        }
        private void ImplyControllerTest(Action<MathBattlesDbContext, AdminController> test)
        {
            var options = new DbContextOptionsBuilder<MathBattlesDbContext>()
                .UseInMemoryDatabase("testDB")
                .Options;
            using (var context = new MathBattlesDbContext(options))
            {
                // Arrange: Seed data into the context
                AdminController controller = InitializeAdminController(context);
                test(context, controller);
            }
        }
        public AdminController InitializeAdminController(MathBattlesDbContext context)
        {
            IGameRepository gameRep = new GameRepository(context);
            ITaskRepository taskRep = new TaskRepository(context);
            IStudentRepository studRep = new StudentRepository(context);
            ITeamRepository teamRep = new TeamRepository(context);
            AdminController controller = new AdminController(gameRep, taskRep, studRep, teamRep);
            return controller;
        }
    }
}
