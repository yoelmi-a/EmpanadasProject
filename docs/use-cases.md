# Use Cases — D' Méndez Empanadas

## 1. Actors

| Actor | Description |
|-------|-------------|
| **Guest** | An unauthenticated visitor browsing the web platform. |
| **Customer** | A registered and authenticated user. |
| **Administrator** | An internal staff account created by the database seed. |
| **FakePaymentService** | Internal simulated payment component (no external service). |
| **System (Seed)** | The application startup process that seeds the default admin. |

---

## 2. Use Case Index

| ID | Use Case | Primary Actor |
|----|----------|--------------|
| UC-01 | Register Account | Guest |
| UC-02 | Log In | Guest / Customer |
| UC-03 | Log Out | Customer / Administrator |
| UC-04 | Browse Product Catalog | Guest / Customer |
| UC-05 | Manage Shopping Cart | Customer |
| UC-06 | Place an Order | Customer |
| UC-07 | Process Simulated Payment | Customer / FakePaymentService |
| UC-08 | Cancel an Order | Customer |
| UC-09 | Track Order Status | Customer |
| UC-10 | View Order History | Customer |
| UC-11 | Manage Products (CRUD) | Administrator |
| UC-12 | Manage Orders | Administrator |
| UC-13 | Manage Promotions | Administrator |
| UC-14 | View Sales Reports | Administrator |
| UC-15 | Seed Default Administrator | System (Seed) |

---

## 3. Use Case Descriptions

---

### UC-01 — Register Account

**Actor:** Guest  
**Preconditions:** The user is not authenticated.  
**Postconditions:** A new `Customer` account is created; the user is redirected to the home page as an authenticated Customer.

**Main Flow:**
1. The guest navigates to the registration page.
2. The guest enters a valid email address and a password that meets complexity requirements.
3. The system validates the input (email uniqueness, password policy) via ASP.NET Core Identity.
4. Identity creates the account, assigns the `Customer` role, and signs the user in with a cookie.
5. The system displays a welcome message and redirects to the home page.

**Exception Flows:**
- Email already registered → Identity returns an error; the form is re-displayed with the message.
- Password does not meet policy → validation error highlighted on the password field.

**Operation Result:** The registration service method returns `OperationResult` indicating success or the Identity error messages on failure.

**Related Requirements:** FR-AUTH-001, FR-AUTH-007, BR-U02

---

### UC-02 — Log In

**Actor:** Guest  
**Preconditions:** The user has a registered account.  
**Postconditions:** The user is authenticated; a session cookie is issued by ASP.NET Core Identity.

**Main Flow:**
1. The user navigates to the login page.
2. The user enters their email and password.
3. Identity validates the credentials and issues a session cookie.
4. The user is redirected to the page they were trying to access (or the home page).

**Exception Flows:**
- Invalid credentials → Identity returns a failed sign-in; a generic error is displayed (no user enumeration).
- Account locked out → an appropriate message is shown.

**Operation Result:** The login service method returns `OperationResult` with the error reason on failure.

**Related Requirements:** FR-AUTH-002, BR-U06

---

### UC-03 — Log Out

**Actor:** Customer / Administrator  
**Preconditions:** The user is authenticated.  
**Postconditions:** The session cookie is invalidated; the user is redirected to the home page.

**Main Flow:**
1. The user clicks "Log Out."
2. Identity signs the user out (cookie invalidated).
3. The user is redirected to the home page as a Guest.

**Related Requirements:** FR-AUTH-002

---

### UC-04 — Browse Product Catalog

**Actor:** Guest / Customer  
**Preconditions:** None.  
**Postconditions:** The user views available products.

**Main Flow:**
1. The user navigates to the catalog page.
2. The system retrieves all **available** products via the product repository.
3. Products are displayed organized by category (Empanadas, Beverages, Combos).
4. The user can filter by category using navigation tabs or links.
5. The user can select a product to view its detail (name, description, price).

**Exception Flows:**
- Repository returns an `OperationResult` failure → an error message is displayed; no products are shown.

**Related Requirements:** FR-CAT-001 to FR-CAT-005, BR-P01 to BR-P04

---

### UC-05 — Manage Shopping Cart

**Actor:** Customer  
**Preconditions:** The user is authenticated as a Customer.  
**Postconditions:** The cart reflects the Customer's desired products and quantities.

