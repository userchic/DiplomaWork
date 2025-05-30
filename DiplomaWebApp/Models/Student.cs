using DiplomaWebApp.Records;
using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fatname { get; set; }
        public string Email { get; set; }
        public ICollection<Team> Teams { get; set; }
        public Student()
        {

        }

        public Student(StudentRecord studentRecord)
        {
            Id = studentRecord.Id;
            Name = studentRecord.Name;
            Surname = studentRecord.Surname;
            Fatname = studentRecord.Fatname;
            Email = studentRecord.Email;
        }
    }
}
