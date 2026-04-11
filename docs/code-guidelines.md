# Code Guidelines — D' Méndez Empanadas

## 1. Introduction

This document defines the coding standards, naming conventions, and design principles that every contributor must follow when working on the D' Méndez Empanadas platform (.NET 10 / C#). Adherence to these guidelines is enforced during code reviews and through automated tooling (Roslyn analyzers, EditorConfig).

> **Language rule:** All source code — identifiers (classes, methods, properties, variables), XML documentation comments, and inline comments — must be written in **Spanish**. Markdown documentation files (including this one) are written in English.

**Core principles:**

| Principle | Summary |
|-----------|---------|
| **SOLID** | Five OO design principles for maintainable, extensible code. |
| **KISS** | *Keep It Simple* — prefer the simplest solution that works. |
| **DRY** | *Don't Repeat Yourself* — every piece of knowledge has a single authoritative representation. |

---

## 2. SOLID Principles

### 2.1 Single Responsibility Principle (SRP)

Every class must have one, and only one, reason to change.

```csharp
// ❌ Incorrecto — el controlador mezcla lógica de negocio y acceso a datos
public class PedidoController : Controller
{
    public IActionResult Colocar(PedidoViewModel modelo)
    {
        var pedido = new Pedido { ... };
        _contexto.Pedidos.Add(pedido);
        _contexto.SaveChanges();
        return RedirectToAction("Confirmacion");
    }
}

// ✅ Correcto — el controlador delega al repositorio
public class PedidoController : Controller
{
    private readonly IRepositorioPedido _repositorioPedido;

    public PedidoController(IRepositorioPedido repositorioPedido)
        => _repositorioPedido = repositorioPedido;

    public async Task<IActionResult> Colocar(PedidoViewModel modelo)
    {
        var resultado = await _repositorioPedido.CrearAsync(modelo.AEntidad());
        if (!resultado.EsExitoso)
        {
            ModelState.AddModelError(string.Empty, resultado.MensajeError!);
            return View(modelo);
        }
        return RedirectToAction("Confirmacion", new { id = resultado.Valor });
    }
}
```

### 2.2 Open/Closed Principle (OCP)

Classes should be open for extension, closed for modification. Use interfaces to allow new behaviour without touching existing code.

```csharp
// ✅ Una nueva pasarela de pago = una nueva clase; el código existente no cambia
public interface IServicioPago
{
    Task<OperationResult<string>> ProcesarAsync(SolicitudPago solicitud);
}

public class ServicioPagoFalso : IServicioPago { ... }
// En el futuro, sin tocar el código existente:
// public class ServicioStripe : IServicioPago { ... }
```

### 2.3 Liskov Substitution Principle (LSP)

Derived types must be substitutable for their base types without breaking behaviour.

- Never override a method only to throw `NotImplementedException`.
- Do not weaken preconditions or strengthen postconditions in overrides.

### 2.4 Interface Segregation Principle (ISP)

Prefer small, focused interfaces over large general-purpose ones.

```csharp
// ❌ Incorrecto — interfaz demasiado amplia
public interface IRepositorio<T>
{
    T ObtenerPorId(int id);
    IEnumerable<T> ObtenerTodos();
    void Agregar(T entidad);
    void Actualizar(T entidad);
    void Eliminar(int id);
    IEnumerable<T> BuscarPorEstado(string estado); // no todas las entidades tienen estado
}

// ✅ Correcto — interfaces segregadas
public interface IRepositorioLectura<T>  { Task<OperationResult<T>> ObtenerPorIdAsync(int id); }
public interface IRepositorioEscritura<T> { Task<OperationResult<int>> AgregarAsync(T entidad); }
```

### 2.5 Dependency Inversion Principle (DIP)

High-level modules must not depend on low-level modules; both must depend on abstractions.

```csharp
// ❌ Incorrecto — el controlador instancia directamente la implementación concreta
public class ProductoController : Controller
{
    private readonly RepositorioProducto _repo = new RepositorioProducto();
}

// ✅ Correcto — depende de la abstracción; el contenedor DI inyecta la implementación
public class ProductoController : Controller
{
    private readonly IRepositorioProducto _repositorioProducto;
    public ProductoController(IRepositorioProducto repositorioProducto)
        => _repositorioProducto = repositorioProducto;
}
```

---

## 3. KISS — Keep It Simple

- Implement only what the current requirement demands.
- Avoid speculative generalization.
- Avoid deep inheritance hierarchies; prefer composition.
- Avoid clever one-liners that sacrifice readability.

