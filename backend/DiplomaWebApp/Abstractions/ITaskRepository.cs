using DiplomaWebApp.Models;

namespace DiplomaWebApp.Abstractions
{
    public interface ITaskRepository:IRepository
    {
        public ICollection<Problem> GetTasks(int page=1);
        public ICollection<Problem> GetTasksRange(List<int> ids);
        public Problem? GetTask(int Id);
        public Problem? GetTaskInfo(int Id);
        public void AddTask(Problem task);
        public void UpdateTask(Problem task);
        public void RemoveTask(Problem task);
    }
}
