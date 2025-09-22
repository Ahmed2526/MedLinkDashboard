using MedLinkDashboard.IRepository;
using MedLinkDashboard.Models;
using MedLinkDashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedLinkDashboard.Controllers
{
    [Authorize]
    public class SpecialitiesController : Controller
    {
        private readonly ISpecialityRepository _specialityRepository;

        public SpecialitiesController(ISpecialityRepository specialityRepository)
        {
            _specialityRepository = specialityRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var data = _specialityRepository
                .GetAll()
                .OrderBy(o => o.Id)
                .ToList();

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SpecialityVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var speciality = new Speciality()
            {
                Name = vm.Name
            };

            _specialityRepository.Add(speciality);
            _specialityRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = _specialityRepository.GetById(id);

            if (data is not null)
            {
                var specialityVM = new SpecialityVM()
                {
                    Id = data.Id,
                    Name = data.Name
                };

                return View(specialityVM);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(SpecialityVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var speciality = _specialityRepository.GetById(vm.Id);

            if (speciality is null)
                return NotFound();

            speciality.Name = vm.Name;

            _specialityRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = _specialityRepository.GetById(id);

            if (data is null)
                return NotFound();

            _specialityRepository.Delete(data);
            _specialityRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CheckUnique(int id, string name)
        {
            var isExist = _specialityRepository.CheckUnique(id, name);

            return Json(!isExist);
        }

    }
}
