using DiplomaWebApp.Abstractions;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;
using Microsoft.EntityFrameworkCore;


namespace DiplomaWebApp.Repositories
{
    public class TaskRepository : Repository,ITaskRepository
    {
        public TaskRepository(MathBattlesDbContext context) : base(context)
        {
        }
        public ICollection<Problem> GetTasks(int page=1)
        {
            IList<Problem> problem = _context.Tasks.OrderBy(x => -x.Id).Take(1).ToList();
            if (problem.Count == 1)
            {
                int lastId = problem[0].Id;
                return _context.Tasks.OrderBy(x => -x.Id).Where(x => (x.Id > lastId - pageSize * page && x.Id <= lastId - pageSize * (page - 1))).ToArray();
            }
            else
                return (ICollection<Problem>)Array.CreateInstance(typeof(Problem), 0);
        }

        public ICollection<Problem> GetTasksRange(List<int> ids)
        {
            return _context.Tasks.Where(x => ids.Contains(x.Id)).ToArray();
        }
        public Problem? GetTask(int Id)
        {
            return _context.Tasks.Find(Id);
        }
        public Problem? GetTaskInfo(int Id)
        {
            return _context.Tasks
                .Include(x => x.Games)
                .Include(x => x.Rounds).ThenInclude(x => x.RoundResults).ThenInclude(x => x.Mistakes)
                .Include(x => x.Rounds).ThenInclude(x => x.Speaker)
                .Include(x => x.Rounds).ThenInclude(x => x.Opponent)
                .Where(x => x.Id == Id).FirstOrDefault();
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
