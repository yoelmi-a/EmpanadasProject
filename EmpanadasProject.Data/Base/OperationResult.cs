namespace EmpanadasProject.Data.Base
{
    /// <summary>
    /// Representa el resultado de una operación que retorna un valor.
    /// </summary>
    /// <typeparam name="T">Tipo del valor retornado.</typeparam>
    public class OperationResult<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool EsExitoso { get; private set; }

        /// <summary>
        /// Valor retornado en caso de éxito.
        /// </summary>
        public T? Valor { get; private set; }

        /// <summary>
        /// Mensaje de error en caso de fallo.
        /// </summary>
        public string? MensajeError { get; private set; }

        /// <summary>
        /// Crea un resultado exitoso con el valor especificado.
        /// </summary>
        public static OperationResult<T> Exitoso(T valor) =>
            new() { EsExitoso = true, Valor = valor };

        /// <summary>
        /// Crea un resultado fallido con el mensaje de error especificado.
        /// </summary>
        public static OperationResult<T> Fallido(string mensajeError) =>
            new() { EsExitoso = false, MensajeError = mensajeError };
    }

    /// <summary>
    /// Representa el resultado de una operación sin valor de retorno.
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool EsExitoso { get; private set; }

        /// <summary>
        /// Mensaje de error en caso de fallo.
        /// </summary>
        public string? MensajeError { get; private set; }

        /// <summary>
        /// Crea un resultado exitoso.
        /// </summary>
        public static OperationResult Exitoso() => new() { EsExitoso = true };

        /// <summary>
        /// Crea un resultado fallido con el mensaje de error especificado.
        /// </summary>
        public static OperationResult Fallido(string mensajeError) =>
            new() { EsExitoso = false, MensajeError = mensajeError };
    }
}
