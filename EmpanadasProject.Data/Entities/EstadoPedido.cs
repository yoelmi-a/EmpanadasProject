namespace EmpanadasProject.Data.Entities
{
    /// <summary>
    /// Define los posibles estados de un pedido.
    /// </summary>
    public enum EstadoPedido
    {
        Recibido,
        EnPreparacion,
        EnCamino,
        Entregado,
        Cancelado
    }
}
