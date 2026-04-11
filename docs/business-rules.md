# Business Rules — D' Méndez Empanadas

## 1. Overview

D' Méndez Empanadas is a food business that sells empanadas, beverages, and combos through a **web-only** platform. This document describes the business rules that govern the platform's behaviour and that must be enforced at the domain and application layers.

> **Educational context:** This platform is built for educational purposes. The payment system is simulated; no real financial transactions are processed.

---

## 2. Users & Roles

| Rule ID | Rule Description |
|---------|-----------------|
| BR-U01 | There are two types of users: **Customer** and **Administrator**. |
| BR-U02 | A Customer must register with a valid email address and password before placing an order. |
| BR-U03 | An Administrator account is created exclusively through the **database seed** process; self-registration as Administrator is not allowed. |
| BR-U04 | A Customer session expires after a configurable period of inactivity. |
| BR-U05 | A Customer must be authenticated before accessing the shopping cart, order history, or checkout. |
| BR-U06 | Authentication is managed entirely by **ASP.NET Core Identity** using cookie-based sessions. No JWT tokens or OAuth 2.0 are used. |

---

## 3. Default Administrator Seed

| Rule ID | Rule Description |
|---------|-----------------|
| BR-SEED-01 | The application must seed a default Administrator account on first startup if no Administrator exists in the database. |
| BR-SEED-02 | The default Administrator credentials (email and password) must be defined via application configuration (`appsettings.json` or environment variables); they must never be hardcoded in source code. |
| BR-SEED-03 | The seed process must be **idempotent**: running it multiple times must not create duplicate accounts or roles. |
| BR-SEED-04 | The seeded Administrator must be assigned the `Administrator` role, which must also be created by the seed if it does not already exist. |

---

## 4. Product Catalog

| Rule ID | Rule Description |
|---------|-----------------|
| BR-P01 | Products are organized into three categories: **Empanadas**, **Beverages**, and **Combos**. |
| BR-P02 | Every product must have a name, description, and price before it can be published. |
| BR-P03 | A product can be marked as **unavailable** without being deleted from the catalog. |
| BR-P04 | Price must always be a positive value greater than zero. |
| BR-P05 | A Combo must reference at least two individual products and must define a combined price lower than the sum of its components. |
| BR-P06 | Only Administrators can create, edit, or delete products. |

---

## 5. Shopping Cart

| Rule ID | Rule Description |
|---------|-----------------|
| BR-C01 | A Customer can add one or more products to the cart before checking out. |
| BR-C02 | The cart must recalculate the total automatically when a product is added, removed, or its quantity changes. |
| BR-C03 | The cart total must include applicable taxes. |
| BR-C04 | A product that becomes unavailable while in a Customer's cart must be flagged and cannot be submitted as part of an order. |
| BR-C05 | The cart is tied to the authenticated session and must not persist across different user accounts. |

---

## 6. Orders

| Rule ID | Rule Description |
|---------|-----------------|
| BR-O01 | An order can only be placed by an authenticated Customer. |
| BR-O02 | An order must include at least one product. |
| BR-O03 | The Customer must select a delivery method (**home delivery** or **pickup**) before confirming. |
| BR-O04 | An order progresses through the following statuses in sequence: `Received → In Preparation → On the Way → Delivered`. |
| BR-O05 | An order can only be **cancelled** while in `Received` status. |
| BR-O06 | Only an Administrator can advance or update an order's status. |
| BR-O07 | Once an order reaches `Delivered` status, it cannot be modified. |
| BR-O08 | The system must record a timestamp for each status transition. |

---

## 7. Payments

| Rule ID | Rule Description |
|---------|-----------------|
| BR-PAY01 | The payment system is **simulated** for educational purposes. No real financial transactions are processed. |
| BR-PAY02 | The simulated payment always succeeds unless the Customer deliberately selects a "Simulate Failure" option. |
| BR-PAY03 | If the simulated payment fails, the order must not be created and the Customer must be informed. |
| BR-PAY04 | The payment module must implement the same interface (`IPaymentService`) that a real gateway would implement, enabling future replacement without modifying calling code. |
| BR-PAY05 | A transaction reference must be generated and stored for every simulated payment, mirroring real gateway behaviour. |

---

## 8. Notifications

| Rule ID | Rule Description |
|---------|-----------------|
| BR-N01 | The system must display an on-screen confirmation message when an order is successfully placed. |
| BR-N02 | The Customer must be able to see status changes reflected immediately on the order detail page. |

> Email and push notifications are out of scope for this educational version.

---

## 9. Promotions

| Rule ID | Rule Description |
|---------|-----------------|
| BR-PR01 | Promotions have a defined start date and end date; they cannot be applied outside this window. |
| BR-PR02 | A promotion can apply a percentage discount or a fixed-amount discount. |
| BR-PR03 | Only one promotion can be applied per order. |
| BR-PR04 | Only Administrators can create, activate, or deactivate promotions. |

---

## 10. Order History

| Rule ID | Rule Description |
|---------|-----------------|
| BR-H01 | A Customer can view all their past orders. |
| BR-H02 | Order history is read-only for Customers; they cannot modify historical records. |

---

## 11. Administrative Panel

| Rule ID | Rule Description |
|---------|-----------------|
| BR-A01 | Administrators have access to a dedicated management dashboard not accessible to Customers. |
| BR-A02 | The dashboard must display sales reports aggregated by day, week, and month. |
| BR-A03 | Administrators can manage products (create, read, update, delete). |
| BR-A04 | Administrators can manage orders and update their status. |
| BR-A05 | Administrators can manage promotions (create, activate, deactivate). |
