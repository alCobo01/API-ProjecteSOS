# API-ProjecteSOS
## Objective
The API-ProjecteSOS provides a RESTful service to manage dishes, ingredients, and menus for a restaurant application. It supports user registration and login, role-based access control (Admin, User), JSON data import, and dynamic menu generation based on available ingredients.

## Technologies
- ASP.NET Core Web API (C# .NET 9)
- Entity Framework Core
- Microsoft SQL Server
- ASP.NET Identity for user management
- JWT (JSON Web Tokens) for authentication
- Swagger / OpenAPI for API documentation

## Endpoints
### Authentication
- POST `/api/auth/register` : Register a new user (requires secret key)
- POST `/api/auth/admin/register` : Register a new admin user (requires admin secret key)
- POST `/api/auth/login` : Login with email and password, returns JWT

### Dishes
- GET `/api/dish` : Get all dishes (Authenticated)
- GET `/api/dish/{id}` : Get a specific dish by ID (Authenticated)
- POST `/api/dish` : Create a new dish (Admin only)
- PUT `/api/dish/{id}` : Update an existing dish (Admin only)
- DELETE `/api/dish?id={id}` : Delete a dish by ID (Admin only)

### Ingredients
- GET `/api/ingredient` : Get all ingredients (Authenticated)
- GET `/api/ingredient/{id}` : Get a specific ingredient by ID (Authenticated)
- POST `/api/ingredient` : Create a new ingredient (Admin only)
- PUT `/api/ingredient/{id}` : Update an existing ingredient (Admin only)
- DELETE `/api/ingredient?id={id}` : Delete an ingredient by ID (Admin only)

### Ingredient Names
- GET `/api/ingredientName` : Get all ingredient names (Authenticated)

### JSON Import
- POST `/api/json` : Import ingredients from a JSON file (Admin only, multipart/form-data with `jsonFile`)

### Menu
- GET `/api/menu` : Get the current menu with dishes and available ingredients (Authenticated)

### Root
- GET `/` : Welcome message and basic info
