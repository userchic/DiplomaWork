using DiplomaWebApp.Abstractions;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DiplomaWebApp.Repositories
{
    public class StudentRepository : Repository,IStudentRepository
    {
        public StudentRepository(MathBattlesDbContext context) : base(context)
        {
        }

        public ICollection<Student> GetStudents(int page=1)
        {
            IList<Student> student = _context.Students.OrderBy(x => -x.Id).Take(1).ToList();
            if (student.Count == 1)
            {
                int lastId = student[0].Id;
                return _context.Students.OrderBy(x => -x.Id).Where(x => (x.Id > lastId - pageSize * page && x.Id <= lastId - pageSize * (page - 1))).ToArray();
            }
            else
                return (ICollection<Student>)Array.CreateInstance(typeof(Student), 0);
        }
        public ICollection<Student> GetStudentsRange(List<int> ids)
        {
            return _context.Students.Where(x => ids.Contains(x.Id)).ToArray();
        }

        public Student? GetStudent(int id)
        {
            return _context.Students.Find(id);
        }
        public Student? GetStudentInfo(int id)
        {
            var student = _context.Students
        .Include(x => x.Teams)
        .FirstOrDefault(x => x.Id == id);

            if (student == null)
                return null;
            _context.Entry(student)
                .Collection(x => x.SpeakerRounds)
                .Query()
                .Include(x => x.RoundResults)
                .ThenInclude(x => x.Mistakes)
                .Include(x => x.Speaker)
                .Include(x => x.Opponent)
                .Load();
            _context.Entry(student)
                .Collection(x => x.OpponentRounds)
                .Query()
                .Include(x => x.RoundResults)
                .ThenInclude(x => x.Mistakes)
                .Include(x => x.Speaker)
                .Include(x => x.Opponent)
                .Load();
            return student;
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