**Main Flow:**
1. The Customer selects a product from the catalog and clicks "Add to Cart."
2. The cart service adds the item and recalculates the subtotal, taxes, and total.
3. The Customer navigates to the cart page to review items.
4. The Customer can increase/decrease quantities or remove items.
5. The system recalculates totals after each change.

**Exception Flows:**
- A product becomes unavailable while in the cart → the item is flagged; checkout is blocked with a clear message until it is removed.
- Cart service returns an `OperationResult` failure (e.g., product not found) → an error message is displayed.

**Related Requirements:** FR-CART-001 to FR-CART-005, BR-C01 to BR-C05

---

### UC-06 — Place an Order

**Actor:** Customer  
**Preconditions:** The Customer is authenticated and has at least one valid item in the cart.  
**Postconditions:** An order is created with `Received` status; an on-screen confirmation is displayed.

**Main Flow:**
1. The Customer proceeds to checkout.
2. The Customer selects a delivery method (Home Delivery or Pickup).
3. The Customer optionally enters a promotion code; the system validates and applies it.
4. The system displays an order summary (items, subtotal, discount, taxes, total).
5. The Customer clicks "Confirm Order," which triggers the simulated payment (UC-07).
6. If payment succeeds, the order service creates the order with status `Received` and a timestamp.
7. The system clears the cart and displays a success message with the order ID.

**Exception Flows:**
- Cart contains unavailable items → Customer is redirected to the cart to resolve them before proceeding.
- Payment fails (UC-07 failure path) → order is not created; the failure message is shown on the checkout page.
- Order service returns `OperationResult` failure → the error is displayed; the Customer can retry.

**Related Requirements:** FR-ORD-001 to FR-ORD-003, FR-NOT-001, BR-O01 to BR-O04

---

### UC-07 — Process Simulated Payment

**Actor:** Customer, FakePaymentService  
**Preconditions:** The Customer has confirmed an order at checkout.  
**Postconditions:** A simulated payment result (success or failure) is returned to the order flow.

**Main Flow:**
1. The order service calls `IPaymentService.ProcessAsync(paymentRequest)`.
2. `FakePaymentService` evaluates the `SimulateFailure` flag from the request.
3. If `SimulateFailure` is `false`, the service returns a successful `OperationResult` containing a fake GUID transaction reference.
4. The order service stores the transaction reference on the order and proceeds.

**Alternative Flow — Simulate Failure:**
1. The Customer checked "Simulate Failure" on the checkout page.
2. `FakePaymentService` returns an `OperationResult` failure with a descriptive error message.
3. No order is created; the failure message is rendered on the checkout page.

**Related Requirements:** FR-PAY-001 to FR-PAY-005, BR-PAY01 to BR-PAY05

---

### UC-08 — Cancel an Order

**Actor:** Customer  
**Preconditions:** The Customer has an order in `Received` status.  
**Postconditions:** The order status changes to `Cancelled`; an on-screen confirmation is shown.

**Main Flow:**
1. The Customer navigates to the order detail page.
2. The Customer clicks "Cancel Order."
3. The order service validates that the order belongs to the requesting Customer and is in `Received` status.
4. The service updates the status to `Cancelled` and records the timestamp.
5. The system returns an `OperationResult` success and renders a confirmation message.

**Exception Flows:**
- Order is not in `Received` status → service returns `OperationResult` failure with message "El pedido no puede ser cancelado en su estado actual."
- Order does not belong to the requesting Customer → service returns `OperationResult` failure (unauthorized).

**Related Requirements:** FR-ORD-004, BR-O05

---

### UC-09 — Track Order Status

**Actor:** Customer  
**Preconditions:** The Customer has placed at least one order.  
**Postconditions:** The Customer views the current status and history of their order.

**Main Flow:**
1. The Customer navigates to the order detail page (from order history or the confirmation link).
2. The system retrieves the order and its status history via the order repository.
3. The page displays: current status, a timeline of all past statuses with timestamps.

**Exception Flows:**
- Order not found or does not belong to the Customer → service returns `OperationResult` failure; 404 page is displayed.

**Related Requirements:** FR-TRK-001, FR-TRK-002, BR-O04, BR-O08

---

### UC-10 — View Order History

**Actor:** Customer  
**Preconditions:** The Customer is authenticated and has placed at least one order.  
**Postconditions:** The Customer sees a read-only list of all their past orders.

**Main Flow:**
1. The Customer navigates to "My Orders."
2. The system retrieves all orders for the authenticated Customer (sorted by date, most recent first).
3. Each order entry shows: order ID, date, total, and current status.
4. The Customer can click an order to view its detail (UC-09).

