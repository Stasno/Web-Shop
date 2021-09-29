# Web-Shop

My implementation of web shop. 
I used this project to learn how to build a web server with client for it, and learn new technologies.

# Client

Implemented the logic and user interface
* shopping cart management
* order creation / cancellation
* catalog navigation
* user registration / login
* adding a product to the cart

<br />

#### Frameworks - Packages used

* [React](https://ru.reactjs.org/)
* [React-bootstrap](https://react-bootstrap.github.io/)

# Server

Contains api for catalog, cart, order, user.

[How to see the api of the project.](#SWAGGER-UI)

#### Frameworks - Packages used.

* [Asp.net core 5.0](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/intro).
* [Entity framework 5.0](https://docs.microsoft.com/ru-ru/ef/).
* [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore).

# How to run

1. [Download and install the .NET Core SDK](https://dotnet.microsoft.com/download).

  * if you don't have MSSQL on your PC, [Download and install Microsoft SQL SERVER 2019 EXPRESS](https://www.microsoft.com/ru-RU/download/details.aspx?id=101064).
  
2. [Download and install Node.js](https://nodejs.org/en/).
  
3. Open the solution in Visual studio 2019 
4. Open Package Manager Console and run following commands (to create database).

```
  cd Database
  Update-database
```
5. Open the terminal and run following commands (to install react dependencies).

```
  cd ./WebShop/ClientApp
  npm install
```
6. Build and run WebShop project.

# SWAGGER UI

Shows api of the project

#### Hot to open SWAGGER UI.

1. Build and run the project 
2. Open page ```https://localhost:44395/swagger ```
