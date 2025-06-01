# Domain & Hosting Management System

A comprehensive web application built with ASP.NET Core for managing domain registrations and hosting packages across multiple companies. This system provides a centralized platform for businesses to track their digital assets and offer hosting services to their clients.

## 🚀 Features

### Domain Management
- **Domain Registration Tracking**: Keep track of all registered domains with purchase and expiry dates
- **Multi-Company Support**: Manage domains across different companies and organizations
- **Status Monitoring**: Track active/inactive domain status
- **Pricing Management**: Monitor domain costs and pricing history
- **Real-time API**: RESTful endpoints for dynamic data retrieval

### Hosting Package Management
- **Package Creation**: Define hosting packages with storage, bandwidth, and pricing
- **Company-Based Packages**: Each company can create and manage their own hosting offerings
- **Package Purchasing**: Built-in purchasing system for hosting packages
- **Status Management**: Enable/disable packages based on availability
- **Resource Allocation**: Track storage (GB) and bandwidth (GB) allocations

### Authentication & Authorization
- **Role-Based Access**: Secure access control based on user roles
- **Company Association**: Users are associated with specific companies
- **Claims-Based Security**: JWT/Cookie-based authentication
- **Multi-Tenant Architecture**: Isolated data access per company

## 🏗️ Architecture

### Technology Stack
- **Backend**: ASP.NET Core 6.0+
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Razor Views with dynamic JavaScript
- **API**: RESTful Web API endpoints

### Database Schema

```
Companies
├── Id (Primary Key)
├── Name
├── CreatedAt
└── Status

Domains
├── Id (Primary Key)
├── DomainName
├── Price
├── PurchaseDate
├── ExpiryDate
├── IsActive
└── CompanyId (Foreign Key)

HostingPackages
├── Id (Primary Key)
├── Name
├── StorageGB
├── BandwidthGB
├── Price
├── Status
├── Description
├── CompanyId (Foreign Key)
└── CreatedAt

Persons (Users)
├── Id (Primary Key)
├── Name
├── Email
├── CompanyId (Foreign Key)
└── Authentication Data
```

## 🔧 Installation & Setup

### Prerequisites
- .NET 6.0 SDK or later
- SQL Server (LocalDB or Full Instance)
- Visual Studio 2022 or VS Code

### Getting Started

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/domain-hosting-management.git
   cd domain-hosting-management
   ```

2. **Configure Database Connection**
   ```json
   // appsettings.json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DomainHostingDB;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Run Database Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Build and Run**
   ```bash
   dotnet build
   dotnet run
   ```

5. **Access the Application**
   - Navigate to `https://localhost:5001` or `http://localhost:5000`

## 📋 Business Logic

### Domain Management Workflow
1. **Domain Registration**: Companies can register new domains with purchase details
2. **Expiry Tracking**: System monitors domain expiry dates for renewal alerts
3. **Status Management**: Domains can be activated/deactivated based on business needs
4. **Cost Analysis**: Track domain costs and ROI across different companies

### Hosting Package Workflow
1. **Package Creation**: Companies define hosting packages with resource allocations
2. **Pricing Strategy**: Set competitive pricing based on storage and bandwidth
3. **Package Purchase**: Customers can purchase hosting packages through the system
4. **Resource Monitoring**: Track usage and availability of hosting resources

### Multi-Tenant Security
- **Company Isolation**: Each company only sees their own domains and hosting packages
- **User Association**: Users are bound to specific companies via CompanyId claims
- **Role-Based Permissions**: Different access levels for administrators and regular users

## 🔌 API Endpoints

### Domains API
```
GET    /Domain/api/recent          - Get recent domains for company
POST   /Domain/create              - Create new domain registration
PUT    /Domain/update/{id}         - Update domain information
DELETE /Domain/delete/{id}         - Remove domain registration
```

### Hosting API
```
GET    /Hosting                    - Get hosting packages for company
POST   /Hosting/BuyPackage/{id}    - Purchase hosting package
GET    /Hosting/api/packages       - Get available packages
POST   /Hosting/create             - Create new hosting package
```

## 🎯 Use Cases

### For Web Hosting Companies
- Manage multiple client domains from a single dashboard
- Create and sell hosting packages with different resource allocations
- Track domain renewals and hosting subscriptions
- Generate reports on revenue and resource usage

### For Digital Agencies
- Track client domain portfolios
- Manage hosting requirements for client projects
- Monitor domain expiry dates for proactive renewal
- Centralized billing and cost management

### For Enterprise Organizations
- Manage corporate domain assets
- Track hosting costs across departments
- Ensure domain security and compliance
- Streamline IT asset management

## 🛠️ Development Guidelines

### Code Structure
```
WebApplication2/
├── Controllers/
│   ├── DomainsController.cs      - Domain management endpoints
│   └── HostingController.cs      - Hosting package management
├── Models/
│   ├── Domain.cs                 - Domain entity model
│   ├── HostingPackage.cs         - Hosting package model
│   └── Company.cs                - Company entity model
├── Views/
│   ├── Domains/                  - Domain management views
│   └── Hosting/                  - Hosting package views
└── Data/
    └── AppDbContext.cs           - Entity Framework context
```

