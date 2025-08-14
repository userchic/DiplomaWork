using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Records
{
    public class RoundRecord
    {
        public int SpeakerId { get; set; }
        [Required(ErrorMessage = "Не указан оппонент")]
        [Remote(action: "CheckStudentExist", controller: "Admin", ErrorMessage = "Выбран не существующий студент-оппонент")]
        public int OpponentId { get; set; }
        [Required(ErrorMessage = "Не указана игра")]
        [Remote(action: "CheckGameExist", controller: "Admin", ErrorMessage = "Выбрана не существующая игра")]
        public int GameId { get; set; }

    }
}
