# cdn_api
 

**Overview:**
Complete Developer Network Web API project to handle freelancer details

**Project Description:**

Web API (Application Program Interface) Application to maintain freelancer details in Relational Database Management System.

**Technology Used:**

1. ASP.NET Core Web API, 
2. MVC (Model View Controller)
3. Microsoft.NETCore 7.0.11, 
4. Microsoft.EntityFrameworkCore
5. SQLite3 (Relational Database Management System)
6. Swashbuckle ASPNetCore ( UI)
7. Dependency Injection Services

**Code Implementation:**

Controllers >  Handles all the HTTP requests made by the user
Models > User data and user context created for database
Services > Business logic handled in interface and class
Data > Sqlite3 database app.db to store users' data
Migrations  > auto-generated database migration info
appSettings.json > database connection strings
Program.cs >  Dependency Injection register, persistent database user context register

MVC pattern implemented.
After the model is created, execute this command create a migration and database update

**Commands:**
 1. dotnet ef migrations add “init_migrations”
 2. dotnet ef database update

Created an Interface IUserService to define the API actions method

public interface IUserService
	{
        Task<ActionResult<IEnumerable<User>>> GetUsers();
        Task<ActionResult<User>> GetUserById(int id);
        Task PostUser(User userDTO);
        Task<IActionResult> PutUser(int id, User userDTO);
        Task<IActionResult> DeleteUser(int id);
      }

As ordered by the 
1. To get all the lists of users
2. To get the user by id passed in the parameter
3. To create a new user with JSON body object
4. To update an existing user with user id and JSON body object
5. To delete the user by user id passed on parameter

IUserService interface is implemented in UserService Class that includes UserContext (EntityFramework) to carry out data operations.

Finally, the Controller has the interface IUserService dependency injected UserController, this type of injection is “Constructor” Injection, here implementing the loosely coupled so that UserController is not tie to any service and implementing less code.

The Controller knows only the method it calling and does not know the implementation of all business logic handled by the UserService class. 

Construction Dependency Injection in Controller:

       private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

Controller get method calls _userService.GetUsers(); with less implementation.
Each method has an attribute HTTP actions [HttpGet]

// To get all users created
// GET: api/User

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
           return await _userService.GetUsers();
        }

// To get user details by passing the user ID
// GET: api/User/{id}

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            return await _userService.GetUserById(id);
        }

// To create a user detail with JSON body data
// POST: api/User

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            await _userService.PostUser(user);

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id },user);
         }
// To update the user with ID and JSON data to modify
// PUT: api/User/{id}

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            return await _userService.PutUser(id, user);
        }

//To delete the user by passing the user ID
// DELETE: api/User/{id}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await _userService.DeleteUser(id);
        }

**IMPORTANT:** With all the implementation the application will not run because we need to register the service class and its interface in the application builder service of the program.cs file

// Register User Service for Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();

Also to take note that UserController must have IUserService used and not the UserService because we are hiding the implementation of our business logic to UserController need not to know.

SQLite3 data is located in \\Data\app.db associated with Models\UserContext.cs and our connection string placed in ‘appsettings.json’

"ConnectionStrings": {
    "DefaultConnection": "DataSource=.\\Data\\app.db"
  }

We register the Persistent database in the program.cs file

// Use Persistent database Sqlite

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserContext>(option => option.UseSqlite(connectionString));
 

**Data Model Validation:**

A data model class created with attribute validation such as 
Required, Email Validation, Regular Expression, MaxLength to ensure the data handled in api are valid, the pre-defined error message are populated when requesting API action.


[EmailAddress(ErrorMessage = "Invalid email address”)]
 public string Mail { get; set; }

Invalid email address identified and the error message is prompted

[Required, RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "User name must be in letters")]
public string UserName { get; set; }

Users can enter only the alphabet no other letters are allowed

 [Required, RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? PhoneNumber { get; set; }

Users can enter phone numbers only 10 digits and not less than 10 or above 10


**API Actions:**

Purpose: Get all the user records 
Action: GET        
Endpoint:  api/User

Purpose: Get user record by id
Action:  GET
Endpoint: api/User/{id}
Parameter: pass the id of the user

Purpose: Create a new user record
Action: POST
Endpoint: api/User
Data: JSON object in request body

Purpose: Update an existing user record
Action: PUT
Endpoint: api/User{id}
Parameter: pass the id of the user
Data:  JSON object in request body

Purpose: Delete the existing user record
Action: DELETE
Endpoint: api/User/{id}
Parameter: pass the id of the user


