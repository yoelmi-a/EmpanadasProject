# Non-Functional Requirements — D' Méndez Empanadas

## 1. Introduction

This document describes the non-functional requirements (NFRs) for the D' Méndez Empanadas web platform. These requirements define quality, maintainability, and operational constraints that apply across all layers of the architecture.

> **Educational context:** This platform is built for educational purposes. Certain production-grade NFRs (e.g., high availability SLAs, cloud scaling) are documented for completeness and future reference but are **not enforced** in the educational build, as noted per requirement.

---

## 2. Requirement Conventions

- **ID format:** `NFR-<CATEGORY>-<NNN>`
- **Priority:** High | Medium | Low
- **Categories:** PERF (Performance), SEC (Security), MAINT (Maintainability), SCALE (Scalability), COMPAT (Compatibility), USAB (Usability), LEGAL (Legal & Compliance)

---

## 3. Performance

| ID | Description | Priority | Enforced in Educational Build |
|----|-------------|----------|-------------------------------|
| NFR-PERF-001 | The system shall respond to UI requests in under **2 seconds** under normal load conditions. | High | Yes |
| NFR-PERF-002 | Queries used on the catalog and cart pages shall complete in under **500 ms**. | High | Yes |
| NFR-PERF-003 | The system shall support at least **50 concurrent users** without noticeable degradation. | Medium | No (educational scope) |

---

## 4. Security

| ID | Description | Priority | Enforced in Educational Build |
|----|-------------|----------|-------------------------------|
| NFR-SEC-001 | All user passwords shall be stored using **ASP.NET Core Identity's default hashing** algorithm (PBKDF2 with HMAC-SHA256). | High | Yes |
| NFR-SEC-002 | The application shall use **HTTPS** in production deployments. For local development, HTTP is acceptable. | High | Yes (production) |
| NFR-SEC-003 | Authentication and session management shall be handled exclusively by **ASP.NET Core Identity** with cookie-based sessions. JWT and OAuth 2.0 are explicitly out of scope. | High | Yes |
| NFR-SEC-004 | All forms shall include **ASP.NET Core Anti-Forgery tokens** (`[ValidateAntiForgeryToken]`) to prevent CSRF attacks. | High | Yes |
| NFR-SEC-005 | Route-level authorization shall be enforced using `[Authorize]` and `[Authorize(Roles = "Administrator")]` attributes. | High | Yes |
| NFR-SEC-006 | The application shall not expose sensitive configuration values (default admin password, connection strings) in source code; they shall be read from `appsettings.json` or environment variables. | High | Yes |
| NFR-SEC-007 | Audit logging of authentication events (login, logout, failed login) is recommended but not required in the educational build. | Low | No |

---

## 5. Maintainability

| ID | Description | Priority | Enforced in Educational Build |
|----|-------------|----------|-------------------------------|
| NFR-MAINT-001 | The source code shall follow the **SOLID**, **KISS**, and **DRY** principles as defined in `code-guidelines.md`. | High | Yes |
| NFR-MAINT-002 | Every public and internal method shall have **XML documentation comments** (`<summary>`, `<param>`, `<returns>`). | High | Yes |
| NFR-MAINT-003 | **Every method** in the Web and Data layers shall have corresponding unit tests covering at least one **success scenario** and one **failure scenario**. | High | Yes |
| NFR-MAINT-004 | Unit tests shall use the **InMemory EF Core database provider** exclusively; no real or containerized database is required. | High | Yes |
| NFR-MAINT-005 | All service and repository methods that can fail for expected business reasons shall return an `OperationResult<T>` instead of throwing exceptions. | High | Yes |
| NFR-MAINT-006 | Database schema changes shall be managed via **EF Core migrations** only; manual SQL scripts in any environment are forbidden. | High | Yes |
| NFR-MAINT-007 | Version control shall use **Git** with a defined branching strategy (e.g., GitFlow: `main`, `develop`, `feature/*`, `fix/*`). | High | Yes |
| NFR-MAINT-008 | A CI pipeline (e.g., GitHub Actions) shall build the project and run all tests automatically on every pull request. | Medium | Recommended |
| NFR-MAINT-009 | The minimum unit-test coverage target for the Web and Data layers is **80 %** of all methods. | High | Yes |

---

## 6. Scalability

| ID | Description | Priority | Enforced in Educational Build |
|----|-------------|----------|-------------------------------|
| NFR-SCALE-001 | The application architecture shall allow future migration to a persistent database (SQL Server, PostgreSQL) by swapping the EF Core provider with no changes to repository or service code. | High | Yes (by design) |
| NFR-SCALE-002 | The Web layer shall remain stateless (no in-process session state tied to a specific server instance), enabling future horizontal scaling. | Medium | No (educational scope) |

---

## 7. Compatibility

| ID | Description | Priority | Enforced in Educational Build |
|----|-------------|----------|-------------------------------|
| NFR-COMPAT-001 | The web application shall be compatible with current versions of **Chrome**, **Firefox**, **Safari**, and **Edge**. | High | Yes |
| NFR-COMPAT-002 | The web UI shall be **responsive**, functioning correctly on screens as narrow as 375 px (mobile browser). | High | Yes |
| NFR-COMPAT-003 | The platform is **web-only**. No native mobile application is in scope. | High | Yes |

---

## 8. Usability

| ID | Description | Priority | Enforced in Educational Build |
|----|-------------|----------|-------------------------------|
| NFR-USAB-001 | The complete flow from product selection to order confirmation shall require no more than **5 distinct steps**. | Medium | Yes |
| NFR-USAB-002 | The system shall display clear, user-friendly error messages whenever an operation fails. | High | Yes |
| NFR-USAB-003 | The system shall show loading indicators for operations that take more than **1 second**. | Medium | Yes |
| NFR-USAB-004 | The platform's primary language shall be **Spanish**. All UI labels, messages, and validation errors shall be written in Spanish. | High | Yes |

---

## 9. Legal & Compliance

| ID | Description | Priority | Enforced in Educational Build |
|----|-------------|----------|-------------------------------|
| NFR-LEGAL-001 | The platform is for **educational use only**; it shall not process real financial transactions or store real payment data. | High | Yes |
| NFR-LEGAL-002 | The software development lifecycle shall be aligned with **ISO/IEC 12207** processes as documented in `srs.md`. | Medium | Documented only |
