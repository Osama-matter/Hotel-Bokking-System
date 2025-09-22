# Hotel Booking System

## 1. Project Title and Short Summary

**Project Title:** Hotel Booking System

**Short Summary:**
The Hotel Booking System is a comprehensive web application designed to streamline the process of managing hotel rooms, bookings, and customer interactions. It addresses the common challenges faced by hotels in efficiently handling reservations, payments, and customer feedback. By providing a robust and intuitive platform, the system empowers hotels to optimize their operations, enhance customer satisfaction, and drive revenue growth.

## 2. Full List of Features

### Feature: User Authentication and Authorization

*   **Purpose:** Securely manage user access to the system, distinguishing between customers and administrative roles.
*   **User Story:** As a user, I can register an account, log in, and access features based on my assigned role (customer or admin).
*   **Implementation Details:**
    *   **Main Files:** `Controllers/AccountController.cs`, `UserApplection/ApplicationUser.cs`, `Role/RoleSeeder.cs`, `DTO/DTOLogin.cs`, `DTO/DTO_Register.cs`
    *   **Classes:** `ApplicationUser`, `AccountController`
    *   **Methods:** `Register`, `Login`
*   **API Endpoints:**
    *   **Route:** `/api/Account/register`
        *   **Method:** `POST`
        *   **Sample Request:**
            ```json
            {
              "username": "newuser",
              "email": "newuser@example.com",
              "password": "Password123!"
            }
            ```
        *   **Sample Response (Success):**
            ```json
            {
              "message": "User registered successfully!"
            }
            ```
    *   **Route:** `/api/Account/login`
        *   **Method:** `POST`
        *   **Sample Request:**
            ```json
            {
              "email": "user@example.com",
              "password": "Password123!"
            }
            ```
        *   **Sample Response (Success - JWT token):**
            ```json
            {
              "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
              "expiration": "2025-09-22T12:00:00Z"
            }
            ```
*   **Database Tables or Entities Used:** `ApplicationUser` (extended IdentityUser), `ApplicationRole` (extended IdentityRole)
*   **Demo Steps:**
    1.  Send a POST request to `/api/Account/register` with valid user details.
    2.  Send a POST request to `/api/Account/login` with the registered user's credentials.
    3.  Verify the JWT token is returned.
*   **Challenges Solved or Special Techniques Used:** Implemented ASP.NET Core Identity for robust user management and JWT for stateless authentication. Role-based access control is integrated to differentiate user permissions.

### Feature: Room Management

*   **Purpose:** Allow administrators to add, view, update, and delete hotel room details, including images and descriptions.
*   **User Story:** As an administrator, I can manage the inventory of hotel rooms, ensuring accurate information and availability.
*   **Implementation Details:**
    *   **Main Files:** `Controllers/RoomsController.cs`, `Models/Cls_Room.cs`, `Models/Cls_RoomIMages.cs`, `Interface/IRoom.cs`, `Repositry/RoomsRepository.cs`, `DTO/DTO_CreateRoom.cs`, `DTO/DTO_Rooms.cs`, `DTO/DTO_RoomImages.cs`, `Attributes/UniqueRoomNumberAttribute.cs`
    *   **Classes:** `RoomsController`, `Cls_Room`, `RoomsRepository`
    *   **Methods:** `CreateRoom`, `GetAllRooms`, `GetRoomById`, `UpdateRoom`, `DeleteRoom`
*   **API Endpoints:**
    *   **Route:** `/api/Rooms`
        *   **Method:** `POST`
        *   **Sample Request:**
            ```json
            {
              "roomNumber": "101",
              "roomTypeId": 1,
              "price": 150.00,
              "description": "Luxurious single room with city view.",
              "capacity": 2,
              "images": [
                {
                  "imageUrl": "url_to_image1.jpg",
                  "altText": "Room 101 interior"
                }
              ]
            }
            ```
        *   **Sample Response (Success):**
            ```json
            {
              "roomId": 1,
              "roomNumber": "101",
              "roomTypeId": 1,
              "price": 150.00,
              "description": "Luxurious single room with city view.",
              "capacity": 2
            }
            ```
    *   **Route:** `/api/Rooms`
        *   **Method:** `GET`
        *   **Sample Response (Success):**
            ```json
            [
              {
                "roomId": 1,
                "roomNumber": "101",
                "roomTypeId": 1,
                "price": 150.00,
                "description": "Luxurious single room with city view.",
                "capacity": 2,
                "images": [...]
              }
            ]
            ```