```csharp
// ❌ Demasiado elaborado
var total = items.Aggregate(0m, (acc, item) =>
    acc + item.Cantidad * item.Precio * (1 + ObtenerTasaImpuesto(item.Categoria)));

// ✅ Claro y legible
decimal total = 0;
foreach (var item in items)
{
    decimal tasaImpuesto = ObtenerTasaImpuesto(item.Categoria);
    total += item.Cantidad * item.Precio * (1 + tasaImpuesto);
}
```

---

## 4. DRY — Don't Repeat Yourself

- Extract repeated logic into shared methods, extension methods, or base classes.
- Centralize configuration values; never duplicate magic strings or numbers.
- Use constants or enumerations instead of repeated literal values.

```csharp
// ❌ Incorrecto — cadena repetida en todo el código
if (pedido.Estado == "Recibido") { ... }
if (pedido.Estado == "Recibido") { ... }

// ✅ Correcto — definición única
/// <summary>Define los posibles estados de un pedido.</summary>
public static class EstadoPedido
{
    public const string Recibido      = "Recibido";
    public const string EnPreparacion = "EnPreparacion";
    public const string EnCamino      = "EnCamino";
    public const string Entregado     = "Entregado";
    public const string Cancelado     = "Cancelado";
}

if (pedido.Estado == EstadoPedido.Recibido) { ... }
```

---

## 5. Operation Result Pattern

**Every service and repository method that can succeed or fail for an expected business reason must return `OperationResult<T>` or `OperationResult`.**

### 5.1 Definition

```csharp
/// <summary>Representa el resultado de una operación que retorna un valor.</summary>
public class OperationResult<T>
{
    /// <summary>Indica si la operación fue exitosa.</summary>
    public bool EsExitoso { get; private set; }

    /// <summary>Valor retornado en caso de éxito.</summary>
    public T? Valor { get; private set; }

    /// <summary>Mensaje de error en caso de fallo.</summary>
    public string? MensajeError { get; private set; }

    /// <summary>Crea un resultado exitoso con el valor especificado.</summary>
    public static OperationResult<T> Exitoso(T valor) =>
        new() { EsExitoso = true, Valor = valor };

    /// <summary>Crea un resultado fallido con el mensaje de error especificado.</summary>
    public static OperationResult<T> Fallido(string mensajeError) =>
        new() { EsExitoso = false, MensajeError = mensajeError };
}

/// <summary>Representa el resultado de una operación sin valor de retorno.</summary>
public class OperationResult
{
    /// <summary>Indica si la operación fue exitosa.</summary>
    public bool EsExitoso { get; private set; }

    /// <summary>Mensaje de error en caso de fallo.</summary>
    public string? MensajeError { get; private set; }

    /// <summary>Crea un resultado exitoso.</summary>
    public static OperationResult Exitoso() => new() { EsExitoso = true };

    /// <summary>Crea un resultado fallido con el mensaje de error especificado.</summary>
    public static OperationResult Fallido(string mensajeError) =>
        new() { EsExitoso = false, MensajeError = mensajeError };
}
```

### 5.2 Usage in Repository

```csharp
/// <summary>Repositorio de productos.</summary>
public class RepositorioProducto : IRepositorioProducto
{
    private readonly AppDbContext _contexto;

    public RepositorioProducto(AppDbContext contexto) => _contexto = contexto;

    /// <summary>Obtiene un producto por su identificador.</summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>Resultado de la operación con el producto encontrado.</returns>
    public async Task<OperationResult<Producto>> ObtenerPorIdAsync(int id)
    {
        var producto = await _contexto.Productos.FindAsync(id);
        return producto is null
            ? OperationResult<Producto>.Fallido($"Producto con Id {id} no encontrado.")
            : OperationResult<Producto>.Exitoso(producto);
    }
}
```

### 5.3 Usage in Controller

```csharp
public async Task<IActionResult> Detalle(int id)
{
    var resultado = await _repositorioProducto.ObtenerPorIdAsync(id);
    if (!resultado.EsExitoso)
        return NotFound(resultado.MensajeError);

    return View(resultado.Valor!.AViewModel());
}
```

### 5.4 When NOT to use OperationResult

Unexpected exceptions (database connectivity failures, null references from programming errors) must **not** be swallowed into `OperationResult`. Let them propagate and be caught by the global error-handling middleware.

---

## 6. C# Naming Conventions (Spanish)

All identifiers must be written in Spanish.

### 6.1 General Rules

