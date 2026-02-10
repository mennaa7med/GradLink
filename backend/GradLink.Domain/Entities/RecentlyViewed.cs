namespace GradLink.Domain.Entities;

public class RecentlyViewed
{
    public int Id { get; set; }

    // Foreign Keys
    public required string UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public int MaterialId { get; set; }
    public Material? Material { get; set; }

    // System Information
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