*   **Database Tables or Entities Used:** `Rooms`, `RoomImages`
*   **Demo Steps:**
    1.  As an admin, send a POST request to `/api/Rooms` to create a new room.
    2.  Send a GET request to `/api/Rooms` to view all available rooms.
    3.  Send a GET request to `/api/Rooms/{roomId}` to retrieve details of a specific room.
*   **Challenges Solved or Special Techniques Used:** Custom validation attribute `UniqueRoomNumberAttribute` ensures no duplicate room numbers. Image management is handled separately for flexible storage.

### Feature: Booking Management

*   **Purpose:** Enable users to book rooms, view their bookings, and for administrators to manage all bookings.
*   **User Story:** As a customer, I can search for available rooms, make a reservation, and view my upcoming bookings. As an admin, I can see all bookings and their statuses.
*   **Implementation Details:**
    *   **Main Files:** `Controllers/BookingController.cs`, `Models/Cls_Booking.cs`, `Interface/IBooking.cs`, `Repositry/BookingRepository.cs`, `DTO/DTO_Booking.cs`, `DTO/DTO_BookingDetails.cs`, `Validation/BookingValidator.cs`
    *   **Classes:** `BookingController`, `Cls_Booking`, `BookingRepository`
    *   **Methods:** `CreateBooking`, `GetBookingById`, `GetAllBookings`, `UpdateBookingStatus`, `DeleteBooking`
*   **API Endpoints:**
    *   **Route:** `/api/Booking`
        *   **Method:** `POST`
        *   **Sample Request:**
            ```json
            {
              "roomId": 1,
              "checkInDate": "2025-10-01",
              "checkOutDate": "2025-10-05",
              "customerId": 1
            }
            ```
        *   **Sample Response (Success):**
            ```json
            {
              "bookingId": 1,
              "roomId": 1,
              "checkInDate": "2025-10-01T00:00:00",
              "checkOutDate": "2025-10-05T00:00:00",
              "totalPrice": 600.00,
              "status": "Pending"
            }
            ```
    *   **Route:** `/api/Booking/{bookingId}`
        *   **Method:** `GET`
        *   **Sample Response (Success):**
            ```json
            {
              "bookingId": 1,
              "roomId": 1,
              "checkInDate": "2025-10-01T00:00:00",
              "checkOutDate": "2025-10-05T00:00:00",
              "totalPrice": 600.00,
              "status": "Pending",
              "room": { ... },
              "customer": { ... }
            }
            ```
*   **Database Tables or Entities Used:** `Bookings`, `Rooms`, `Customers`
*   **Demo Steps:**
    1.  As a customer, send a POST request to `/api/Booking` to create a new reservation for an available room.
    2.  Send a GET request to `/api/Booking/{bookingId}` to view the details of a specific booking.
    3.  As an admin, send a GET request to `/api/Booking` to see all current bookings.
*   **Challenges Solved or Special Techniques Used:** Integrated `BookingValidator.cs` for business logic validation (e.g., checking room availability during the booking period).

### Feature: Payment Processing

*   **Purpose:** Handle payment transactions for bookings.
*   **User Story:** As a customer, I can make a payment for my booking. As an admin, I can view payment records.
*   **Implementation Details:**
    *   **Main Files:** `Controllers/PaymentController.cs`, `Models/Cls_Payments.cs`, `Interface/iPayment.cs`, `Repositry/PaymentRepository.cs`, `DTO/DTO_Payment.cs`
    *   **Classes:** `PaymentController`, `Cls_Payments`, `PaymentRepository`
    *   **Methods:** `ProcessPayment`, `GetPaymentDetails`
*   **API Endpoints:**
    *   **Route:** `/api/Payment/process`
        *   **Method:** `POST`
        *   **Sample Request:**
            ```json
            {
              "bookingId": 1,
              "amount": 600.00,
              "paymentMethod": "Credit Card",
              "transactionId": "txn_12345"
            }
            ```
        *   **Sample Response (Success):**
            ```json
            {
              "paymentId": 1,
              "bookingId": 1,
              "amount": 600.00,
              "paymentDate": "2025-09-22T10:30:00",
              "status": "Completed"
            }
            ```
*   **Database Tables or Entities Used:** `Payments`, `Bookings`
*   **Demo Steps:**
    1.  After creating a booking, send a POST request to `/api/Payment/process` with payment details linked to the booking ID.
    2.  Verify the payment record is created successfully.
*   **Challenges Solved or Special Techniques Used:** Basic payment record keeping. For a production system, this would integrate with a third-party payment gateway (e.g., Stripe, PayPal).

### Feature: Customer Reviews

