using EmpanadasProject.Data.Interfaces.Usuario;
using EmpanadasProject.Data.Base;
using Microsoft.AspNetCore.Identity;
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

            if (!result.EsExitoso) 
            {
                ViewBag.ErrorMessage = result.MensajeError;
                return View(); 
            }

            return View(result.Valor);
        }

        // GET: RolUsuarioController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var result = await _service.GetRolUsuarioByIdAsync(id);

            if (!result.EsExitoso) 
            {
                ViewBag.ErrorMessage = result.MensajeError;
                return View();
            }

            return View(result.Valor);
        }

        // GET: RolUsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolUsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IdentityRole rolUsuario)
        {
            try
            {
                var result = await _service.AddRolUsuarioAsync(rolUsuario);

                if (!result.EsExitoso) 
                {
                    ViewBag.ErrorMessage = result.MensajeError;
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
        public async Task<ActionResult> Edit(string id)
        {
            var result = await _service.GetRolUsuarioByIdAsync(id);

            if (!result.EsExitoso) 
            {
                ViewBag.ErrorMessage = result.MensajeError;
                return View();
            }

            return View(result.Valor);
        }

        // POST: RolUsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IdentityRole rolUsuario)
        {
            try
            {
                var result = await _service.UpdateRolUsuarioAsync(rolUsuario);

                if (!result.EsExitoso) 
                {
                    ViewBag.ErrorMessage = result.MensajeError;
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
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _service.GetRolUsuarioByIdAsync(id);

            if (!result.EsExitoso) 
            {
                ViewBag.ErrorMessage = result.MensajeError;
                return View();
            }

            return View(result.Valor);
        }

        // POST: RolUsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmarEliminar(string id)
        {
            try
            {
                var result = await _service.DeleteRolUsuarioAsync(id);

                if (!result.EsExitoso) 
                {
                    ViewBag.ErrorMessage = result.MensajeError;
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
