# WinForms Conversion Documentation

## Overview

This document explains the conversion of the existing ASP.NET Core MVC Gym Management System into a C# Windows Forms (WinForms) desktop application while reusing existing models, logic patterns, and the database schema.

The goal was to replace only the presentation layer (`Controllers` + `.cshtml` Views) with WinForms UI and event-driven logic, and keep the system behavior aligned with the original project.

---

## Conversion Scope

### Reused Components

- Existing domain model projects and classes:
  - `OneFitnessVue.Model`
  - `OneFitnessVue.Common`
  - `OneFitnessVue.ViewModel`
- Existing SQL Server schema and table structure:
  - `MemberRegistration`
  - `PaymentDetails`
  - `MembershipTypes`
  - `Installments`
  - `WorkOuts`
  - `Usermaster`
- Core business logic behavior (ported/reused in desktop flow):
  - Login hashing strategy
  - Member validation rules (mobile/email/name constraints)
  - Age calculation from DOB
  - Tax and total payment calculations (GST/VAT style flow)
  - Invoice number generation approach

### Replaced Components

- ASP.NET MVC web presentation:
  - Controllers replaced by WinForms event handlers
  - Razor views replaced by WinForms forms and controls
- Presentation-layer EF interactions replaced by ADO.NET-based helper/repository for desktop execution

---

## New WinForms Project

### Project Added

- `OneFitnessVue.WinForms/OneFitnessVue.WinForms.csproj`

### Project Configuration

- Target framework: `net8.0-windows`
- Enabled WinForms: `UseWindowsForms=true`
- Windows targeting enabled
- References added to existing reusable projects:
  - `..\OneFitnessVue.Common\OneFitnessVue.Common.csproj`
  - `..\OneFitnessVue.Model\OneFitnessVue.Model.csproj`
  - `..\OneFitnessVue.ViewModel\OneFitnessVue.ViewModel.csproj`
- Added package:
  - `Microsoft.Data.SqlClient` (ADO.NET SQL Server provider)

### App Configuration

- `OneFitnessVue.WinForms/appsettings.json`
  - Contains `ConnectionStrings:DatabaseConnection`
  - Uses existing SQL Server database connection strategy

### Entry Point

- `OneFitnessVue.WinForms/Program.cs`
  - Loads connection string from `appsettings.json`
  - Initializes data layer/services
  - Starts application with `LoginForm`

---

## Data Access Conversion (EF to ADO.NET in desktop layer)

### Database Helper

File: `OneFitnessVue.WinForms/Data/DatabaseHelper.cs`

Implemented required methods:

- `ExecuteQuery(string sql, IEnumerable<SqlParameter>? parameters = null, SqlTransaction? transaction = null)`
  - Executes read operations and returns `DataTable`
- `ExecuteNonQuery(string sql, IEnumerable<SqlParameter>? parameters = null, SqlTransaction? transaction = null)`
  - Executes insert/update/delete operations and returns affected row count

Supporting method:

- `CreateConnection()`
  - Returns `SqlConnection` for repository-level transaction workflows

---

## Repository Layer

### GymRepository

File: `OneFitnessVue.WinForms/Data/GymRepository.cs`

This class centralizes database operations for all modules and preserves behavior from the MVC query/command flow.

Implemented responsibilities:

- Authentication data operations:
  - Check username existence
  - Load stored password hash
  - Load user session details
- Member registration support:
  - Generate member number (`OFV{dayOfYear}{random}`)
  - Check duplicate mobile/email
  - Add member + first payment in a single transaction
- Payment operations:
  - Add payment record (renewal flow)
  - Calculate invoice number using stored procedure `Usp_GetNewInvoiceId`
  - Fallback invoice generation with `MAX(InvoiceNo) + 1` if needed
- Lookup loading:
  - Membership types
  - Workouts
  - Installments
  - Payment types
  - Tax options
- Search/list and reports:
  - Search members for DataGridView
  - Search payment history for DataGridView
  - Renewal report retrieval (stored proc + SQL fallback)

---

## Business Logic Services (Desktop Side)

### AuthenticationService

File: `OneFitnessVue.WinForms/Data/AuthenticationService.cs`

Implements desktop login logic by reusing the same hashing sequence used in the web app:

1. Password -> SHA256
2. Token + stored hash -> SHA256
3. Compare final hash with submitted hash

