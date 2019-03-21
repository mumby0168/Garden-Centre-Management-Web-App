# Garden-Centre-Management-Web-App
This was a project made as part of a team to manage a garden centre.
# Getting Started
This application uses an Entity Framework database which is not currently running on a sever. In order to setup the database using a local
instance of SQL Express which comes as part Visual Studio please follow the instructions below:
1. Clone the repository.
2. Open the project in Visual Studio 2017+
3. Open the package manager console.
4. Run Enable-Migrations -Force to re-create the database.
5. Run Update-Database (this will call the seed method and a admin user)
6. Start the Application.
7. Click the link labelled "Dont have an account, Create one here" located under the login form.

A user has been set up by seeding the database this will allow you to use the following details in the system:

- Employee Number: 123456
- Username: joebloggs@outlook.com

Using these details will allow you to set your own password. After setting this you can then login into the system using the email, employee number and newly set password.

Once inside the application you can then go to the employees section and add new emplpyees as your are a admin user. This allows them to then follow the same process to create an account. A account can only be created if it has been setup first by a admin user.
