using BandMe.Api.Data;
using BandMe.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BandMe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BandsController(AppDbContext db) : ControllerBase
{
    // GET: api/bands (List all bands)
    [HttpGet]
    public async Task<ActionResult<List<BandDto>>> GetAll()
    {
        var bands = await db.Bands
            .Include(b => b.Interests)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return Ok(bands.Select(b => ToDto(b, [])).ToList());
    }

    // POST: api/bands/match (Score and rank bands based on questionnaire answers)
    [HttpPost("match")]
    public async Task<ActionResult<List<BandDto>>> Match([FromBody] QuestionnaireSubmission submission)
    {
        var bands = await db.Bands
            .Include(b => b.Interests)
            .ToListAsync();

        var ranked = bands
            .Select(b => ToDto(b, submission.SelectedTagIds))
            .Where(dto => dto.MatchScore > 0)
            .OrderByDescending(dto => dto.MatchScore)
            .ThenBy(dto => dto.Name)
            .ToList();

        return Ok(ranked);
    }

    // POST: api/bands (Create a new band/group)
    [HttpPost]
    public async Task<ActionResult<BandDto>> Create([FromBody] CreateBandRequest request)
    {
        var selectedInterests = await db.Interests
            .Where(i => request.SelectedTagIds.Contains(i.Id))
            .ToListAsync();

        if (selectedInterests.Count == 0)
            return BadRequest("At least one valid interest tag is required.");

        var band = new Band
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            MaxMembers = request.MaxMembers,
            CurrentMemberCount = 1,
            Interests = selectedInterests
        };

        db.Bands.Add(band);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { id = band.Id }, ToDto(band, request.SelectedTagIds));
    }

    // POST: api/bands/{id}/join (Join an existing band)
    [HttpPost("{id:int}/join")]
    public async Task<ActionResult<BandDto>> Join(int id)
    {
        var band = await db.Bands
            .Include(b => b.Interests)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (band is null)
            return NotFound();

        if (band.CurrentMemberCount >= band.MaxMembers)
            return BadRequest("This band is already full.");

        band.CurrentMemberCount++;
        await db.SaveChangesAsync();

        return Ok(ToDto(band, []));
    }

    private static BandDto ToDto(Band band, List<int> userTagIds)
    {
        int matchCount = band.Interests.Count(i => userTagIds.Contains(i.Id));
        int score = userTagIds.Count > 0
            ? (int)Math.Round((double)matchCount / userTagIds.Count * 100)
            : 0;

        return new BandDto
        {
            Id = band.Id,
            Name = band.Name,
            Description = band.Description,
            MaxMembers = band.MaxMembers,
            CurrentMemberCount = band.CurrentMemberCount,
            Tags = band.Interests.Select(i => i.Name).ToList(),
            MatchScore = score
        };
    }
}
