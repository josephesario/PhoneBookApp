#PhoneBookApp Documentation

Welcome to the official documentation for PhoneBookApp, a robust application that seamlessly integrates ASP.NET Core with Angular to streamline your contact management experience.
Getting Started

Follow these steps to set up and run the PhoneBookApp project on your system.
Prerequisites

Ensure that the following prerequisites are installed on your system:

    .NET 7
    SQL Server

Database Setup

    Navigate to the Extra.Info folder in the main project.
    Execute the SQL query found in the database_query.sql file to create the necessary database.

Connection String Configuration

    Open the main project.
    Locate the appsettings.json file.
    In this file, find various settings, including the connection string.
    Set the connection string according to your database configuration.

json

{
  "ConnectionStrings": {
    "PhoneBookDbContext": "YourConnectionStringHere"
  },
  // Other settings...
}

Project Structure

The PhoneBookApp project is organized into four main parts:

    DbContext: Manages database interactions and design.
    Helpers: Provides structured JSON file handling in Swagger.
    Contracts: Contains interfaces for various functionalities.
    PhoneBookApp (Main Project): Houses the API (in the Controllers folder) and the Angular front-end (in the ClientApp folder).

Building and Running the Project

    Build the project using Visual Studio 2022.
    Run the project to start the application.

Note: If you encounter any issues during setup, ensure that you have met all the prerequisites and correctly configured the connection string.
Exploring the Project

Feel free to explore the API endpoints in the Controllers folder and the Angular front-end in the ClientApp folder.

Done With Test, GetINNOtized.

Best regards,

Jose Rodolfo Esapa Riochi
