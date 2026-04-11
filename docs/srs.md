# Software Requirements Specification (SRS)
## D' Méndez Empanadas — Web Platform
**Version:** 2.0  
**Standard:** ISO/IEC 12207 — Software Life Cycle Processes  
**Date:** 2025

---

## Table of Contents

1. [Introduction](#1-introduction)
2. [Overall Description](#2-overall-description)
3. [Functional Requirements Summary](#3-functional-requirements-summary)
4. [Non-Functional Requirements Summary](#4-non-functional-requirements-summary)
5. [External Interface Requirements](#5-external-interface-requirements)
6. [Software Life Cycle Processes (ISO/IEC 12207)](#6-software-life-cycle-processes-isoiec-12207)
7. [Constraints & Assumptions](#7-constraints--assumptions)
8. [Document Approval](#8-document-approval)

---

## 1. Introduction

### 1.1 Purpose

This Software Requirements Specification (SRS) defines the functional and non-functional requirements for the **web platform** of D' Méndez Empanadas. It is adapted to the ISO/IEC 12207 standard and serves as the authoritative reference for the design, development, testing, and maintenance of the system.

All stakeholders — project owner, development team, and QA team — must use this document as the baseline for all system-related decisions.

### 1.2 Scope

The platform is **web-only** and provides the following capabilities:

**Customer-facing features:**
- Browse a categorized product menu (Empanadas, Beverages, Combos)
- Register and authenticate via ASP.NET Core Identity (email + password, cookie sessions)
- Manage a shopping cart
- Place and pay for orders via a **simulated payment system**
- Track order status
- View order history
- Receive on-screen confirmations and status updates

**Administrator-facing features:**
- Manage products (CRUD)
- Manage and update orders
- Manage promotional offers
- View basic sales reports

**Out of scope:**
- Native mobile applications (iOS / Android)
- JWT authentication
- OAuth 2.0 / social login
- Real payment gateway integrations
- Email or push notifications

### 1.3 Definitions, Acronyms, and Abbreviations

| Term | Definition |
|------|-----------|
| SRS | Software Requirements Specification |
| API | Application Programming Interface |
| MVC | Model-View-Controller — architectural pattern used for the web application |
| EF Core | Entity Framework Core — ORM for .NET used for data access |
| InMemory DB | EF Core in-memory database provider used for development and testing |
| Identity | ASP.NET Core Identity — membership and authentication system |
| OperationResult | A return-type pattern that encapsulates success/failure without throwing exceptions |
| JWT | JSON Web Token — **not used in this project** |
| OAuth 2.0 | Open Authorization standard — **not used in this project** |
| CRUD | Create, Read, Update, Delete |
| DI | Dependency Injection |
| SOLID | Five OO design principles (Single Responsibility, Open/Closed, Liskov, Interface Segregation, Dependency Inversion) |
| KISS | Keep It Simple, Stupid |
| DRY | Don't Repeat Yourself |
| ISO/IEC 12207 | International standard defining software life cycle processes |
| SRP | Single Responsibility Principle |
| FR | Functional Requirement |
| NFR | Non-Functional Requirement |
| BR | Business Rule |

### 1.4 References

| Document | Description |
|----------|-------------|
| `business-rules.md` | Complete business rules catalogue |
| `functional-requirements.md` | Detailed functional requirements |
| `non-functional-requirements.md` | Detailed non-functional requirements |
| `use-cases.md` | Use case descriptions |
| `architecture.md` | System architecture definition |
| `code-guidelines.md` | Coding standards, naming conventions, and design principles |
| ISO/IEC 12207:2017 | Software and systems engineering — Software life cycle processes |
| OWASP Top 10 | Common web application security risks |

---

## 2. Overall Description

### 2.1 Product Perspective

The platform is a **web application** following a 3-layer .NET 10 architecture:

| Component | Technology | Description |
|-----------|-----------|-------------|
| **Web Layer** | ASP.NET Core 10 MVC | Handles HTTP requests, renders Razor views, enforces authentication and authorization |
| **Data Layer** | EF Core 10 — InMemory Provider | Manages entities, repositories, and data access logic |
| **Test Layer** | xUnit | Unit tests covering every method with success and failure scenarios |

### 2.2 User Classes

| User Class | Description | Access Level |
|-----------|-------------|-------------|
| **Guest** | Unauthenticated visitor | Browse catalog (read-only) |
| **Customer** | Registered & authenticated user | Cart, orders, history |
| **Administrator** | Seeded internal staff account | Full management panel |

### 2.3 Authentication Model

Authentication is handled exclusively by **ASP.NET Core Identity** with cookie-based sessions:

- Customers register via a web form (email + password).
- The Administrator account is created by a **database seed** that runs at application startup.
- No JWT, no OAuth 2.0, and no social login are implemented.
- Role-based authorization (`Customer`, `Administrator`) is enforced via Identity roles.

### 2.4 Payment Model

The payment system is **simulated** for educational purposes:

- A `FakePaymentService` implements `IPaymentService`.
- It always returns success unless the user checks a "Simulate Failure" option.
- It generates a fake GUID transaction reference, mirroring real gateway behaviour.
- The interface design allows future replacement with a real gateway.

### 2.5 Data Persistence

- The EF Core **InMemory database provider** is used throughout development and testing.
- All repository and service code is provider-agnostic, enabling future migration to SQL Server or PostgreSQL by changing the provider registration only.

### 2.6 Operating Environment

- **Runtime:** .NET 10 on the developer's local machine or any .NET-compatible host.
- **Database:** EF Core InMemory (no external database server required).
- **Web browsers:** Chrome, Firefox, Safari, Edge (current versions).
- **Platform:** Web only; no mobile app.

### 2.7 Design and Implementation Constraints

- The backend must be implemented in **C# / .NET 10**.
- The data access layer must use **Entity Framework Core** with the InMemory provider.
- Unit tests must be written with **xUnit**.
- **Every method** must have unit tests covering at least one success and one failure scenario.
- All service and repository methods that can fail for expected reasons must return `OperationResult<T>`.
- No credentials or secrets may be committed to source control.
- The project's source code (identifiers, comments, XML docs) must be written in **Spanish**.

---

## 3. Functional Requirements Summary

Full descriptions are in `functional-requirements.md`.

| Module | Key Requirements |
|--------|----------------|
| Authentication & Registration | Email + password registration, Identity cookie sessions, role enforcement |
| Default Admin Seed | Idempotent seed creating `Administrator` role and default admin user from config |
| Product Catalog | Categorized listing (Empanadas, Beverages, Combos), availability flags |
| Shopping Cart | Add/remove/update, tax calculation, availability checks, session-scoped |
| Orders | Placement, delivery method, status lifecycle (`Received → Delivered`), cancellation |
| Payments (Simulated) | `FakePaymentService` implementing `IPaymentService`, simulate failure toggle, GUID reference |
| Order Tracking | Current status + timestamps on order detail page |
| Order History | Customer read-only list of past orders |
| Notifications | On-screen success/error messages only |
| Promotions | % or fixed discount, date-bounded, one per order, admin-managed |
| Administration Panel | CRUD products, order management, promotions, basic sales reports |
| Operation Result Pattern | All service/repository methods return `OperationResult<T>` for expected failures |

---

## 4. Non-Functional Requirements Summary

Full descriptions are in `non-functional-requirements.md`.

| Category | Key Constraint |
|----------|---------------|
| Performance | Response < 2 s; DB queries < 500 ms |
| Security | Identity password hashing, anti-forgery tokens, role-based authorization, no secrets in code |
| Maintainability | SOLID/KISS/DRY, XML docs, 80 % test coverage, every method tested (success + failure), OperationResult pattern |
| Scalability | Provider-agnostic data layer (easy swap from InMemory to real DB) |
| Compatibility | Chrome/Firefox/Safari/Edge current; responsive at 375 px+; web-only |
| Usability | ≤ 5-step checkout, Spanish UI, clear error messages |
| Legal | Educational only; no real payments; ISO/IEC 12207 lifecycle alignment |

---

## 5. External Interface Requirements

### 5.1 User Interfaces

- The web application must be fully responsive.
- The administrative dashboard must be accessible from desktop browsers.
- All UI text must be in **Spanish**.

### 5.2 Hardware Interfaces

Not applicable. The system runs locally or on any .NET-compatible host; there are no direct hardware dependencies.

### 5.3 Software Interfaces

| External System | Purpose | Notes |
|----------------|---------|-------|
| ASP.NET Core Identity | User management, authentication, role management | Built-in; no external service |
| EF Core InMemory | Data persistence during runtime and tests | No database server required |
| `FakePaymentService` | Simulated payment processing | Implements `IPaymentService`; in-process only |

### 5.4 Communication Interfaces

- All communication between the browser and the server uses standard **HTTP/HTTPS**.
- No external APIs, webhooks, or message queues are used in this educational build.

---

## 6. Software Life Cycle Processes (ISO/IEC 12207)

### 6.1 Primary Processes

| Process | Implementation |
|---------|---------------|
| **Acquisition** | Developed as a proprietary educational solution for D' Méndez Empanadas. |
| **Supply** | Development team delivers incremental releases for stakeholder review before final delivery. |
| **Development** | Agile methodology (Scrum); sprints aligned with SRS requirements. |
| **Operation** | Application runs locally or on a .NET-compatible host; no 24/7 SLA in educational scope. |
| **Maintenance** | Corrective and evolutionary maintenance cycles following each release. |

### 6.2 Supporting Processes

| Process | Implementation |
|---------|---------------|
| **Documentation** | All deliverables include technical documentation in Markdown. Source code documented in Spanish via XML comments. |
| **Configuration Management** | Version control via Git (GitFlow branching strategy). |
| **Quality Assurance** | xUnit tests for every method (success + failure); code reviews on all pull requests. |
| **Verification & Validation** | Acceptance testing by the project owner against requirements in this SRS. |
| **Problem Resolution** | Issues tracked in a project management tool (GitHub Issues or equivalent). |

### 6.3 Organizational Processes

| Process | Implementation |
|---------|---------------|
| **Project Management** | Sprint schedule with defined delivery milestones. |
| **Process Improvement** | Sprint retrospectives to identify and implement improvements. |

---

## 7. Constraints & Assumptions

### 7.1 Constraints

- The system must be built with **.NET 10** and **C#**.
- Data access must use **Entity Framework Core** with the **InMemory** provider.
- Unit tests must use **xUnit**.
- **Every method** must have at least one success test and one failure test.
- Service and repository methods must return `OperationResult<T>` for expected failure paths.
- No JWT, no OAuth 2.0, no real payment gateways.
- Source code identifiers, XML documentation, and comments must be written in **Spanish**.

### 7.2 Assumptions

- The client (D' Méndez Empanadas) will supply product content (names, descriptions, prices).
- The development team has experience with ASP.NET Core MVC, EF Core, and xUnit.
- No external database server needs to be provisioned for development or testing.
- The default Administrator credentials will be defined in `appsettings.json` before first run.

---

## 8. Document Approval

This document must be reviewed and approved by the project owner prior to the start of the design and development phase. It serves as the baseline for auditing system conformance with ISO/IEC 12207.

| Role | Name | Signature | Date |
|------|------|-----------|------|
| Project Owner | | | |
| Lead Developer | | | |
| QA Lead | | | |
