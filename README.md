# Band-Me

A full-stack modern .NET web application that helps people discover, match with, and join interest-based groups ("Bands") through an interactive questionnaire.

---

## Architecture Overview

Band-Me uses a **decoupled 3-project solution architecture** (`BandMe.slnx`) to enforce clean separation of concerns and end-to-end type safety:

```text
BandMe/
├── BandMe.slnx                  # Master .NET solution file
└── src/
    ├── BandMe.Shared/           # Shared C# class library (DTOs & validation contracts)
    ├── BandMe.Api/              # ASP.NET Core Web API, EF Core, SQLite & matching engine
    └── BandMe.Client/           # Blazor WebAssembly interactive single-page frontend
```

### How the Projects Interact
* **`BandMe.Shared`:** Contains strongly-typed Data Transfer Objects (`BandDto`, `InterestTag`) and DataAnnotations request models (`CreateBandRequest`, `QuestionnaireSubmission`). Both the backend and frontend reference this library so API contracts never drift out of sync.
* **`BandMe.Api`:** Exposes RESTful endpoints (`/api/interests`, `/api/bands`, `/api/bands/match`, `/api/bands/{id}/join`), manages relational data with **Entity Framework Core** and **SQLite** (including many-to-many relationships between bands and interests), and calculates percentage match scores based on user selections.
* **`BandMe.Client`:** A **Blazor WebAssembly** frontend that consumes the Web API via `HttpClient`, providing category-grouped interest selection, real-time match ranking, group creation with client-side validation (`EditForm`), and one-click joining.

---

## Tech Stack
* **Backend:** .NET 10, ASP.NET Core Web API, Entity Framework Core, SQLite
* **Frontend:** Blazor WebAssembly, Razor Components, Bootstrap 5
* **Language & Tooling:** C#, `.slnx` solution format, Git

---

## Getting Started Locally

### Prerequisites
* [.NET 10 SDK](https://microsoft.com)
* EF Core CLI tool (`dotnet tool install --global dotnet-ef --version 10.0.*`)

### 1. Clone the Repository
```bash
git clone https://github.com
cd BandMe
```

### 2. Apply the Database Migration
Create and seed the local SQLite database (`bandme.db`) with initial interest categories:
```bash
dotnet ef database update --project src/BandMe.Api
```

### 3. Run the Backend API & Blazor Client Side-by-Side
Open two terminal windows at the repository root:

**Terminal 1 — Start the Web API (`http://localhost:5043`):**
```bash
dotnet run --project src/BandMe.Api
```

**Terminal 2 — Start the Blazor WebAssembly Client:**
```bash
dotnet run --project src/BandMe.Client
```

Open the local URL displayed in Terminal 2 (e.g., `http://localhost:5146`) in your browser to take the interest questionnaire, view ranked matches, or create a new group.
