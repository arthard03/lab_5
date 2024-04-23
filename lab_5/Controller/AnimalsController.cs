namespace lab_5.Controller;

using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using lab_5.Models;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    [HttpGet]
    public IActionResult GetAnimals(string orderBy = "name")
    {
        try
        {
            string query = $"SELECT * FROM Animals ORDER BY {orderBy}";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var animals = new List<Animals>();
                        while (reader.Read())
                        {
                            animals.Add(new Animals
                            {
                                IdAnimal = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Category = reader["Category"].ToString(),
                                Area = reader["Area"].ToString()
                            });
                        }

                        return Ok(animals); 
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }


    [HttpPost]
    public IActionResult PostAnimal(Animals animal)
    {
        try
        {
            string query = @"
            INSERT INTO Animals (Name, Description, Category, Area)
            VALUES (@Name, @Description, @Category, @Area)";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", animal.Name);
                    command.Parameters.AddWithValue("@Description", animal.Description);
                    command.Parameters.AddWithValue("@Category", animal.Category);
                    command.Parameters.AddWithValue("@Area", animal.Area);
                    command.ExecuteNonQuery();
                }
            }

            return CreatedAtRoute("GetAnimalById", new { id = animal.IdAnimal }, animal);
        }
        catch (Exception ex)
        {
            return Content("Error", "text/plain");
        }
    }


    [HttpPut("{id}")]
    public IActionResult PutAnimal(int id, Animals animal)
    {
        if (id != animal.IdAnimal)
        {
            return BadRequest("ID in URL and body must match");
        }

        try
        {
            string query = @"
                UPDATE Animals
                SET Name = @Name, Description = @Description, Category = @Category, Area = @Area
                WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", animal.Name);
                    command.Parameters.AddWithValue("@Description", animal.Description);
                    command.Parameters.AddWithValue("@Category", animal.Category);
                    command.Parameters.AddWithValue("@Area", animal.Area);
                    command.ExecuteNonQuery();
                }
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return Content("Error", "text/plain");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        try
        {
            string query = "DELETE FROM Animals WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return Content("Error", "text/plain");
        }
    }
}