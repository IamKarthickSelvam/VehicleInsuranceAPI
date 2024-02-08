# Vehicle Insurance API
Backend API of the Vehicle Insurance application used for fetching vehicle data, static images uri, performing calculations for determining premium for different vehicle and insurance types and Invoice PDF generation for [Vehicle Insurance Client](https://github.com/IamKarthickSelvam/VehicleInsuranceClient).

The API is written in C# using ASP.NET Core with Entity Framework Core as the ORM to interact with the SQLite Database which stores the Vehicle data. 

Live Web App Link: [wonderful-wave-002693203.4.azurestaticapps.net](https://wonderful-wave-002693203.4.azurestaticapps.net/)

Backend API Link: [vehicleinsuranceapi.azurewebsites.net](https://vehicleinsuranceapi.azurewebsites.net/)

## Tech Stack
ASP.NET Core 7.0 | Azure SDK | Entity Framework Core | SQLite

Utilizes the following Azure services:
* Azure Web App for hosting the backend on the cloud
* Azure Blob Storage for storing the invoice template document and static images used throught the web app
* Azure Cache for Redis for high performance in-memory caching

## Video Demo
https://github.com/IamKarthickSelvam/VehicleInsuranceAPI/assets/102350733/4a33a9b1-cd5e-49cb-942e-2f044cfcb9c0
