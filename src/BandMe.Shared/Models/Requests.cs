using System.ComponentModel.DataAnnotations;

namespace BandMe.Shared.Models;

public class CreateBandRequest
{
    [Required, StringLength(60, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(2, 50)]
    public int MaxMembers { get; set; } = 10;

    [MinLength(1, ErrorMessage = "Select at least one interest tag.")]
    public List<int> SelectedTagIds { get; set; } = [];
}

public class QuestionnaireSubmission
{
    [MinLength(1, ErrorMessage = "Select at least one interest to find matching bands.")]
    public List<int> SelectedTagIds { get; set; } = [];
}
