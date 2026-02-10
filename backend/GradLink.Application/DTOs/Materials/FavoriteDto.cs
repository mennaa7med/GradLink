namespace GradLink.Application.DTOs.Materials;

public class FavoriteDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int MaterialId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Include material info
    public MaterialDto? Material { get; set; }
}

