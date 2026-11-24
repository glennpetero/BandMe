namespace BandMe.Shared.Models;

public class BandDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxMembers { get; set; } = 10;  //TODO: Make this a constant somewhere
    public int CurrentMemberCount { get; set; }
    public List<string> Tags { get; set; } = [];
    public int MatchScore { get; set; }
}
