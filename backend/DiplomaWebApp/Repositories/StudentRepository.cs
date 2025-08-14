using DiplomaWebApp.Abstractions;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Repositories
{
    public class StudentRepository : Repository,IStudentRepository
    {
        public StudentRepository(MathBattlesDbContext context) : base(context)
        {
        }

        public ICollection<Student> GetStudents()
        {
            return _context.Students.Select(x => x).ToArray();
        }
        public ICollection<Student> GetStudentsRange(List<int> ids)
        {
            return _context.Students.Where(x => ids.Contains(x.Id)).ToArray();
        }

        public Student GetStudent(int Id)
        {
            return _context.Students.Find(Id);
        }
        public void AddStudent(Student student)
        {
            _context.Students.Add(student);
        }
        public void UpdateStudent(Student student)
        {
            _context.Students.Update(student);
        }
        public void RemoveStudent(Student student)
        {
            _context.Students.Remove(student);
        }
    }
}