| Element | Convention | Example |
|---------|-----------|---------|
| Namespace | PascalCase | `DMendez.Web.Controladores` |
| Class | PascalCase | `ControladorPedido` |
| Interface | `I` + PascalCase | `IRepositorioPedido` |
| Abstract class | PascalCase | `EntidadBase` |
| Enum | PascalCase (type and values) | `EstadoPedido.Entregado` |
| Record | PascalCase | `ResultadoPago` |
| Method | PascalCase | `ObtenerPorIdAsync` |
| Property | PascalCase | `CreadoEn` |
| Private field | `_camelCase` | `_repositorioPedido` |
| Local variable | camelCase | `totalPedido` |
| Parameter | camelCase | `idPedido` |
| Constant | PascalCase | `MaximosIntentos` |
| Generic type parameter | `T` or `T` + descriptor | `TEntidad`, `TResultado` |

### 6.2 Async Methods

All asynchronous methods must be suffixed with `Async`.

```csharp
// ✅
Task<OperationResult<Pedido>> ObtenerPorIdAsync(int id);
Task<OperationResult<int>> CrearAsync(Pedido pedido);
```

### 6.3 Boolean Members

Use affirmative prefixes: `Es`, `Tiene`, `Puede`, `Debe`.

```csharp
bool EsDisponible { get; set; }
bool TienePromocionActiva { get; }
bool PuedeCancelarse => Estado == EstadoPedido.Recibido;
```

### 6.4 Collections

Use plural names for collection-typed members.

```csharp
IReadOnlyList<Producto> Productos { get; }
IEnumerable<Pedido> ObtenerPedidosDelClienteAsync(string idCliente);
```

---

## 7. Code Organization

### 7.1 One Type Per File

One class, interface, or enum per file. The file name must match the type name.

```
RepositorioProducto.cs     → public class RepositorioProducto
IRepositorioProducto.cs    → public interface IRepositorioProducto
EstadoPedido.cs            → public static class EstadoPedido
```

### 7.2 Member Ordering Within a Class

1. Constants and static readonly fields
2. Private fields
3. Constructors
4. Public properties
5. Public methods
6. Private / protected methods

### 7.3 No Regions

Do **not** use `#region` directives. If a class needs regions, it violates SRP and must be refactored.

---

## 8. .NET 10 / ASP.NET Core Conventions

### 8.1 Dependency Injection Registration

```csharp
// Extensions/ServiciosAplicacion.cs
/// <summary>Registra los servicios de la aplicación en el contenedor DI.</summary>
public static class ServiciosAplicacion
{
    public static IServiceCollection AgregarServiciosAplicacion(
        this IServiceCollection servicios)
    {
        servicios.AddScoped<IRepositorioProducto, RepositorioProducto>();
        servicios.AddScoped<IRepositorioPedido, RepositorioPedido>();
        servicios.AddScoped<IRepositorioPromocion, RepositorioPromocion>();
        servicios.AddScoped<IServicioPago, ServicioPagoFalso>();
        return servicios;
    }
}
```

### 8.2 Controllers

- Controllers must be thin: no business logic, no direct EF Core calls.
- Action methods must be `async` for all I/O.
- Every POST action must carry `[ValidateAntiForgeryToken]`.
- Input validation uses Data Annotations on ViewModels.
- Controllers inspect `OperationResult` and render appropriate views.

### 8.3 Entity Framework Core (InMemory)

```csharp
// Program.cs
builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseInMemoryDatabase("DMendezDb"));
```

- Always use `async` EF Core methods.
- Never expose `IQueryable<T>` outside a repository.
- Use explicit `Include` / `ThenInclude` for related data.
- All entity configurations go in `IEntityTypeConfiguration<T>` classes, applied via `ApplyConfigurationsFromAssembly`.

### 8.4 Identity Configuration

```csharp
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
{
    opciones.Password.RequiredLength = 8;
    opciones.Password.RequireUppercase = true;
    opciones.Password.RequireDigit = true;
    opciones.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>();
```

### 8.5 Configuration Binding (Options Pattern)

```csharp
/// <summary>Opciones del administrador por defecto.</summary>
public class OpcionesAdminPorDefecto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// Program.cs
builder.Services.Configure<OpcionesAdminPorDefecto>(
    builder.Configuration.GetSection("DefaultAdmin"));
```

---

## 9. Default Admin Seed

The seed class must follow these rules:

