using MedLinkDashboard.Models;

namespace MedLinkDashboard.IRepository
{
    public interface ISpecialityRepository : IRepository<Speciality>
    {
        void Add(Speciality speciality);
        void Update(Speciality speciality);
        void Delete(Speciality speciality);
        bool CheckUnique(int id, string name);
    }
}
