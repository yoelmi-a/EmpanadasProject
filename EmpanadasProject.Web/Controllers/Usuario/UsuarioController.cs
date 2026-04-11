using EmpanadasProject.Data.Interfaces.Usuario;
using EmpanadasProject.Data.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmpanadasProject.Web.Controllers.Usuario
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        // GET: UsuarioController
        public async Task<ActionResult> Index()
        {
            var result = await _service.GetAllUsuariosAsync();

            if (!result.EsExitoso)
            {
                ViewBag.ErrorMessage = result.MensajeError;
                return View();
            }

            return View(result.Valor);
        }

        // GET: UsuarioController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var result = await _service.GetUsuarioByIdAsync(id);

            if (!result.EsExitoso)
            {
                ViewBag.ErrorMessage = result.MensajeError;
                return View();
            }

            return View(result.Valor);
        }

        // GET: UsuarioController/UsuarioByEmail/5
        public async Task<ActionResult> UsuarioByEmail(string email)
        {
            var result = await _service.GetUsuariosByEmailAsync(email);
            if (!result.EsExitoso)
            {
                ViewBag.ErrorMessage = result.MensajeError;
                return View();
            }
            return View(result.Valor);
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IdentityUser usuario, string password)
        {
            try
            {
                var result = await _service.AddUsuarioAsync(usuario, password);

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

        // GET: UsuarioController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var result = await _service.GetUsuarioByIdAsync(id);

            if (!result.EsExitoso)
            {
                ViewBag.ErrorMessage = result.MensajeError;
                return View();
            }

            return View(result.Valor);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IdentityUser usuario)
        {
            try
            {
                var result = await _service.UpdateUsuarioAsync(usuario);

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

        // GET: UsuarioController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _service.GetUsuarioByIdAsync(id);

            if (!result.EsExitoso)
            {
                ViewBag.ErrorMessage = result.MensajeError;
                return View();
            }

            return View(result.Valor);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmarEliminar(string id)
        {
            try
            {
                var result = await _service.DeleteUsuarioAsync(id);

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