*   **Purpose:** Allow customers to submit reviews for rooms and for administrators to manage them.
*   **User Story:** As a customer, I can leave a review and rating for a room I've stayed in. As an admin, I can view and moderate all reviews.
*   **Implementation Details:**
    *   **Main Files:** `Controllers/ReviewsController.cs`, `Models/Cls_Reviews.cs`, `Interface/IReview.cs`, `Repositry/ReviewsRepository.cs`, `DTO/DTO_Reviews.cs`
    *   **Classes:** `ReviewsController`, `Cls_Reviews`, `ReviewsRepository`
    *   **Methods:** `AddReview`, `GetReviewsForRoom`, `DeleteReview`
*   **API Endpoints:**
    *   **Route:** `/api/Reviews`
        *   **Method:** `POST`
        *   **Sample Request:**
            ```json
            {
              "roomId": 1,
              "customerId": 1,
              "rating": 5,
              "comment": "Excellent room and service!"
            }
            ```
        *   **Sample Response (Success):**
            ```json
            {
              "reviewId": 1,
              "roomId": 1,
              "customerId": 1,
              "rating": 5,
              "comment": "Excellent room and service!",
              "reviewDate": "2025-09-22T11:00:00"
            }
            ```
    *   **Route:** `/api/Reviews/room/{roomId}`
        *   **Method:** `GET`
        *   **Sample Response (Success):**
            ```json
            [
              {
                "reviewId": 1,
                "roomId": 1,
                "customerId": 1,
                "rating": 5,
                "comment": "Excellent room and service!",
                "reviewDate": "2025-09-22T11:00:00",
                "customer": { ... }
              }
            ]
            ```
*   **Database Tables or Entities Used:** `Reviews`, `Rooms`, `Customers`
*   **Demo Steps:**
    1.  As a customer, send a POST request to `/api/Reviews` to submit a review for a room.
    2.  Send a GET request to `/api/Reviews/room/{roomId}` to view all reviews for a specific room.
*   **Challenges Solved or Special Techniques Used:** Basic CRUD operations for reviews. Future enhancements could include sentiment analysis or moderation workflows.

## 📊 API Endpoints Summary

| Feature                 | Route                         | Method | Description                                    |
| :---------------------- | :---------------------------- | :----- | :--------------------------------------------- |
| User Authentication     | `/api/Account/register`       | `POST` | Register a new user account                    |
| User Authentication     | `/api/Account/login`          | `POST` | Log in a user and get a JWT token              |
| Room Management         | `/api/Rooms`                  | `POST` | Create a new room                              |
| Room Management         | `/api/Rooms`                  | `GET`  | Get all available rooms                        |
| Room Management         | `/api/Rooms/{roomId}`         | `GET`  | Get details of a specific room                 |
| Booking Management      | `/api/Booking`                | `POST` | Create a new room booking                      |
| Booking Management      | `/api/Booking/{bookingId}`    | `GET`  | Get details of a specific booking              |
| Payment Processing      | `/api/Payment/process`        | `POST` | Process a payment for a booking                |
| Customer Reviews        | `/api/Reviews`                | `POST` | Add a new review for a room                    |
| Customer Reviews        | `/api/Reviews/room/{roomId}`  | `GET`  | Get all reviews for a specific room            |

## 3. Architecture & Tech Stack

The project follows a layered architectural pattern, common in ASP.NET Core applications, promoting separation of concerns and maintainability.

*   **Framework:** ASP.NET Core 8.0
*   **Language:** C#
*   **Database:** SQL Server (via Entity Framework Core)
*   **ORM:** Entity Framework Core
*   **Authentication:** ASP.NET Core Identity, JWT Bearer Tokens
*   **Dependency Injection:** Built-in ASP.NET Core DI
*   **DTOs:** Used for data transfer between layers, ensuring clean separation of concerns and preventing over-posting/under-posting.
*   **Repositories:** Implement the repository pattern to abstract data access logic from the business layer.
*   **Migrations:** Entity Framework Core Migrations for database schema management.
*   **Validation:** Data annotations and custom validation attributes.
*   **PDF Generation:** `PDF_Generator/PdfGenerator.cs` (indicates a custom PDF generation utility).

## 4. Installation & Setup

To set up and run the Hotel Booking System locally, follow these steps:

