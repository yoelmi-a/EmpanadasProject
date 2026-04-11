# Architecture — D' Méndez Empanadas

## 1. Overview

The D' Méndez Empanadas platform is a **web-only** application structured as a **3-layer .NET 10 solution**. Each layer has a single, well-defined responsibility and communicates only with adjacent layers.

```
┌──────────────────────────────────────────┐
│            DMendez.Web                   │  ← ASP.NET Core 10 MVC
│  Controllers · Views · ViewModels        │
│  Filters · Middleware · Extensions       │
│  Identity (cookie auth) · Seed           │
└──────────────┬───────────────────────────┘
               │ depends on
┌──────────────▼───────────────────────────┐
│            DMendez.Data                  │  ← EF Core 10 — InMemory Provider
│  AppDbContext · Entities · Repositories  │
│  Configurations · Migrations             │
│  OperationResult · IPaymentService       │
│  FakePaymentService                      │
└──────────────────────────────────────────┘

┌──────────────────────────────────────────┐
│            DMendez.Tests                 │  ← xUnit
│  Unit/ · Integration/                    │
│  InMemory DB for all data tests          │
└──────────────────────────────────────────┘
```

> **No mobile application, no JWT, no OAuth 2.0, no real payment gateway, no external database server.**

---

## 2. Solution Structure

```
DMendezEmpanadas.sln
├── src/
│   ├── DMendez.Web/                        # ASP.NET Core 10 MVC
│   │   ├── Controllers/
│   │   │   ├── AccountController.cs        # Registration, login, logout
│   │   │   ├── CatalogController.cs        # Product browsing
│   │   │   ├── CartController.cs           # Shopping cart
│   │   │   ├── OrderController.cs          # Checkout, order status, history
│   │   │   └── Admin/
│   │   │       ├── ProductController.cs    # Admin CRUD products
│   │   │       ├── OrderController.cs      # Admin manage orders
│   │   │       ├── PromotionController.cs  # Admin manage promotions
│   │   │       └── ReportController.cs     # Admin sales reports
│   │   ├── Views/
│   │   ├── ViewModels/
│   │   ├── Filters/
│   │   ├── Middleware/
│   │   ├── Extensions/                     # IServiceCollection helpers
│   │   ├── Seed/
│   │   │   └── DefaultAdminSeed.cs         # Seeds default admin on startup
│   │   ├── wwwroot/
│   │   └── Program.cs
│   │
│   └── DMendez.Data/                       # Data access + domain abstractions
│       ├── Common/
│       │   └── OperationResult.cs          # OperationResult<T> and OperationResult
│       ├── Context/
│       │   └── AppDbContext.cs             # EF Core DbContext (InMemory)
│       ├── Entities/                       # EF Core POCO entities
│       │   ├── BaseEntity.cs
│       │   ├── Producto.cs
│       │   ├── Pedido.cs
│       │   ├── ItemPedido.cs
│       │   ├── HistorialEstado.cs
│       │   └── Promocion.cs
│       ├── Repositories/
│       │   ├── Interfaces/
│       │   │   ├── IRepositorioProducto.cs
│       │   │   ├── IRepositorioPedido.cs
│       │   │   └── IRepositorioPromocion.cs
│       │   ├── RepositorioProducto.cs
│       │   ├── RepositorioPedido.cs
│       │   └── RepositorioPromocion.cs
│       ├── Services/
│       │   ├── Interfaces/
│       │   │   └── IServicioPago.cs        # Payment abstraction
│       │   └── ServicioPagoFalso.cs        # FakePaymentService implementation
│       └── Configurations/                 # IEntityTypeConfiguration<T>
│
└── tests/
    └── DMendez.Tests/                      # xUnit test project
        ├── Unit/
        │   ├── Repositories/
        │   ├── Services/
        │   └── Controllers/
        └── Helpers/
            └── InMemoryDbHelper.cs         # Factory for InMemory AppDbContext
```

---

## 3. Layer Descriptions

### 3.1 DMendez.Web — Presentation Layer

**Technology:** ASP.NET Core 10 MVC  
**Responsibility:** Handle HTTP requests, render Razor views, validate input via model binding, enforce authentication/authorization, and coordinate calls to the Data layer.