1. Injected dependencies: `UserManager<IdentityUser>`, `RoleManager<IdentityRole>`, `IOptions<OpcionesAdminPorDefecto>`.
2. The seed is **idempotent**: check before creating.
3. Return `OperationResult` to signal success or failure.
4. Called once from `Program.cs` after `app.Build()`.

```csharp
/// <summary>Inicializa el rol y usuario administrador por defecto.</summary>
public class SemillaAdminPorDefecto
{
    private readonly UserManager<IdentityUser> _gestorUsuarios;
    private readonly RoleManager<IdentityRole> _gestorRoles;
    private readonly OpcionesAdminPorDefecto _opciones;

    public SemillaAdminPorDefecto(
        UserManager<IdentityUser> gestorUsuarios,
        RoleManager<IdentityRole> gestorRoles,
        IOptions<OpcionesAdminPorDefecto> opciones)
    {
        _gestorUsuarios = gestorUsuarios;
        _gestorRoles    = gestorRoles;
        _opciones       = opciones.Value;
    }

    /// <summary>Ejecuta la siembra de manera idempotente.</summary>
    public async Task<OperationResult> EjecutarAsync()
    {
        const string nombreRol = "Administrador";

        if (!await _gestorRoles.RoleExistsAsync(nombreRol))
            await _gestorRoles.CreateAsync(new IdentityRole(nombreRol));

        if (await _gestorUsuarios.FindByEmailAsync(_opciones.Email) is not null)
            return OperationResult.Exitoso(); // Ya existe; no hacer nada

        var admin = new IdentityUser
        {
            UserName = _opciones.Email,
            Email    = _opciones.Email,
            EmailConfirmed = true
        };

        var resultadoCreacion = await _gestorUsuarios.CreateAsync(admin, _opciones.Password);
        if (!resultadoCreacion.Succeeded)
        {
            var errores = string.Join(", ", resultadoCreacion.Errors.Select(e => e.Description));
            return OperationResult.Fallido($"No se pudo crear el administrador: {errores}");
        }

        await _gestorUsuarios.AddToRoleAsync(admin, nombreRol);
        return OperationResult.Exitoso();
    }
}
```

---

## 10. Exception Handling

- Use a global exception-handling middleware for unexpected errors.
- Only catch exceptions you can meaningfully handle at that layer.
- Empty `catch` blocks are **forbidden**.
- `OperationResult.Fallido(...)` is for expected business failures; exceptions are for unexpected runtime errors.

```csharp
/// <summary>Middleware global de manejo de excepciones no controladas.</summary>
public class MiddlewareExcepcionGlobal
{
    private readonly RequestDelegate _siguiente;
    private readonly ILogger<MiddlewareExcepcionGlobal> _logger;

    public MiddlewareExcepcionGlobal(
        RequestDelegate siguiente,
        ILogger<MiddlewareExcepcionGlobal> logger)
    {
        _siguiente = siguiente;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _siguiente(contexto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado en la solicitud {Ruta}",
                contexto.Request.Path);
            contexto.Response.Redirect("/Error");
        }
    }
}
```

---

## 11. XML Documentation

All `public` and `internal` types and members must have XML documentation in Spanish.

```csharp
/// <summary>
/// Crea un nuevo pedido para el cliente especificado.
/// </summary>
/// <param name="idCliente">Identificador único del cliente que realiza el pedido.</param>
/// <param name="carrito">Carrito de compras con los productos a pedir.</param>
/// <returns>
/// Un <see cref="OperationResult{T}"/> con el identificador del pedido creado,
/// o el mensaje de error si la operación falla.
/// </returns>
public async Task<OperationResult<int>> CrearPedidoAsync(
    string idCliente, Carrito carrito) { ... }
```

---

## 12. Unit Testing Standards

### 12.1 Coverage Requirement

**Every method** must have at least:
- One test for the **success scenario**.
- One test for the **failure scenario** (or each distinct failure path if there are multiple).

### 12.2 InMemory Database Usage

All tests that require data access must use the EF Core **InMemory provider**. No real or containerized database is ever used in tests.

```csharp
// Use a unique database name per test to ensure full isolation
await using var contexto = AyudanteDbEnMemoria.CrearContexto();
```

### 12.3 Test Structure

- File naming: `<ClassUnderTest>Pruebas.cs` → e.g., `RepositorioProductoPruebas.cs`.
- Method naming: `NombreMetodo_EstadoBajoPrueba_ComportamientoEsperado`.
- Use `[Fact]` for single-case tests and `[Theory]` + `[InlineData]` for parameterized tests.
- Use **Moq** for mocking interfaces (e.g., `IServicioPago`) in controller tests.

