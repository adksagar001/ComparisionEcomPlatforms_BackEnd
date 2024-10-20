using Microsoft.AspNetCore.Mvc;
using ComparisionEcomPlatforms_BackEnd.Model.User;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration; // For accessing configuration

namespace ComparisionEcomPlatforms_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Inject IConfiguration to access the connection string
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("getUsers")]  // Explicit route for this action
        public async Task<IActionResult> GetUsers([FromBody] User u)
        {
            // Connection string from appsettings.json
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Define the SQL connection and command
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Comp_GetAllUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add the TokenNo parameter for the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@TokenNo", SqlDbType.NVarChar, 200)).Value = u.TokenNo;

                    // Open connection
                    conn.Open();

                    // Execute the command and read the results
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        var users = new List<object>();

                        // Read the data returned by the stored procedure
                        while (await reader.ReadAsync())
                        {
                            users.Add(new
                            {
                                Id = reader["id"],
                                UserId = reader["userid"],
                                Password = reader["password"]
                            });
                        }

                        // Return the result as JSON
                        return Ok(users);
                    }
                }
            }
        }
    }
}
