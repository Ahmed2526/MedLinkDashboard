using MedLinkDashboard.Data;
using MedLinkDashboard.IRepository;
using MedLinkDashboard.Models;
using MedLinkDashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedLinkDashboard.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IRepository<Patient> _Patientrepository;
        private readonly ApplicationDbContext _context;

        public PatientsController(IRepository<Patient> Patientrepository, ApplicationDbContext context)
        {
            _Patientrepository = Patientrepository;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var data = _Patientrepository.GetAll()
                .Select(e => new PatientVM()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone
                });

            return View(data);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var data = _Patientrepository.GetById(id);

            if (data is null)
                return NotFound();

            var vm = new PatientVM()
            {
                Id = data.Id,
                Name = data.Name,
                Email = data.Email,
                Phone = data.Phone
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Appointmenthistory(int id)
        {
            var data = await _context.Appointments.Where(e => e.PatientId == id).Select(e => new PatientAppointmentVM()
            {
                Id = e.Id,
                Patient = e.Patient.Name,
                Doctor = e.Doctor.FirstName + " " + e.Doctor.LastName,
                Clinic = e.Clinic.Name,
                Day = e.Day,
                AppointmentStart = e.AppointmentStart,
                AppointmentEnd = e.AppointmentEnd,
                Status = e.Status,
                Price = e.Price
            }).ToListAsync();

            return View(data);
        }


    }
}
