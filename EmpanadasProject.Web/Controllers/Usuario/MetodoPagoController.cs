using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.Interfaces.Usuario;
using Microsoft.AspNetCore.Mvc;


namespace EmpanadasProject.Web.Controllers.Usuario
{
    public class MetodoPagoController : Controller
    {
        private readonly IMetodoPagoService _service;

        public MetodoPagoController(IMetodoPagoService service)
        {
            _service = service;
        }

        // GET: MetodoPagoController
        public async Task<ActionResult> Index()
        {
            var result = await _service.GetAllMetodoPago();

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View(result.Data);
        }

        // GET: MetodoPagoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var result = await _service.GetMetodoPagoByIdAsync(id);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View(result.Data);
        }

        // GET: MetodoPagoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MetodoPagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MetodoPago metodoPago)
        {
            try
            {
                var result = await _service.AddMetodoPagoAsync(metodoPago);

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

        // GET: MetodoPagoController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var result = await _service.GetMetodoPagoByIdAsync(id);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View(result.Data);
        }

        // POST: MetodoPagoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MetodoPago metodoPago)
        {
            try
            {
                var result = await _service.UpdateMetodoPago(metodoPago);

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

        // GET: MetodoPagoController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.GetMetodoPagoByIdAsync(id);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }

            return View();
        }

        // POST: MetodoPagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Desactivar(int id)
        {
            try
            {
                var result = await _service.DeleteMetodoPago(id);

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
