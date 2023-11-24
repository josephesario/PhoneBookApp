#PhoneBookApp

Welcome to the documentation for PhoneBookApp, a combined ASP.NET Core and Angular project designed to manage your contacts effectively. Follow the steps below to set up and run the project.
Prerequisites

#Ensure you have the following prerequisites installed on your system:

    .NET 7
    SQL Server

#Database Setup

    Navigate to the Extra.Info folder in the main project.
    Execute the SQL query in the database_query.sql file to create the necessary database.

#Connection String Configuration

    Open the main project and locate the appsettings.json file.
    In this file, you will find various settings, including the connection string.
    Set the connection string according to your database configuration.

json

{
  "ConnectionStrings": {
    "PhoneBookDbContext": "YourConnectionStringHere"
  },
  // Other settings...
}

Project Structure

#The PhoneBookApp project is structured into four main parts:

    DbContext: Manages the database interactions and design.
    Helpers: Provides structured JSON file handling in Swagger.
    Contracts: Contains interfaces for various functionalities.
    PhoneBookApp (Main Project): Houses the API (in the Controllers folder) and the Angular front-end (in the ClientApp folder).

#Building and Running the Project

    Build the project using Visual Studio 2022.
    Run the project to start the application.

Note: If you encounter any issues during setup, ensure that you have met all the prerequisites and correctly configured the connection string.

Feel free to explore the API endpoints in the Controllers folder and the Angular front-end in the ClientApp folder.

Done With Test, Hope Is Up to requirements.

Jose Rodolfo Esapa Riochi