1.  **Prerequisites:**
    *   .NET 8.0 SDK: [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
    *   SQL Server (or another compatible database system)
    *   An IDE like Visual Studio or Visual Studio Code with C# extensions.

2.  **Clone the Repository:**
    ```bash
    git clone <repository_url>
    cd Hotel_Bokking_System
    ```

3.  **Database Configuration:**
    *   Open `appsettings.json` (and `appsettings.Development.json`).
    *   Update the `ConnectionStrings:DefaultConnection` with your SQL Server connection string. Example:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HotelBookingDb;Trusted_Connection=True;MultipleActiveResultSets=true"
        }
        ```

4.  **Apply Database Migrations:**
    Navigate to the project directory in your terminal and run the following commands:
    ```bash
    dotnet ef database update
    ```
    This will create the database and apply all pending migrations.

5.  **Build the Project:**
    ```bash
    dotnet build
    ```

6.  **Run the Application:**
    ```bash
    dotnet run
    ```
    The application will typically run on `https://localhost:7001` or `http://localhost:5001`. The exact ports can be found in `Properties/launchSettings.json`.

## 5. Usage Instructions

Once the application is running:

1.  **Access API Endpoints:** Use a tool like Postman, Insomnia, or curl to interact with the API endpoints.
2.  **User Registration:**
    *   `POST /api/Account/register` to create new user accounts.
3.  **User Login:**
    *   `POST /api/Account/login` to obtain a JWT token. Include this token in the `Authorization: Bearer <token>` header for protected endpoints.
4.  **Admin Operations (e.g., Room Management):**
    *   Use the JWT token of an authenticated admin user to perform operations like `POST /api/Rooms`, `GET /api/Rooms`, etc.
5.  **Customer Operations (e.g., Booking a Room):**
    *   Use the JWT token of an authenticated customer to perform operations like `POST /api/Booking`, `GET /api/Booking/{bookingId}`, etc.

## 6. Testing Instructions

*   Currently, there are no dedicated test projects identified in the provided directory structure.
*   **Future Improvement:** Implement unit, integration, and end-to-end tests for all critical functionalities.

## 7. Security and Role-Based Access Notes

*   **Authentication:** Uses ASP.NET Core Identity for user management and JWT for API authentication.
*   **Authorization:** Role-based authorization is implemented to restrict access to certain actions based on user roles (e.g., "Admin", "Customer").
*   **Password Hashing:** Passwords are automatically hashed and salted by ASP.NET Core Identity.
*   **Known Vulnerabilities:** As with any custom application, care must be taken to prevent common web vulnerabilities (e.g., SQL Injection, XSS, CSRF). Input validation and proper API security practices are crucial.

## 8. Known Issues or Limitations

*   **No Dedicated Testing:** Lack of automated tests may lead to regressions.
*   **Basic Error Handling:** Error handling might be basic and could be improved with more specific error messages and logging.
*   **Limited Search/Filtering:** The current API endpoints might not offer advanced search or filtering capabilities for rooms or bookings.
*   **Scalability:** For high-traffic scenarios, performance optimizations and horizontal scaling considerations would be needed.
*   **External Payment Gateway Integration:** Payments are currently recorded internally; integration with real payment gateways is a future step.

## 9. Future Improvements

*   **Comprehensive Testing:** Implement a full suite of unit, integration, and end-to-end tests.
*   **Advanced Room Search:** Implement features to search rooms by dates, amenities, price range, etc.
*   **User Interface (UI):** Develop a modern and responsive front-end application (e.g., using React, Angular, or Vue.js) to consume the API.
*   **Real-time Notifications:** Implement real-time updates for booking status changes or new reviews.
*   **Logging and Monitoring:** Integrate a robust logging framework and monitoring tools.
*   **Payment Gateway Integration:** Integrate with a live payment gateway (e.g., Stripe, PayPal).
*   **Admin Dashboard:** Create a dedicated admin dashboard for managing rooms, bookings, users, and reviews more efficiently.
*   **Caching:** Implement caching mechanisms to improve performance for frequently accessed data.
*   **API Versioning:** Introduce API versioning for better manageability of changes.
*   **Containerization:** Dockerize the application for easier deployment and scalability.

## 10. References to Official Docs for Each Technology

*   **ASP.NET Core:** [https://learn.microsoft.com/en-us/aspnet/core/](https://learn.microsoft.com/en-us/aspnet/core/)
*   **Entity Framework Core:** [https://learn.microsoft.com/en-us/ef/core/](https://learn.microsoft.com/en-us/ef/core/)
*   **ASP.NET Core Identity:** [https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
*   **JWT (JSON Web Tokens):** [https://jwt.io/](https://jwt.io/) (general information, not specific to .NET implementation)
*   **C# Language:** [https://learn.microsoft.com/en-us/dotnet/csharp/](https://learn.microsoft.com/en-us/dotnet/csharp/)
*   **SQL Server:** [https://learn.microsoft.com/en-us/sql/sql-server/](https://learn.microsoft.com/en-us/sql/sql-server/)
*   **.NET SDK:** [https://dotnet.microsoft.com/](https://dotnet.microsoft.com/)
