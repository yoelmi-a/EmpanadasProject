namespace EmpanadasProject.Data.Entities.Usuario
{
    public class Usuarios : BaseEntity.BaseEntity
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public string Telefono { get; set; }
        public int RolId { get; set; }
        public DateTime FechaDeRegistro { get; set; }
    }
}
 