namespace lab_5.Controller;

using Microsoft.AspNetCore.Mvc;
using lab_5.Models;
using lab_5.AnimalsService; 

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AnimalsService _animalService;

    public AnimalsController(IConfiguration configuration, AnimalsService animalService)
    {
        _configuration = configuration;
        _animalService = animalService;
    }

    [HttpGet]
    public IActionResult GetAnimals(string orderBy = "name")
    {
        try
        {
            var animals = _animalService.GetAnimals(orderBy);
            return Ok(animals);
        }
        catch (Exception ex)
        {
            return Content("Error get", "text/plain");
        }
    }

    [HttpPost]
    public IActionResult PostAnimal(Animals animal)
    {
        try
        {
            _animalService.CreateAnimal(animal);
            return CreatedAtRoute("GetAnimalById", new { id = animal.IdAnimal }, animal);
        }
        catch (Exception ex)
        {
            return Content("Error create", "text/plain");
        }
    }

    [HttpPut("{id}")]
    public IActionResult PutAnimal(int id, Animals animal)
    {
        if (id != animal.IdAnimal)
        {
            return Content("Error id", "text/plain");
        }

        try
        {
            animal.IdAnimal = id; 
            _animalService.UpdateAnimal(animal);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Content("Error update", "text/plain");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        try
        {
            _animalService.DeleteAnimal(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Content("Error delete", "text/plain");
        }
    }
}
