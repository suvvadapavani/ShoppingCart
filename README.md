#  Simple E-Commerce Application

A basic e-commerce web application built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**.  
This project demonstrates core full-stack development concepts including authentication, authorization, session management, CRUD operations, and validation.

---

## Features

- **Authentication & Authorization**
  - ASP.NET Core Identity implementation
  - Login, Logout, Role-based permissions (Admin, User)
- **Cart Management**
  - Session-based shopping cart
- **CRUD Operations**
  - Products, Categories, Users
- **Validation**
  - Client-side validation with Bootstrap & jQuery
  - Server-side validation with Data Annotations
- **State Management**
  - ViewBag, ViewData, TempData usage
- **Database**
  - SQL Server with EF Core
  - Code-first migrations
- **UI**
  - Bootstrap for responsive design

---

##  Tech Stack

- **Backend:** ASP.NET Core MVC (C#)
- **Database:** SQL Server + EF Core
- **Authentication:** ASP.NET Core Identity
- **Frontend:** Bootstrap, jQuery
- **ORM:** Entity Framework Core

---

## 📂 Project Structure
ShoppingCart.DataAccess
  Data       //ef dore db context
  Migrations //EF core migrations

ShoppingCart.Models
   Models     //Entities and View models
   
ShoppingCart.Utility
  SD.cs

ShoppingCart
  Controllers //MVC controllers 
  Views       //Razor views
  wwwroot     //Statis files(css,js,images)
  Program.cs //Middleware ,dependency injection


## ⚙️ Getting Started

### Prerequisites
- .NET 8 SDK (or compatible version)
- SQL Server
- Visual Studio / VS Code

### Setup
1. Clone the repository:
   ```bash
   git clone
   https://github.com/suvvadapavani/ShoppingCart.git

