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

    private List<Dictionary<string, object>> ConvertDataTableToList(DataTable dt)
    {
        var columns = dt.Columns.Cast<DataColumn>();
        return dt.Rows.Cast<DataRow>()
            .Select(row => columns.ToDictionary(column => column.ColumnName, column => row[column])).ToList();
    }

    [HttpGet]
    public IActionResult GetAnimals(string orderBy = "name")
    {
        string query = $"SELECT * FROM Animals ORDER BY {orderBy}";
        DataTable table = new DataTable();
        string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        {
            myCon.Open();
            using (SqlCommand myCommand = new SqlCommand(query, myCon))
            {
                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    table.Load(myReader);
                }
            }
            myCon.Close();
        }

        var list = ConvertDataTableToList(table);
        return Ok(list);
    }

    [HttpPost]
    public IActionResult PostAnimal(Animals animal)
    {
        string query = @"
           INSERT INTO Animals (Name, Description, Category, Area) 
           VALUES (@Name, @Description, @Category, @Area)";
        DataTable table = new DataTable();
        string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
        SqlDataReader myReader;
        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        {
            myCon.Open();
            using (SqlCommand myCommand = new SqlCommand(query, myCon))
            {
                myCommand.Parameters.AddWithValue("@Name", animal.Name);
                myCommand.Parameters.AddWithValue("@Description", animal.Description);
                myCommand.Parameters.AddWithValue("@Category", animal.Category);
                myCommand.Parameters.AddWithValue("@Area", animal.Area);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();
            }
        }
        return new JsonResult("Added Successfully");
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
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
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
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
        
    }
}

