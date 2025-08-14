using DiplomaWebApp.Models;

namespace DiplomaWebApp.Abstractions
{
    public interface IStudentRepository:IRepository
    {
        public ICollection<Student> GetStudents();
        public ICollection<Student> GetStudentsRange(List<int> ids);
        public Student GetStudent(int Id);
        public void AddStudent(Student student);
        public void UpdateStudent(Student student);
        public void RemoveStudent(Student student);
    }
}
