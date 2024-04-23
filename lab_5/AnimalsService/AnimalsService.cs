using System.Data.SqlClient;
using lab_5.Models;

namespace lab_5.AnimalsService;

public class AnimalsService
{
    private readonly IConfiguration _configuration;

    public AnimalsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<Animals> GetAnimals(string orderBy = "name")
    {
        var query = $"SELECT * FROM Animals ORDER BY {orderBy}";

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

                    return animals;
                }
            }
        }
    }

    public void CreateAnimal(Animals animal)
    {
        var query = @"
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
    }

    public void UpdateAnimal(Animals animal)
    {
        var query = @"
            UPDATE Animals
            SET Name = @Name, Description = @Description, Category = @Category, Area = @Area
            WHERE Id = @Id";

        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", animal.IdAnimal);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteAnimal(int id)
    {
        var query = "DELETE FROM Animals WHERE Id = @Id";

        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
