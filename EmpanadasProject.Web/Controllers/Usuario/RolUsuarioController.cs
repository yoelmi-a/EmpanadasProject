using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.Interfaces.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace EmpanadasProject.Web.Controllers.Usuario
{
    public class RolUsuarioController : Controller
    {
        private readonly IRolUsuarioService _service;

        public RolUsuarioController(IRolUsuarioService service)
        {
            _service = service;
        }

        // GET: RolUsuarioController
        public async Task<ActionResult> Index()
        {
            var result = await _service.GetAllRolUsuariosAsync();

            if (!result.Success) 
            {
                ViewBag.ErrorMessage = result.Message;
                return View(); 
            }

            return View();
        }

        // GET: RolUsuarioController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var result = await _service.GetRolUsuarioByIdAsync(id);

            if (!result.Success) 
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View();
        }

        // GET: RolUsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolUsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RolUsuario rolUsuario)
        {
            try
            {
                var result = await _service.AddRolUsuarioAsync(rolUsuario);

                if (!result.Success) 
                {
                    ViewBag.ErrorMessage = result.Message;
                    return View(rolUsuario);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RolUsuarioController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var result = await _service.GetRolUsuarioByIdAsync(id);

            if (!result.Success) 
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View(result.Data);
        }

        // POST: RolUsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RolUsuario rolUsuario)
        {
            try
            {
                var result = await _service.UpdateRolUsuarioAsync(rolUsuario);

                if (!result.Success) 
                {
                    ViewBag.ErrorMessage = result.Message;
                    return View(rolUsuario);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RolUsuarioController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.GetRolUsuarioByIdAsync(id);

            if (!result.Success) 
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View(result.Data);
        }

        // POST: RolUsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Desactivar(int id)
        {
            try
            {
                var result = await _service.DeleteRolUsuarioAsync(id);

                if (!result.Success) 
                {
                    ViewBag.ErrorMessage = result.Message;
                    return View();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
