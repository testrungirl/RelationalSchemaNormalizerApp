# RelationalSchemaNormalizerApp Installation Guide
## System Requirements
### Minimum Hardware Specifications

- Processor: Intel Core i5 or AMD equivalent, 2.0 GHz or higher
RAM: 8GB
- Storage: 100GB available disk space
- Display: 1920x1080 resolution or higher

### Software Requirements

- Operating System: Windows 10 or higher (64-bit)
.NET 8.0 SDK
- Visual Studio 2022 (any edition)
- SQL Server 2022 Express
- Graphviz

## Pre-Installation Steps

- Ensure your system meets the minimum hardware requirements.
- Update your Windows operating system to the latest version.

## Installation Process
1. Install Required Software
   - Download and install .NET 8.0 SDK from the official Microsoft website.
   - Download and install Visual Studio 2022 from the official Microsoft website.
During installation, ensure the ".NET desktop development" workload is selected.
   - Download and install SQL Server 2022 Express from the official Microsoft website.
   - Download and install Graphviz from https://graphviz.org/download/
After installation, add the Graphviz bin directory to your system's PATH environment variable.

2. Open the Solution
   - Open Visual Studio 2022 and select "Open a project or solution". Navigate to the extracted repository and open RelationalSchemaNormalizerApp.sln.

3. Restore NuGet Packages - In Visual Studio:
   - Right-click on the solution in Solution Explorer
   - Select "Restore NuGet Packages"

4. Configure the Database Connection
   - Open the appsettings.json file in the RelationalSchemaNormalizerUI project.
   - Locate the ConnectionStrings section and update the DefaultConnection string if necessary.

5. Apply Database Migrations
   - In Visual Studio, open the Package Manager Console and ensure the Default Project is set to RelationalSchemaNormalizerUI. Then run:
   - Add-Migration InitialCreate
   - Update-Database

6. Build the Solution
   - In Visual Studio, select "Build" > "Build Solution" from the menu.
   - Ensure there are no build errors.

Running the Application

Set RelationalSchemaNormalizerUI as the startup project.
 select "Debug" > "Start Debugging" from the menu to run the application with debugging.

Troubleshooting
If you encounter any issues during installation or setup:

Ensure all prerequisites are correctly installed.
Check that your system meets the minimum requirements.
Verify that all connection strings and paths are correct.
Consult the project's documentation or reach out to the support team for assistance.
