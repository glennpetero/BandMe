using BandMe.Api.Data;
using BandMe.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BandMe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InterestsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<InterestTag>>> GetAll()
    {
        var tags = await db.Interests
            .OrderBy(i => i.Category)
            .ThenBy(i => i.Name)
            .Select(i => new InterestTag
            {
                Id = i.Id,
                Name = i.Name,
                Category = i.Category
            })
            .ToListAsync();

        return Ok(tags);
    }
}
