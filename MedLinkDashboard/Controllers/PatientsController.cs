using MedLinkDashboard.IRepository;
using MedLinkDashboard.Models;
using MedLinkDashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MedLinkDashboard.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IRepository<Patient> _Patientrepository;
        public PatientsController(IRepository<Patient> Patientrepository)
        {
            _Patientrepository = Patientrepository;
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



    }
}
