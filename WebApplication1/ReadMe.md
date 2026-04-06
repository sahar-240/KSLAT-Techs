# Natural Gallery - Museum Tour Management

A web application for the Natural Gallery museum, built with ASP.NET Core MVC and connected to an Azure SQL database.

## How to Compile and Run

### Prerequisites
- Visual Studio 2022 (or later) with the **ASP.NET and web development** workload
- .NET 10 SDK
- Access to the Azure SQL database (connection string is in `Program.cs`)

### Steps
1. Clone the repository and switch to the `Louisa` branch:
   ```
   git clone https://github.com/sahar-240/KSLAT-Techs.git
   cd KSLAT-Techs
   git checkout Louisa
   ```
2. Open `WebApplication1.sln` in Visual Studio.
3. Restore NuGet packages (Visual Studio does this automatically on open).
4. Build the solution: **Build → Build Solution** (Ctrl+Shift+B).
5. Run the project: **Debug → Start Without Debugging** (Ctrl+F5).
6. The site opens at `https://localhost:xxxx`.

### Database Setup
The application connects to an Azure SQL database. On first run, `DbSeeder.cs` seeds the Events and OpeningHours tables automatically if they are empty.

To manually create or reset the database tables, run the SQL script located at:
```
WebApplication1/SQL/DatabaseSetup.sql
```
Open the Azure portal → SQL Database → Query Editor, paste the script, and execute.

## Project Structure

| Folder / File | Purpose |
|---|---|
| `Controllers/` | MVC controllers handling HTTP requests |
| `Models/` | C# entity classes mapping to SQL tables |
| `Views/` | Razor (.cshtml) templates for each page |
| `Data/MuseumDbContext.cs` | Entity Framework database context |
| `Data/DbSeeder.cs` | Seeds initial event and opening hour data |
| `SQL/DatabaseSetup.sql` | SQL script to create all database tables |
| `wwwroot/css/` | Stylesheets for each page |
| `wwwroot/images/` | Event banner images and site graphics |
| `Program.cs` | Application startup and service configuration |

## Sections

- **Events** – Browse, filter, and sort exhibitions and workshops
- **Event Booking** – Select a date, time, and quantity to book tickets
- **Contact** – Submit enquiries with newsletter opt-in, saved to SQL
- **Favourites** – Save events to a favourites list (requires login)
- **Tickets** – View booking history on the membership page (requires login)
- **Search** – Navbar search bar to find events by keyword

## Technologies Used

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core (Code-First approach)
- Azure SQL Database
- HTML5, CSS3, JavaScript
- Font Awesome icons
- QRCode.js (ticket QR code generation)