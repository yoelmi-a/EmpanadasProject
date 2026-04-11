# Functional Requirements — D' Méndez Empanadas

## 1. Introduction

This document lists the functional requirements for the D' Méndez Empanadas **web-only** platform. Each requirement is identified with a unique ID, a priority level (`High / Medium / Low`), and a reference to the business rule(s) it supports.

> **Scope note:** The platform is web-only. There is no mobile application. Authentication uses ASP.NET Core Identity with cookie-based sessions exclusively — no JWT and no OAuth 2.0. The payment system is simulated for educational purposes.

---

## 2. Requirement Conventions

- **ID format:** `FR-<MODULE>-<NNN>`
- **Priority:** High | Medium | Low

---

## 3. Authentication & Registration

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-AUTH-001 | The system shall allow customers to register using a valid email address and a password that meets defined complexity requirements. | High | BR-U02 |
| FR-AUTH-002 | The system shall authenticate users via **ASP.NET Core Identity** using cookie-based sessions. | High | BR-U06 |
| FR-AUTH-003 | The system shall provide a password recovery flow via a security question or admin reset (no email delivery required for this educational version). | Medium | — |
| FR-AUTH-004 | The system shall invalidate the session after a configurable inactivity timeout. | Medium | BR-U04 |
| FR-AUTH-005 | The system shall restrict Administrator account creation to the database seed process only; no registration form shall allow selecting the Administrator role. | High | BR-U03 |
| FR-AUTH-006 | The system shall redirect unauthenticated users attempting to access protected routes to the login page. | High | BR-U05 |
| FR-AUTH-007 | The system shall support two roles: `Customer` and `Administrator`, enforced via ASP.NET Core Identity role management. | High | BR-U01 |

---

## 4. Default Administrator Seed

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-SEED-001 | The system shall execute a seed process at application startup that creates the `Administrator` role if it does not exist. | High | BR-SEED-01, BR-SEED-04 |
| FR-SEED-002 | The system shall create a default Administrator user (email and password read from configuration) if no user with the `Administrator` role exists. | High | BR-SEED-01, BR-SEED-02 |
| FR-SEED-003 | The seed process shall be idempotent: re-running it shall not create duplicate roles or users. | High | BR-SEED-03 |
| FR-SEED-004 | The default Administrator credentials shall be configurable via `appsettings.json` under a dedicated `DefaultAdmin` section. | High | BR-SEED-02 |

---

## 5. Product Catalog

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-CAT-001 | The system shall display a product menu organized by category: Empanadas, Beverages, Combos. | High | BR-P01 |
| FR-CAT-002 | Each product listing shall show name, description, and price. | High | BR-P02 |
| FR-CAT-003 | The system shall allow customers to filter products by category. | Medium | BR-P01 |
| FR-CAT-004 | The system shall hide unavailable products from the customer-facing catalog. | High | BR-P03 |
| FR-CAT-005 | Combo products shall display their component items and the combined price. | Medium | BR-P05 |

---

## 6. Shopping Cart

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-CART-001 | The system shall allow authenticated customers to add products to a cart. | High | BR-C01 |
| FR-CART-002 | The system shall allow customers to update the quantity or remove items from the cart. | High | BR-C01 |
| FR-CART-003 | The system shall automatically recalculate and display the cart subtotal, taxes, and total on every change. | High | BR-C02, BR-C03 |
| FR-CART-004 | The system shall flag cart items that become unavailable and prevent order submission until they are removed. | High | BR-C04 |
| FR-CART-005 | The cart shall be associated with the authenticated user's session and not shared between accounts. | High | BR-C05 |

---

## 7. Order Management

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-ORD-001 | The system shall allow authenticated customers to place an order from their cart. | High | BR-O01 |
| FR-ORD-002 | The system shall require the customer to select a delivery method (home delivery or pickup) before confirming. | High | BR-O03 |
| FR-ORD-003 | The system shall assign a unique order ID and an initial status of `Received` upon successful placement. | High | BR-O02, BR-O04 |
| FR-ORD-004 | The system shall allow customers to cancel an order only when it is in `Received` status. | Medium | BR-O05 |
| FR-ORD-005 | The system shall allow administrators to advance order status through the defined lifecycle. | High | BR-O06 |
| FR-ORD-006 | The system shall record a timestamp each time an order status changes. | High | BR-O08 |
| FR-ORD-007 | The system shall prevent any modification to orders in `Delivered` status. | High | BR-O07 |

