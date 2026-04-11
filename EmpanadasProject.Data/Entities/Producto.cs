namespace EmpanadasProject.Data.Entities
{
    /// <summary>
    /// Representa un producto en el sistema.
    /// </summary>
    public class Producto : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool EsDisponible { get; set; } = true;
        public string Categoria { get; set; } = string.Empty; // Empanadas, Bebidas, Combos
    }
}