```csharp
public class ServicioPedidoPruebas
{
    // ✅ Prueba de escenario exitoso
    [Fact]
    public async Task CrearPedidoAsync_ConCarritoValido_RetornaExito()
    {
        // Arrange
        await using var contexto = AyudanteDbEnMemoria.CrearContexto();
        var mockPago = new Mock<IServicioPago>();
        mockPago.Setup(s => s.ProcesarAsync(It.IsAny<SolicitudPago>()))
                .ReturnsAsync(OperationResult<string>.Exitoso("ref-1234"));

        var servicio = new ServicioPedido(contexto, mockPago.Object);
        var carrito  = new Carrito { Items = [new ItemCarrito { IdProducto = 1, Cantidad = 2 }] };

        // Act
        var resultado = await servicio.CrearPedidoAsync("usuario-1", carrito);

        // Assert
        Assert.True(resultado.EsExitoso);
        Assert.True(resultado.Valor > 0);
    }

    // ✅ Prueba de escenario fallido
    [Fact]
    public async Task CrearPedidoAsync_ConCarritoVacio_RetornaFallo()
    {
        // Arrange
        await using var contexto = AyudanteDbEnMemoria.CrearContexto();
        var mockPago = new Mock<IServicioPago>();
        var servicio = new ServicioPedido(contexto, mockPago.Object);
        var carritoVacio = new Carrito { Items = [] };

        // Act
        var resultado = await servicio.CrearPedidoAsync("usuario-1", carritoVacio);

        // Assert
        Assert.False(resultado.EsExitoso);
        Assert.NotNull(resultado.MensajeError);
    }
}
```

### 12.4 Testing the Fake Payment Service

```csharp
public class ServicioPagoFalsoPruebas
{
    [Fact]
    public async Task ProcesarAsync_SinSimularFallo_RetornaReferenciaExitosa()
    {
        // Arrange
        var servicio  = new ServicioPagoFalso();
        var solicitud = new SolicitudPago { SimularFallo = false };

        // Act
        var resultado = await servicio.ProcesarAsync(solicitud);

        // Assert
        Assert.True(resultado.EsExitoso);
        Assert.False(string.IsNullOrEmpty(resultado.Valor));
    }

    [Fact]
    public async Task ProcesarAsync_SimulandoFallo_RetornaFallo()
    {
        // Arrange
        var servicio  = new ServicioPagoFalso();
        var solicitud = new SolicitudPago { SimularFallo = true };

        // Act
        var resultado = await servicio.ProcesarAsync(solicitud);

        // Assert
        Assert.False(resultado.EsExitoso);
        Assert.NotNull(resultado.MensajeError);
    }
}
```

### 12.5 Testing the Admin Seed

```csharp
public class SemillaAdminPorDefectoPruebas
{
    [Fact]
    public async Task EjecutarAsync_CuandoAdminNoExiste_CreaAdminYRol()
    {
        // Arrange — use in-memory Identity stores via test helpers
        // ... (setup UserManager and RoleManager with InMemory stores)

        // Act
        var resultado = await semilla.EjecutarAsync();

        // Assert
        Assert.True(resultado.EsExitoso);
        // verify role and user exist
    }

    [Fact]
    public async Task EjecutarAsync_CuandoAdminYaExiste_NoCreaDuplicado()
    {
        // Arrange — seed once, then call again
        // Act — second call
        // Assert — still only one admin user
    }
}
```

---

## 13. Git & Pull Request Standards

- Commit messages use **Conventional Commits** format in Spanish:
  `tipo(ámbito): descripción`
  Examples: `feat(pedidos): agregar endpoint de cancelación`, `fix(carrito): recalcular impuesto al eliminar ítem`
- Every pull request must:
  - Reference at least one requirement or issue ID.
  - Pass all CI checks (build + all tests).
  - Be reviewed and approved by at least one other developer.
- Branch naming (GitFlow): `main`, `develop`, `feature/*`, `fix/*`, `release/*`.
- Direct commits to `main` are **forbidden**.

---

## 14. Tooling & Enforcement

| Tool | Purpose |
|------|---------|
| `.editorconfig` | Enforce indentation (4 spaces), charset (UTF-8), and whitespace rules. |
| Roslyn Analyzers | Static analysis; naming and code-quality rules at compile time. |
| `dotnet format` | Auto-format enforcement in CI. |
| GitHub Actions | CI pipeline: build → test → format check → publish coverage report. |
| Moq | Mocking library for unit tests. |