---

## 8. Payments (Simulated)

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-PAY-001 | The system shall process payments through a **simulated payment service** (`FakePaymentService`) that implements `IPaymentService`. | High | BR-PAY01, BR-PAY04 |
| FR-PAY-002 | The checkout page shall include a "Simulate Failure" toggle that instructs the fake payment service to return a failure result. | Medium | BR-PAY02 |
| FR-PAY-003 | The system shall display a clear error message and not create an order when the simulated payment returns a failure. | High | BR-PAY03 |
| FR-PAY-004 | The system shall generate and store a fake transaction reference (e.g., a GUID) for every successful simulated payment. | High | BR-PAY05 |
| FR-PAY-005 | The `IPaymentService` interface shall be designed so that a real gateway implementation can replace `FakePaymentService` without changes to the calling code. | High | BR-PAY04 |

---

## 9. Order Tracking & History

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-TRK-001 | The system shall display the current status of an order on the order detail page. | High | BR-O04 |
| FR-TRK-002 | The system shall show the full status history (with timestamps) for each order on the order detail page. | Medium | BR-O08 |
| FR-HIST-001 | The system shall allow customers to view a list of all their past orders. | High | BR-H01 |
| FR-HIST-002 | The order history view shall be read-only for customers. | High | BR-H02 |

---

## 10. Notifications (On-screen only)

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-NOT-001 | The system shall display a success message on-screen when an order is successfully placed. | High | BR-N01 |
| FR-NOT-002 | The order detail page shall always reflect the latest status without requiring a manual page reload. | Medium | BR-N02 |

---

## 11. Promotions

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-PROMO-001 | The system shall allow administrators to create promotions with a name, discount type (% or fixed), amount, start date, and end date. | Medium | BR-PR01, BR-PR02 |
| FR-PROMO-002 | The system shall apply a promotion discount to the order total at checkout when a valid code is entered. | Medium | BR-PR03 |
| FR-PROMO-003 | The system shall reject promotion codes that are expired or inactive. | Medium | BR-PR01 |
| FR-PROMO-004 | The system shall allow only one promotion per order. | Medium | BR-PR03 |
| FR-PROMO-005 | The system shall allow administrators to activate or deactivate promotions at any time. | Medium | BR-PR04 |

---

## 12. Administration Panel

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-ADM-001 | The system shall provide an administration dashboard accessible only to users in the `Administrator` role. | High | BR-A01 |
| FR-ADM-002 | The dashboard shall allow full CRUD operations on products. | High | BR-A03 |
| FR-ADM-003 | The dashboard shall list all orders with filtering by status. | High | BR-A04 |
| FR-ADM-004 | The dashboard shall allow administrators to advance the status of any non-delivered order. | High | BR-A04 |
| FR-ADM-005 | The dashboard shall display basic sales reports aggregated by day, week, and month. | Medium | BR-A02 |
| FR-ADM-006 | The dashboard shall allow administrators to manage promotions (create, activate, deactivate). | Medium | BR-A05 |

---

## 13. Operation Result Pattern

| ID | Description | Priority | BR Ref |
|----|-------------|----------|--------|
| FR-ORP-001 | Every service and repository method that can succeed or fail shall return an `OperationResult<T>` (or non-generic `OperationResult`) instead of throwing exceptions for expected failure conditions. | High | — |
| FR-ORP-002 | `OperationResult` shall expose at minimum: `bool IsSuccess`, `string? ErrorMessage`, and `T? Value`. | High | — |
| FR-ORP-003 | Controllers shall inspect the `OperationResult` returned by service calls and render appropriate views or error messages based on `IsSuccess`. | High | — |
| FR-ORP-004 | Unexpected exceptions (infrastructure failures, unhandled edge cases) shall still propagate and be caught by the global error handler; `OperationResult` is for **expected** business failures only. | High | — |
