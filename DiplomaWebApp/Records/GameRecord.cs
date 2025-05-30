using DiplomaWebApp.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Records
{
    public record class GameRecord
    {
        [Required(ErrorMessage = "Не указано время начала игры, укажите его")]
        [UpToDate(ErrorMessage = "Дата начала игры должна указывать на момент времени в будущем")]
        public DateTime StartTime{get;set;}
        [Required(ErrorMessage = "Не указана длительность решения задач, укажите её")]
        [Range(10, 300, ErrorMessage = "Указано слишком маленькое (<10) или слишком большое(>300) значение длительности игры")]
        public int SolvingTime { get;set;}
        [Required(ErrorMessage = "Не указано название игры, укажите его")]
        [StringLength(50, MinimumLength = 3,ErrorMessage ="Длина названия игры может быть от 0 до 50")]
        public string Name { get;set;}
        [Required(ErrorMessage = "Не указан формат капитанского раунда, укажите его")]
        [StringLength(999, MinimumLength = 10,ErrorMessage ="объем капитанского раунда может быть от 10 до 999 символов")]
        public string captainsRoundFormat { get;set;}
        [Required(ErrorMessage ="Не выбраны задачи для игры")]
        [Remote(action: "CheckTasksExist", controller: "Admin", ErrorMessage = "Выбраны не существующие задачи")]
        [MasRange(0,51,ErrorMessage ="Количество указанных задач может быть от 1 до 50")]
        public List<int> chosenTasksIds{get;set;}


        [Required(ErrorMessage ="Не выбраны члены первой команды")]
        [Remote(action: "CheckStudentsExist", controller: "Admin", ErrorMessage = "Выбраны не существующие студенты")]
        [MasRange(0,21,ErrorMessage ="Количество указанных игроков в команде может быть от 1 до 20 человек, измените кол-во указанных студентов в первой команде ")]
        public List<int> studentsTeam1 { get;set; }
        [Required(ErrorMessage = "Не указано название первой команды")]
        [StringLength(50, MinimumLength = 3,ErrorMessage ="Длина названия команды может быть от 3 до 25. введите название первой команды снова")]
        public string team1Name { get;set;}
        [Required(ErrorMessage ="Не указан номер капитана первой команды")]
        [Remote(action: "CheckStudentExist", controller: "Admin", ErrorMessage = "Выбраны не существующий студент")]
        public int team1CaptainId { get;set;}
        [Remote(action: "CheckStudentExist", controller: "Admin", ErrorMessage = "Выбраны не существующий студент")]
        public int? team1ViceCaptainId { get;set; }


        [Required(ErrorMessage = "Не выбраны члены второй команды")]
        [Remote(action: "CheckStudentsExist", controller: "Admin", ErrorMessage = "Выбраны не существующие студенты")]
        [MasRange(0,21,ErrorMessage ="Количество указанных игроков в команде может быть от 1 до 20 человек, измените кол-во указанных студентов во второй команде ")]
        public List<int> studentsTeam2 { get;set; }
        [Required(ErrorMessage = "Не указано название второй команды")]
        [StringLength(25, MinimumLength = 3,ErrorMessage ="Длина названия команды может быть от 3 до 25. введите название первой команды снова")]
        public string team2Name { get;set;}
        [Required(ErrorMessage = "Не указан номер капитана второй команды")]
        [Remote(action: "CheckStudentExist", controller: "Admin", ErrorMessage = "Выбраны не существующий студент")]
        public int team2CaptainId { get; set; }
        [Remote(action: "CheckStudentExist", controller: "Admin", ErrorMessage = "Выбраны не существующий студент")]
        public int? team2ViceCaptainId { get; set; }
    }
}