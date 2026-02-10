using EmpanadasProject.Data.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EmpanadasProject.Web.Controllers
{
    public class LoginController1 : Controller
    {

        private static List<LoginDto> _usuarios = new List<LoginDto>
        {
            new LoginDto { Usuarios = "admin", Contraseña = "1234" },
            new LoginDto { Usuarios = "user", Contraseña = "pass" }
        };

        [HttpGet]
        public IActionResult Login(string usuarios, string contraseña)
        {
            var encontrado = _usuarios.Any(u => u.Usuarios == usuarios && u.Contraseña == contraseña);

            if (encontrado)
                return Content("Usuario Logueado Satisfactoriamente");

            return Content("Credenciales incorrectas");
        }

        [HttpPost]
        public IActionResult Register(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos");

            _usuarios.Add(dto);

            return Content("Usuario registrado (hardcode)");
        }
    }
}