**Key rules:**
- Controllers must be **thin** — no business logic or direct EF Core usage inside a controller.
- All action methods that call service/repository code must handle `OperationResult` and render appropriate views or error messages.
- Every form must include `@Html.AntiForgeryToken()` and every POST action must carry `[ValidateAntiForgeryToken]`.
- Route access must be protected with `[Authorize]` or `[Authorize(Roles = "Administrador")]`.
- ViewModels must never expose EF Core entity types directly.

**Authentication — ASP.NET Core Identity:**

```csharp
// Program.cs (simplified)
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("DMendezDb"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Account/Login";
    opt.AccessDeniedPath = "/Account/AccessDenied";
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(
        builder.Configuration.GetValue<int>("Session:TimeoutMinutes"));
});
```

**Default Admin Seed:**

The `DefaultAdminSeed` class is called once during startup (`app.SeedDefaultAdminAsync()`). It is injected with `UserManager<IdentityUser>` and `RoleManager<IdentityRole>` and reads credentials from `appsettings.json`:

```json
"DefaultAdmin": {
  "Email": "admin@dmendez.com",
  "Password": "Admin@12345"
}
```

The seed is **idempotent**: if the `Administrador` role and admin user already exist, it returns immediately without making any changes.

---

### 3.2 DMendez.Data — Data Access Layer

**Technology:** Entity Framework Core 10 — InMemory Provider  
**Responsibility:** Define entities, configurations, repositories, and the payment service abstraction.

#### 3.2.1 OperationResult

`OperationResult<T>` is the single return type for all service and repository methods that can fail for expected business reasons.

```csharp
/// <summary>Representa el resultado de una operación que puede fallar.</summary>
public class OperationResult<T>
{
    public bool EsExitoso { get; private set; }
    public T? Valor { get; private set; }
    public string? MensajeError { get; private set; }

    public static OperationResult<T> Exitoso(T valor) =>
        new() { EsExitoso = true, Valor = valor };

    public static OperationResult<T> Fallido(string mensajeError) =>
        new() { EsExitoso = false, MensajeError = mensajeError };
}

/// <summary>Versión no genérica para operaciones sin valor de retorno.</summary>
public class OperationResult
{
    public bool EsExitoso { get; private set; }
    public string? MensajeError { get; private set; }

    public static OperationResult Exitoso() => new() { EsExitoso = true };
    public static OperationResult Fallido(string mensajeError) =>
        new() { EsExitoso = false, MensajeError = mensajeError };
}
```

#### 3.2.2 AppDbContext

```csharp
public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItemsPedido => Set<ItemPedido>();
    public DbSet<HistorialEstado> HistorialesEstado => Set<HistorialEstado>();
    public DbSet<Promocion> Promociones => Set<Promocion>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
```

#### 3.2.3 Base Entity

```csharp
/// <summary>Entidad base con campos de auditoría.</summary>
public abstract class EntidadBase
{
    public int Id { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }
}
```

#### 3.2.4 Payment Service Abstraction

```csharp
/// <summary>Abstracción del servicio de pago.</summary>
public interface IServicioPago
{
    Task<OperationResult<string>> ProcesarAsync(SolicitudPago solicitud);
}

/// <summary>Implementación simulada para propósitos educativos.</summary>
public class ServicioPagoFalso : IServicioPago
{
    public Task<OperationResult<string>> ProcesarAsync(SolicitudPago solicitud)
    {
        if (solicitud.SimularFallo)
            return Task.FromResult(
                OperationResult<string>.Fallido("Pago rechazado: simulación de fallo activa."));

        var referencia = Guid.NewGuid().ToString();
        return Task.FromResult(OperationResult<string>.Exitoso(referencia));
    }
}
```

**Key rules for the Data layer:**
- The `AppDbContext` must never be used directly in controllers; always access data through repository interfaces.
- Never expose `IQueryable<T>` outside a repository method.
- Always use `async` EF Core methods (`ToListAsync`, `SaveChangesAsync`, etc.).
- Use explicit `Include` / `ThenInclude` for related data; no lazy loading.
- All service/repository methods must return `OperationResult<T>` or `OperationResult` for expected failure paths.

---

### 3.3 DMendez.Tests — Test Layer

**Technology:** xUnit  
**Responsibility:** Verify the correctness of every method in the Web and Data layers through unit tests.

**Rules:**
- **Every method** must have at least one test for the **success scenario** and one test for the **failure scenario**.
- All tests use the **EF Core InMemory database**; no real or containerized database is used.
- External dependencies (e.g., `IServicioPago`) are replaced with **Moq** mocks in controller/service tests.
- Tests follow the **Arrange / Act / Assert** pattern.
- Test method names follow: `NombreMetodo_EstadoBajoPrueba_ComportamientoEsperado`.

