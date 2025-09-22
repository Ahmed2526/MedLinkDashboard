using MedLinkDashboard.Data;
using MedLinkDashboard.IRepository;
using MedLinkDashboard.Models;

namespace MedLinkDashboard.Repository
{
    public class SpecialityRepository : Repository<Speciality>, ISpecialityRepository
    {
        public SpecialityRepository(ApplicationDbContext context) : base(context)
        {

        }

        public void Add(Speciality speciality)
        {
            _context.Add(speciality);
        }
        public void Update(Speciality speciality)
        {
            _context.Update(speciality);
        }

        public void Delete(Speciality speciality)
        {
            _context.Remove(speciality);
        }

        public bool CheckUnique(int id, string name)
        {
            var isExist = _context.Specialities
                .Any(e => e.Name == name && e.Id != id);

            return isExist;
        }
    }
}
