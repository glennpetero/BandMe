namespace BandMe.Api.Data;

public class Band
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxMembers { get; set; } = 10;  //TODO: Make this a constant somewhere
    public int CurrentMemberCount { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Many-to-many navigation property
    public List<Interest> Interests { get; set; } = [];
}

public class Interest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    // Many-to-many navigation property
    public List<Band> Bands { get; set; } = [];
}