**InMemory helper:**

```csharp
/// <summary>Factoría de contextos InMemory para pruebas aisladas.</summary>
public static class AyudanteDbEnMemoria
{
    public static AppDbContext CrearContexto(string nombreBd = "")
    {
        var nombre = string.IsNullOrEmpty(nombreBd)
            ? Guid.NewGuid().ToString()   // nombre único por prueba → aislamiento
            : nombreBd;

        var opciones = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nombre)
            .Options;

        return new AppDbContext(opciones);
    }
}
```

**Example test class:**

```csharp
public class RepositorioProductoPruebas
{
    [Fact]
    public async Task ObtenerPorIdAsync_ConIdExistente_RetornaProducto()
    {
        // Arrange
        await using var contexto = AyudanteDbEnMemoria.CrearContexto();
        contexto.Productos.Add(new Producto { Id = 1, Nombre = "Empanada de Pollo", Precio = 150 });
        await contexto.SaveChangesAsync();
        var repositorio = new RepositorioProducto(contexto);

        // Act
        var resultado = await repositorio.ObtenerPorIdAsync(1);

        // Assert
        Assert.True(resultado.EsExitoso);
        Assert.NotNull(resultado.Valor);
        Assert.Equal("Empanada de Pollo", resultado.Valor!.Nombre);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ConIdInexistente_RetornaFallo()
    {
        // Arrange
        await using var contexto = AyudanteDbEnMemoria.CrearContexto();
        var repositorio = new RepositorioProducto(contexto);

        // Act
        var resultado = await repositorio.ObtenerPorIdAsync(99);

        // Assert
        Assert.False(resultado.EsExitoso);
        Assert.NotNull(resultado.MensajeError);
    }
}
```

---

## 4. Dependency Flow

```
DMendez.Web   ──►  DMendez.Data
DMendez.Tests ──►  DMendez.Web
DMendez.Tests ──►  DMendez.Data
```

- `DMendez.Data` has **no dependency** on `DMendez.Web`.
- `DMendez.Tests` is **never referenced** by either production project.
- All dependencies are registered via ASP.NET Core's built-in DI container in `Program.cs` / `Extensions/`.

---

## 5. Dependency Registration

```csharp
// Extensions/ServiciosAplicacion.cs
public static class ServiciosAplicacion
{
    public static IServiceCollection AgregarServiciosAplicacion(
        this IServiceCollection servicios)
    {
        // Repositorios
        servicios.AddScoped<IRepositorioProducto, RepositorioProducto>();
        servicios.AddScoped<IRepositorioPedido, RepositorioPedido>();
        servicios.AddScoped<IRepositorioPromocion, RepositorioPromocion>();

        // Servicios
        servicios.AddScoped<IServicioPago, ServicioPagoFalso>();

        return servicios;
    }
}
```

---

## 6. Order Status Lifecycle

```
Recibido ──► En Preparación ──► En Camino ──► Entregado
    │
    └──► Cancelado  (only from Recibido, by Customer)
```

- Status advances are performed by Administrators only (except Customer cancellation from `Recibido`).
- Each transition records a timestamp in `HistorialEstado`.
- Any attempt to modify an `Entregado` order returns `OperationResult.Fallido(...)`.

---

## 7. Configuration Reference

```json
// appsettings.json
{
  "DefaultAdmin": {
    "Email": "admin@dmendez.com",
    "Password": "Admin@12345"
  },
  "Session": {
    "TimeoutMinutes": 30
  }
}
```

> These values must be overridden via environment variables or user secrets before production deployment. They must never be committed with real credentials.

---

## 8. Key Design Decisions Summary

| Decision | Choice | Reason |
|----------|--------|--------|
| Authentication | ASP.NET Core Identity (cookies) | Simplest fit for MVC web app; educational scope |
| JWT | Not used | Out of scope |
| OAuth 2.0 | Not used | Out of scope |
| Database | EF Core InMemory | No server setup required; easy test isolation |
| Payment | `ServicioPagoFalso` implementing `IServicioPago` | Educational; interface allows future real gateway |
| Failure signalling | `OperationResult<T>` | Explicit, testable, no exception-based flow control |
| Admin provisioning | Idempotent seed at startup | Ensures admin always exists; config-driven |
| Source code language | Spanish | Project requirement |
| Documentation language | English | Project requirement |
