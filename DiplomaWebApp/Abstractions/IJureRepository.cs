using DiplomaWebApp.Models;

namespace DiplomaWebApp.Abstractions
{
    public interface IJureRepository:IRepository
    {
        public ICollection<Jure> GetJures();
        public Jure GetJure(string login);
        public void AddJure(Jure jure);
        public void RemoveJure(string login);
        public void EditJure(Jure editedJure);
    }
}