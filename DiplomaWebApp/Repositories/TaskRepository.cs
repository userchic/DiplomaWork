using DiplomaWebApp.Abstractions;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;


namespace DiplomaWebApp.Repositories
{
    public class TaskRepository : Repository,ITaskRepository
    {
        public TaskRepository(MathBattlesDbContext context) : base(context)
        {
        }
        public ICollection<Problem> GetTasks()
        {
            return _context.Tasks.Select(x => x).ToArray();
        }
        public ICollection<Problem> GetTasksRange(List<int> ids)
        {
            return _context.Tasks.Where(x => ids.Contains(x.Id)).ToArray();
        }
        public Problem GetTask(int Id)
        {
            return _context.Tasks.Find(Id);
        }
        public void AddTask(Problem task)
        {
            _context.Tasks.Add(task);
        }
        public void UpdateTask(Problem task)
        {
            _context.Tasks.Update(task);
        }
        public void RemoveTask(Problem task)
        {
            _context.Tasks.Remove(task);
        }
    }
}
