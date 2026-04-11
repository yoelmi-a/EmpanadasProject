# US-15: Default Roles and Administrator Seed

**As** the system (on startup),
**I want to** create the default roles and an initial administrator user based on configuration,
**So that** the platform is ready for operation and has an available administrator without manual intervention.

## Acceptance Criteria

### AC-01: Idempotent Creation of "Administrador" and "Cliente" Roles
- Upon application startup, the system must check if the "Administrador" and "Cliente" roles exist in ASP.NET Core Identity.
- If they do not exist, they must be created automatically.
- If they already exist, no action should be taken.

### AC-02: Idempotent Creation of Default Administrator User
- Upon application startup, the system must check if any user exists with the "Administrador" role.
- If none exists, a user must be created using the email and password configured in `appsettings.json` (under the `DefaultAdmin` section).
- The created user must be automatically assigned to the "Administrador" role.
- If an administrator already exists, the process must finish without making changes.

### AC-03: Configuration Management
- Administrator credentials (email and password) MUST NOT be hardcoded in the source code.
- They must be read from `appsettings.json` or environment variables.

### AC-04: Result Reporting
- The seed process must return an `OperationResult` indicating success or a detailed error (e.g., if the password does not meet the policy).
- The result of the process must be logged for auditing during server startup.

## Related Technical Requirements
- FR-SEED-001 to FR-SEED-004
- BR-SEED-01 to BR-SEED-04
- FR-AUTH-007 (Support for Customer and Administrator roles)
- ASP.NET Core Identity Integration
