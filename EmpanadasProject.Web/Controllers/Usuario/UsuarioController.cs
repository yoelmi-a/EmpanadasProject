using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.Interfaces.Usuario;

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

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View(result.Data);
        }

        // GET: UsuarioController/Details/5
        public async Task<ActionResult> Details(int id)
        {

            var result = await _service.GetUsuarioByIdAsync(id);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View(result.Data);
        }

        // GET: UsuarioController/UsuarioByEmail/5
        public async Task<ActionResult> UsuarioByEmail(string email)
        {
            var result = await _service.GetUsuariosByEmailAsync(email);
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Usuarios usuario)
        {
            try
            {
                var result = await _service.AddUsuarioAsync(usuario);

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

        // GET: UsuarioController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            var result = await _service.GetUsuarioByIdAsync(id);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View(result.Data);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Usuarios usuario)
        {
            try
            {

                var result = await _service.UpdateUsuarioAsync(usuario);

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

        // GET: UsuarioController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.GetUsuarioByIdAsync(id);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View(result.Data);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Desactivar(int id)
        {
            try
            {
                var result = await _service.DeleteUsuarioAsync(id);

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

        // GET: Usuario/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Usuarios usuarios)
        {
            var result = await _service.Login(usuarios);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
