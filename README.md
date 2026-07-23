# FinancialSystem

A multi-bank financial management system built with ASP.NET Core. Supports user accounts, loans, installment plans, inter-account transfers, salary projects, and role-based dashboards (Client, Operator, Manager, Enterprise Specialist, Administrator).

---

## Tech Stack

| Layer | Technology |
|---|---|
| **Framework** | .NET 9 (ASP.NET Core MVC) |
| **Database** | PostgreSQL 18 |
| **ORM** | Entity Framework Core 9 |
| **Auth** | JWT (cookie-based) |
| **Logging** | Serilog (console + file sink) |
| **Architecture** | Clean Architecture (Domain, Application, Infrastructure, Web) |

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/)

### 1. Clone & restore

```bash
git clone https://github.com/Dmitro44/FinancialSystem.git
cd FinancialSystem
dotnet restore
```

### 2. Start the database

```bash
docker compose up -d
```

This starts PostgreSQL on port `5432` — the connection string in `FinancialSystem.Web/appsettings.json` is already configured for it.

### 3. Apply migrations

```bash
dotnet ef database update \
    --project FinancialSystem.Infrastructure \
    --startup-project FinancialSystem.Web
```

### 4. Run

```bash
dotnet run --project FinancialSystem.Web
```

The app starts at `http://localhost:5221` (or `https://localhost:7258`).

### 5. Register & log in

Navigate to the app, register a new user, then register to a bank with the desired role to access the corresponding dashboard.

---

## API Overview

The application is an **MVC web app** (not a pure REST API). All routes return HTML views. The default route is `{controller=Auth}/{action=Login}/{id?}`.

### Auth & Home

| Route | Description |
|---|---|
| `GET /` or `Auth/Login` | Login page |
| `POST Auth/Login` | Authenticate, set JWT cookie |
| `GET Auth/Register` | Registration page |
| `POST Auth/Register` | Create a new user |
| `GET Auth/Logout` | Clear session |

### Bank Hub

| Route | Description |
|---|---|
| `GET User/Banks` | List all banks (registered + available to join) |
| `POST Bank/RegisterToBank` | Join a bank with a chosen role |
| `GET Bank/RedirectToDashboard/{bankId}` | Route to the correct role dashboard |
| `GET Bank/Requests/{bankId}` | Pending loan & installment requests (Manager view) |
| `GET Bank/Finances/{bankId}` | User's accounts, loans, installments (Client view) |
| `GET Bank/TransferStatistics/{bankId}` | Transfers for a bank (Operator/Manager) |
| `GET Bank/SalaryProjectRequests/{bankId}` | Salary project requests (Operator/Manager) |

### Client Dashboard

| Route | Description |
|---|---|
| `GET Client/ClientDashboard/{bankId}` | Client dashboard home |
| `POST Client/CreateAccount` | Open a new bank account |
| `POST Client/CreateLoan` | Submit a loan request |
| `POST Client/CreateInstallment` | Submit an installment request |
| `GET Client/Transfer/{bankId}` | Transfer form with user's accounts |
| `POST Client/CreateTransfer` | Execute a money transfer |
| `GET Client/AvailableSalaryProjects/{bankId}` | List salary projects available to join |
| `POST Client/ConnectToSalaryProject` | Join a salary project |
| `POST Client/DisconnectFromSalaryProject` | Leave a salary project |

### Manager Dashboard

| Route | Description |
|---|---|
| `GET Manager/ManagerDashboard/{bankId}` | Manager dashboard home |
| `POST Manager/ApproveLoan` | Approve a loan request |
| `POST Manager/RejectLoan` | Reject a loan request |
| `POST Manager/ApproveInstallment` | Approve an installment |
| `POST Manager/RejectInstallment` | Reject an installment |
| `POST Manager/CancelTransfer/{transferId}` | Cancel a transfer |
| `POST Manager/ApproveSalaryProject` | Approve a salary project |
| `POST Manager/RejectSalaryProject` | Reject a salary project |

### Operator Dashboard

| Route | Description |
|---|---|
| `GET Operator/OperatorDashboard/{bankId}` | Operator dashboard home |
| `POST Operator/CancelTransfer/{transferId}` | Cancel a transfer |
| `POST Operator/ApproveSalaryProject` | Approve a salary project |
| `POST Operator/RejectSalaryProject` | Reject a salary project |

### Enterprise Specialist Dashboard

| Route | Description |
|---|---|
| `GET EnterpriseSpecialist/EnterpriseSpecialistDashboard/{bankId}` | Dashboard home |
| `GET EnterpriseSpecialist/EnterpriseAccounts/{bankId}` | List enterprise accounts |
| `POST EnterpriseSpecialist/CreateEnterpriseAccount` | Create an enterprise account |
| `GET EnterpriseSpecialist/SalaryProjects/{bankId}` | List salary projects |
| `POST EnterpriseSpecialist/CreateSalaryProject` | Create a salary project |
| `POST EnterpriseSpecialist/ProcessProjects/{bankId}/{salaryProjectId}` | Process payroll |

### Administrator Dashboard

| Route | Description |
|---|---|
| `GET AdministratorDashboard/{bankId}` | Admin dashboard home |
| `GET Admin/ShowOperations` | View operation audit log |
| `POST Admin/RevertOperation` | Undo an operation |
| `POST Admin/RestoreOperation` | Redo a reverted operation |

### Financial Calculators (JSON)

| Route | Description |
|---|---|
| `POST Loan/CalculateTotalAmount` | Calculate loan totals (`{totalAmount, interestRate, monthlyPayment}`) |
| `POST Installment/CalculateMonthlyPayment` | Calculate installment payment |

---

## Project Structure

```
FinancialSystem/
├── FinancialSystem.Domain/          # Entities, enums, interfaces
├── FinancialSystem.Application/     # Services, use cases, DTOs
├── FinancialSystem.Infrastructure/  # EF Core, repositories, migrations
├── FinancialSystem.Web/             # MVC controllers, views, auth
├── FinancialSystem.sln
└── README.md
```

Clean Architecture: Domain has no dependencies, Application depends on Domain, Infrastructure depends on Application, Web depends on Infrastructure and Application.

---

## Migrations

All migrations live in `FinancialSystem.Infrastructure/Migrations/`.

```bash
# Create a new migration
dotnet ef migrations add MigrationName \
    --project FinancialSystem.Infrastructure \
    --startup-project FinancialSystem.Web

# Apply pending migrations
dotnet ef database update \
    --project FinancialSystem.Infrastructure \
    --startup-project FinancialSystem.Web
```

---

## License

MIT