Also validates:

- user exists
- account status is active

### ValidationService

File: `OneFitnessVue.WinForms/Data/ValidationService.cs`

Reused/ported validation constraints:

- Name format validation
- Mobile format validation
- Email format validation
- Required field checks for member registration workflow

### CalculationService

File: `OneFitnessVue.WinForms/Data/CalculationService.cs`

Implements:

- `CalculateAge(DateTime dob)` (auto age from DOB)
- `CalculateTotal(decimal amount, decimal taxPercentage)`:
  - base amount
  - tax amount
  - total amount

---

## WinForms Module Conversion

## 1) Login Module

File: `OneFitnessVue.WinForms/Forms/LoginForm.cs`

Converted from web login page to desktop form:

- Username/password input fields
- Tokenized hash validation logic
- On successful auth -> opens `DashboardForm`
- On failure -> shows validation messages

---

## 2) Dashboard Module

File: `OneFitnessVue.WinForms/Forms/DashboardForm.cs`

Created a main dashboard form with navigation buttons:

- Add Member
- Search Member
- Payment
- Reports
- Logout

This replaces web menu/controller navigation with desktop form-driven navigation.

---

## 3) Member Module

File: `OneFitnessVue.WinForms/Forms/AddMemberForm.cs`

Converted member application page into desktop form.

Implemented:

- Required input fields:
  - Name, Mobile, Email, DOB, Age, MembershipType, Workout, Installment, Address, emergency details
- Age auto-calculation on DOB change
- Duplicate mobile and email checks
- Tax and total amount display based on selected membership/tax
- Save member and initial payment transactionally

Data persisted to existing tables:

- `MemberRegistration`
- `PaymentDetails`

---

## 4) Payment Module

File: `OneFitnessVue.WinForms/Forms/PaymentForm.cs`

Converted payment functionality into WinForms:

- Load member by Member Number
- Select membership/workout/installment/payment type/tax
- Reuse calculation logic for amount/tax/total
- Compute period dates using installment months
- Save payment as renewal record in `PaymentDetails`

---

## 5) Search/List Module

File: `OneFitnessVue.WinForms/Forms/SearchMembersForm.cs`

Converted member listing/search into DataGridView:

- Search by Member Number or First Name
- Display member records in grid
- Uses repository query logic against same DB structure

---

## 6) Reports Module

File: `OneFitnessVue.WinForms/Forms/ReportsForm.cs`

Converted report behavior into desktop tabs and grids:

- Renewal report:
  - date range filters
  - DataGridView result set
- Payment history:
  - search field
  - DataGridView listing

This replaces web report pages with desktop report screens.

---

## Supporting WinForms Models Added

Files under `OneFitnessVue.WinForms/Models`:

- `LookupItem.cs`
- `UserSession.cs`
- `MemberFormModel.cs`
- `TaxCalculationResult.cs`

Purpose:

- Keep form-specific state/data clean
- Avoid modifying shared domain entities for UI-specific needs
- Maintain separation of concerns

---

## Solution Update

Updated:

- `OneFitnessVueSolution.sln`

Change made:

- Replaced web presentation project mapping with WinForms presentation project:
  - Removed `OneFitnessVue.Web` from solution project list
  - Added `OneFitnessVue.WinForms`

This aligns the solution to desktop presentation as requested.

---

## Separation of Concerns Achieved

- **UI Layer:** `OneFitnessVue.WinForms/Forms`
- **Data Access Layer:** `DatabaseHelper`, `GymRepository`
- **Business Logic Layer (desktop-side):** `AuthenticationService`, `ValidationService`, `CalculationService`
- **Domain Models:** reused from existing `OneFitnessVue.Model`

---

## Notes and Assumptions

- Existing database schema remains unchanged.
- Existing stored procedure `Usp_GetNewInvoiceId` is used when available.
- A fallback SQL invoice strategy is present for robustness.
- Tax options include GST and VAT for calculation continuity in the desktop flow.

---

## Final Outcome

The project now has a WinForms desktop presentation layer with:

- Login
- Dashboard
- Add Member
- Search Members
- Payment
- Reports

All connected to the existing SQL Server schema, with reusable logic preserved and ASP.NET-specific UI/controller flow replaced by WinForms event-driven behavior.
