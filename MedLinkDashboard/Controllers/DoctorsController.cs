using MedLinkDashboard.Data;
using MedLinkDashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedLinkDashboard.Controllers
{
    [Authorize]
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {

            var data = _context.Doctors
                .AsNoTracking()
                .Select(e => new DoctorVM()
                {
                    Id = e.Id,
                    Name = e.FirstName + " " + e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Speciality = e.Speciality.Name
                }).ToList();

            return View(data);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var doc = _context.Doctors
                .Include(e => e.Speciality)
                .FirstOrDefault(e => e.Id == id);

            if (doc is null)
                return NotFound();

            var docVM = new DoctorVM()
            {
                Id = doc.Id,
                Name = doc.FirstName + " " + doc.LastName,
                Email = doc.Email,
                Phone = doc.Phone,
                Speciality = doc.Speciality.Name
            };

            return View(docVM);
        }


    }
}
