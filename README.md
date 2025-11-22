# SmartTickets -- Support Ticket System

SmartTickets is a practical full‑stack demo project built with ASP.NET
Core on the backend and React + TypeScript on the frontend.\
The goal was to create a clean, simple system that demonstrates solid
structure, separation between layers, and real‑world features such as
authentication, email notifications, and AI‑generated summaries.

------------------------------------------------------------------------

## Features

### Core Functionality

-   Create new support tickets\
-   View all tickets in a table\
-   Filter by status\
-   Search by name or description\
-   Ticket details screen\
-   Clean UI styled with TailwindCSS

### Admin Features

-   Login with JWT authentication\
-   Update ticket status\
-   Add or edit resolution\
-   Admin‑only API routes\
-   Visual admin controls on the frontend

### Additional Features

-   Email notifications on status/resolution updates\
-   AI summary generation using Google Gemini (gemini‑2.5‑flash)\
-   Guest mode (view + create tickets without login)

------------------------------------------------------------------------

## Tech Stack

### Backend

-   ASP.NET Core Minimal API
-   C# / .NET 8
-   DTOs + Entities + Services + Repository pattern
-   JWT Authentication
-   Email sending (SMTP)
-   AI Summary Service (Gemini API)

### Frontend

-   React + TypeScript
-   Vite
-   TailwindCSS
-   React Router
-   AuthContext for token handling
-   Protected routes

------------------------------------------------------------------------

## Folder Structure

### Backend

    SupportTickets.Api/
      Endpoints/
      Services/
      Repositories/
      Models/
      Program.cs
      appsettings.json

### Frontend

    support-tickets-ui/
      src/
        api/
        auth/
        components/
        pages/
        types/
        utils/
      index.css
      package.json

------------------------------------------------------------------------

## Running the Project

### Backend

    cd SupportTickets.Api
    dotnet restore
    dotnet run

Backend runs on:

    http://localhost:5079

Add your Gemini API key in `appsettings.json`:

    "AI": {
      "ApiKey": "YOUR_KEY_HERE"
    }

### Frontend

    cd support-tickets-ui
    npm install
    npm run dev

Frontend runs on:

    http://localhost:5173

------------------------------------------------------------------------

## Admin Login

    Username: admin
    Password: admin123

Guests can still view and create tickets.

