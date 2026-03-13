using DiplomaWebApp.Models;

namespace DiplomaWebApp.Abstractions
{
    public interface IStudentRepository:IRepository
    {
        public ICollection<Student> GetStudents(int page=1);
        public ICollection<Student> GetStudentsRange(List<int> ids);
        public Student? GetStudent(int Id);
        public Student? GetStudentInfo(int Id);
        public void AddStudent(Student student);
        public void UpdateStudent(Student student);
        public void RemoveStudent(Student student);
    }
}
