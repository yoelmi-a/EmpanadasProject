namespace EmpanadasProject.Data.Entities
{
    /// <summary>
    /// Representa un ítem dentro de un pedido.
    /// </summary>
    public class ItemPedido : EntidadBase
    {
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal TotalItem => PrecioUnitario * Cantidad;
    }
}
