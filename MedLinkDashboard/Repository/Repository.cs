using MedLinkDashboard.Data;
using MedLinkDashboard.IRepository;

namespace MedLinkDashboard.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            var data = _context.Set<T>().ToList();
            return data;
        }

        public T GetById(int id)
        {
            var data = _context.Set<T>().Find(id);
            return data;
        }

        public int Save()
        {
            int result = _context.SaveChanges();
            return result;
        }
    }
}
