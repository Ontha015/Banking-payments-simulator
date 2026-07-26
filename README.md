# Banking & Payments Simulator

A full-stack banking application built to simulate real-world banking and payments workflows including user accounts, transactions, fraud detection and admin review tools. 

## Overview

This project simulates the core workflows of a banking/payments platform:

- Users can register and log in securely
- Users can hold multiple accounts and perform deposits, withdrawals and transfers
- Transactions are automatically screened by a fraud-detection rule engine
- Suspicious transactions are flagged and routed to an admin review queue
- Admins can approve or block flagged transactions, with account balances correctly reversed on block

The goal was to go beyond a simple CRUD app and build something that reflects the kind of business logic, security and data integrity concerns found in real financial systems.

## Features

**Authentication & Security**
- User registration with BCrypt password hashing
- JWT-based login and session authentication
- Protected API routes requiring a valid token

**Core Banking Logic**
- Multiple accounts per user (Savings, Cheque)
- Deposit and withdraw with balance validation (no overdrawing, no negative amounts)
- Transfers between accounts with linked debit/credit transaction records

**Fraud Detection**
- Automatic flagging of transfers exceeding 80% of the sender's account balance
- Flagged transactions are blocked from silently completing and routed for review

**Admin Review Workflow**
- Dashboard listing all flagged transactions
- Approve or block each transaction
- Blocking a transaction correctly reverses its effect on the account balance

**Frontend**
- React (Vite) single-page application
- Live user list pulled from the API
- Forms for creating users and logging in
- Admin panel for reviewing flagged transactions

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | React, Vite |
| Backend | ASP.NET Core Web API (.NET 10) |
| Database | MySQL |
| ORM | Entity Framework Core |
| Auth | JWT, BCrypt |
| Testing | Postman (manual end-to-end testing) |

## Architecture

```
banking-frontend/        React frontend (Vite)
BankingSimulator/         ASP.NET Core Web API
 ├── Controllers/         API endpoints (Users, Accounts, Transactions, Auth)
 ├── Models/               Entity classes (User, Account, Transaction)
 ├── Data/                 EF Core DbContext
 ├── Services/             TokenService (JWT generation)
 └── Migrations/           EF Core database migrations
```

## API Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| POST | `/api/users` | Create a new user | No |
| GET | `/api/users` | List all users | No |
| GET | `/api/users/{id}` | Get a single user | No |
| POST | `/api/auth/login` | Log in, returns JWT token | No |
| GET | `/api/accounts` | List all accounts | Yes |
| GET | `/api/accounts/user/{userId}` | Get accounts for a user | Yes |
| POST | `/api/accounts` | Create an account | Yes |
| POST | `/api/transactions/deposit` | Deposit into an account | No |
| POST | `/api/transactions/withdraw` | Withdraw from an account | No |
| POST | `/api/transactions/transfer` | Transfer between accounts | No |
| GET | `/api/transactions/flagged` | List flagged transactions | Yes |
| PUT | `/api/transactions/{id}/review` | Approve or block a flagged transaction | Yes |


### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org) (LTS)
- MySQL Server + MySQL Workbench

### Backend Setup
cd BankingSimulator

1. Update the connection string in `appsettings.json` with your MySQL credentials
2. Run the migrations:

dotnet ef database update

3. Run the API:

dotnet run


### Frontend Setup

cd banking-frontend
npm install
npm run dev


The frontend runs at `http://localhost:5173` and expects the API at `https://localhost:7263`.

## What I Learned

Building this project involved real debugging beyond tutorial-following, resolving NuGet package version conflicts between EF Core and MySQL connectors, fixing a live compatibility bug between .NET 10's OpenAPI tooling and a security-patched dependency, correcting service registration order in `Program.cs` and implementing fraud logic that had to correctly reverse financial state, not just flag a status. These are the kinds of problems that come up in real backend development not just guided exercises.

## Future Improvements

- Automated unit and integration tests
- Deployment to Azure
- CI/CD pipeline with GitHub Actions
- More granular fraud rules (time-of-day, transaction velocity)
- Refined UI/UX for the frontend

## Author

**Onthatile Letsatsi** — IT student (Software Development) at Rosebank International.
