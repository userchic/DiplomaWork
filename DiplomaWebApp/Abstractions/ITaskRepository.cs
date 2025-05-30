using DiplomaWebApp.Models;

namespace DiplomaWebApp.Abstractions
{
    public interface ITaskRepository:IRepository
    {
        public ICollection<Problem> GetTasks();
        public ICollection<Problem> GetTasksRange(List<int> ids);
        public Problem GetTask(int Id);
        public void AddTask(Problem task);
        public void UpdateTask(Problem task);
        public void RemoveTask(Problem task);
    }
}
