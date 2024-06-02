# Cornershop - Nashtech Rookie To Engineer Batch 7
An online bookstore project for midterm assignment by Binh Dang Cong.

This project that consists of three main components:
1. **Web API** - Backend API to handle data operations and business logic.
2. **Web MVC for Customer** - Frontend application for customer interactions.
3. **ReactJS for Admin** - Admin dashboard built with ReactJS for managing the platform.

## Table of Contents
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Applications](#running-the-applications)
- [Project Structure](#project-structure)
- [License](#license)

## Features

### Web API
- RESTful endpoints for categories, products, orders, customers, and admin operations.
- JWT Authentication.
- Data validation and error handling.

### Web MVC for Customer
- User registration and authentication.
- Browse and search for products.

### ReactJS for Admin
- Product management (CRUD operations).
- User management.

## Tech Stack
- **Backend**: .NET Core Web API
- **Frontend (Customer)**: ASP.NET MVC
- **Frontend (Admin)**: ReactJS
- **Database**: SQL Server

## Getting Started

### Prerequisites
- .NET SDK 8.0+
- Node.js 21+
- SQL Server
- Git

### Installation

1. **Clone the repository:**
    ```bash
    git clone https://github.com/congbinh75/Cornershop.git
    cd Cornershop
    ```

2. **Setup the Database:**
   - Ensure SQL Server is running.
   - Update the connection strings in `appsettings.json` for Web API project.
   - Update the base address in `appsettings.json` for Web MVC project.
   - Add and .env file to ReactJS project and add the VITE_ENV_BASE_URL key value pair.

3. **Restore Dependencies:**
   - For Web API and Web MVC:
     ```bash
     cd ..
     dotnet restore
     ```
   - For Admin ReactJS:
     ```bash
     cd ../Cornershop.Presentation.Admin
     npm install
     ```

### Running the Applications

1. **Start the Web API:**
   ```bash
   cd ../Cornershop.Service.Application
   dotnet run
2. **Start the Web MVC for Customer:**
   ```bash
   cd ../Cornershop.Presentation.Customer
   dotnet run
   ```
3. **Start the ReactJS Admin App:**
   ```bash
   cd ../Cornershop.Presentation.Admin
   npm start
   ```

### Project Structure
    .
    Cornershop/
    │
    ├── Cornershop.Presentation.Admin/        # ReactJS project for admin dashboard
    ├── Cornershop.Presentation.Customer/     # ASP.NET MVC project for customers
    ├── Cornershop.Service.Application/       # ASP.NET Web API project as portal to consume API
    ├── Cornershop.Service.Domain/            # .NET library project for processing business logic
    ├── Cornershop.Service.Infrastructure/    # .NET library project for managing entities and database connection
    ├── Cornershop.Service.Common/            # .NET library project for managing resources for the API
    ├── Cornershop.Shared/                    # .NET library project for managing states between API and Presentation app
    │
    ├── LICENSE                               # GNU 3.0 License
    └── README.md                             # Project README file
    

### License
This project is licensed under the GNU 3.0 License. See the [LICENSE](LICENSE) file for details.
