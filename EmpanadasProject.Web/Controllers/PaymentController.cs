using EmpanadasProject.Data.Models;
using EmpanadasProject.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmpanadasProject.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _service;

        public PaymentController()
        {
            _service = new PaymentService();
        }

        [HttpPost("pay")]
        public IActionResult Pay(decimal amount, PaymentMethod method)
        {
            var result = _service.Create(amount, method);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }
    }
}