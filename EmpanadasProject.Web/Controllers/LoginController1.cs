using EmpanadasProject.Data.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EmpanadasProject.Web.Controllers
{
    public class LoginController1 : Controller
    {

        private static List<UsuarioDTO> _usuarios = new List<UsuarioDTO>
        {
            new UsuarioDTO { Email = "admin", Password = "1234" },
            new UsuarioDTO { Email = "user", Password = "pass" }
        };

        [HttpGet]
        public IActionResult Login(string usuarios, string contraseña)
        {
            var encontrado = _usuarios.Any(u => u.Email == usuarios && u.Password == contraseña);

            if (encontrado)
                return Content("Usuario Logueado Satisfactoriamente");

            return Content("Credenciales incorrectas");
        }

        [HttpPost]
        public IActionResult Register(UsuarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos");

            _usuarios.Add(dto);

            return Content("Usuario registrado (hardcode)");
        }
    }
}
