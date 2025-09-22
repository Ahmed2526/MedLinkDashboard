using MedLinkDashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedLinkDashboard.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var data = _roleManager.Roles.Select(e => new RoleVM()
            {
                Id = e.Id,
                Name = e.Name!
            }).ToList();

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleVM roleVM)
        {
            if (!ModelState.IsValid)
                return View(roleVM);

            IdentityRole role = new()
            {
                Name = roleVM.Name
            };

            var data = await _roleManager.CreateAsync(role);

            if (!data.Succeeded)
            {
                foreach (var item in data.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(roleVM);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            RoleVM vM = new RoleVM()
            {
                Id = role.Id,
                Name = role.Name!
            };

            return View(vM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoleVM vM)
        {
            if (!ModelState.IsValid)
                return View(vM);

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

            role.Name = vM.Name;

            var data = await _roleManager.UpdateAsync(role);

            if (!data.Succeeded)
            {
                foreach (var item in data.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

                return View(vM);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));
        }

    }
}