**Related Requirements:** FR-HIST-001, FR-HIST-002, BR-H01, BR-H02

---

### UC-11 — Manage Products (CRUD)

**Actor:** Administrator  
**Preconditions:** The Administrator is authenticated and on the admin dashboard.  
**Postconditions:** The product catalog is updated accordingly.

**Main Flow — Create:**
1. The Administrator navigates to "Products → New Product."
2. They fill in name, description, price, and category, then submit.
3. The product service validates the input and returns `OperationResult` success.
4. The new product is visible in the catalog.

**Main Flow — Edit:**
1. The Administrator selects a product and modifies its fields.
2. The service validates and saves; returns `OperationResult` success.

**Main Flow — Delete:**
1. The Administrator selects a product and confirms deletion.
2. The service removes it; returns `OperationResult` success.

**Main Flow — Toggle Availability:**
1. The Administrator marks a product as available or unavailable.
2. The service updates the flag; returns `OperationResult` success.

**Exception Flows:**
- Missing required fields → service returns `OperationResult` failure with validation messages; form is re-displayed.
- Price ≤ 0 → service returns `OperationResult` failure.

**Related Requirements:** FR-ADM-002, BR-P02 to BR-P06

---

### UC-12 — Manage Orders

**Actor:** Administrator  
**Preconditions:** The Administrator is authenticated and on the admin dashboard.  
**Postconditions:** Order status is updated; the change is visible to the Customer on their order detail page.

**Main Flow:**
1. The Administrator navigates to "Orders."
2. The system lists all orders with filter-by-status capability.
3. The Administrator selects an order and clicks the next-status action button.
4. The order service updates the status, records the timestamp, and returns `OperationResult` success.

**Exception Flows:**
- Attempting to advance a `Delivered` order → service returns `OperationResult` failure; action button is not shown.

**Related Requirements:** FR-ADM-003, FR-ADM-004, BR-O06, BR-O07

---

### UC-13 — Manage Promotions

**Actor:** Administrator  
**Preconditions:** The Administrator is authenticated and on the admin dashboard.  
**Postconditions:** Promotions are created, activated, or deactivated.

**Main Flow — Create:**
1. The Administrator navigates to "Promotions → New Promotion."
2. They set name, discount type (% or fixed), amount, start date, and end date, then submit.
3. The promotion service validates and saves; returns `OperationResult` success.

**Main Flow — Activate / Deactivate:**
1. The Administrator toggles the active state of an existing promotion.
2. The service updates the flag; returns `OperationResult` success.

**Exception Flows:**
- End date is before start date → service returns `OperationResult` failure with a validation message.

**Related Requirements:** FR-PROMO-001, FR-PROMO-005, BR-PR01 to BR-PR04

---

### UC-14 — View Sales Reports

**Actor:** Administrator  
**Preconditions:** The Administrator is authenticated and on the admin dashboard.  
**Postconditions:** The Administrator views aggregated sales data.

**Main Flow:**
1. The Administrator navigates to "Reports."
2. They select an aggregation period (day / week / month).
3. The system queries the order repository and returns aggregated totals and order counts.
4. The page displays the data in a table or simple chart.

**Related Requirements:** FR-ADM-005, BR-A02

---

### UC-15 — Seed Default Administrator

**Actor:** System (Seed) — runs automatically at application startup  
**Preconditions:** The application has just started; no Administrator user exists.  
**Postconditions:** The `Administrator` role and a default admin user exist in the Identity store.

**Main Flow:**
1. At startup, the seed service checks whether the `Administrator` role exists in Identity.
2. If not, it creates the role.
3. The seed service checks whether any user with the `Administrator` role exists.
4. If not, it reads the default admin email and password from `appsettings.json` (`DefaultAdmin` section).
5. It creates the user via `UserManager` and assigns the `Administrator` role.
6. The seed service returns `OperationResult` success and logs the action.

**Alternative Flow — Already Seeded:**
- Role and admin user already exist → the service skips creation and returns `OperationResult` success immediately (idempotent).

**Exception Flows:**
- `UserManager.CreateAsync` fails (e.g., password does not meet policy) → the service returns `OperationResult` failure and logs the error; application startup continues but a warning is logged.

**Related Requirements:** FR-SEED-001 to FR-SEED-004, BR-SEED-01 to BR-SEED-04
