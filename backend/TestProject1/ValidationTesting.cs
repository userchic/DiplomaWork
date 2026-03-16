using DiplomaWebApp.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class ValidationTesting
    {
        [Theory]
        [InlineData(["суперЛогин", "суперПароль",0])]
        [InlineData(["login", "password",0])]
        [InlineData(["", "суперПароль", 1])]
        [InlineData(["login", "", 1])]
        [InlineData(["", "", 2])]
        public void LoginRequestValidation_DifferentParameters_CorrectAmountOfMistakes(string login, string password,int expectedMistakes)
        {
            LoginRequestRecord request = new LoginRequestRecord()
            {
                Login = password,
                Password = login
            };
            //Act
            IList<ValidationResult> results = ValidationTesting.ValidateModel(request);

            // Assert: Verify the result
            Assert.Equal(expectedMistakes, results.Count);
        }
        [Theory]
        [InlineData(["", "Димонович", "CPU", "CPU1", "dwe23rfe23edw2", 1])]
        [InlineData(["Велес", "", "CPU", "CPU2", "dwaferfew23123232", 1])]
        [InlineData(["Лара", "DI", "Dependency", "", "1222e32f3e234", 1])]
        [InlineData(["", "Димонович", "CPU", "CPU1", "dwe23rfe23edw2", 1])]
        [InlineData(["Велес", "", "CPU", "CPU2", "", 2])]
        [InlineData(["", "", "", "", "", 4])]
        [InlineData(["Тима", "Димонович", "CPU", "CPU1", "dwe23rfe23edw2",0])]
        [InlineData(["Велес", "workinprogress", "CPU", "CPU2", "dwaferfew23123232",0])]
        [InlineData(["Лара", "DI", "Dependency", "CPU3", "1222e32f3e234",0])]
        public void RegistryRequestValidation_DifferentParameters_CorrectAmountOfMistakes(string name, string surname, string? fatname, string login, string password, int expectedMistakes)
        {
            RegistryRequestRecord request = new RegistryRequestRecord()
            {
                Login = login,
                Password = password,
                Name = name,
                Surname = surname,
                Fatname = fatname
            };

            // Act: Perform the action to be tested
            IList<ValidationResult> results = ValidationTesting.ValidateModel(request);

            // Assert: Verify the result
            Assert.Equal(expectedMistakes, results.Count);
        }
        [Theory]
        [InlineData("",1)]
        [InlineData(" ", 1)]
        [InlineData("  ", 1)]
        [InlineData("1", 0)]
        public void TaskRecordTesting_DifferentParameters_CorrectAmountOfMistakes(string text,int expectedMistakes)
        {
            TaskRecord request = new TaskRecord()
            {
                Text = text
            };

            IList<ValidationResult> results = ValidationTesting.ValidateModel(request);

            Assert.Equal(expectedMistakes, results.Count);
        }
        [Theory]
        [InlineData("","","","","",3)]
        [InlineData("Новый","студент","","Школа","",0)]
        [InlineData("Не","Студент","","","",1)]
        [InlineData("","Студент","","Школьник","", 1)]
        public void StudentRecordTesting_DifferentParameters_CorrectAmountOfMistakes(string name,string surname,string fatname,string educationFacility,string email, int expectedMistakes)
        {
            StudentRecord request = new StudentRecord()
            {
                Name=   name,
                Surname = surname,
                Fatname = fatname,
                EducationFacility = educationFacility,
                Email = email
            };

            IList<ValidationResult> results = ValidationTesting.ValidateModel(request);

            Assert.Equal(expectedMistakes, results.Count);
        }
        
        public static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
