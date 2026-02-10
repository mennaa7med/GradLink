namespace GradLink.Domain.Entities;

public class TeamRequest
{
    public int Id { get; set; }
    
    // Project Information
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // AI/ML, Web, Mobile, IoT, Gaming, Other
    public string SkillsNeeded { get; set; } = string.Empty; // Comma-separated skills
    
    // Team Information
    public int MembersNeeded { get; set; }
    public int CurrentMembers { get; set; } = 1;
    
    // Owner Information
    public string OwnerId { get; set; } = string.Empty;
    public ApplicationUser? Owner { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
    
    // Status
    public string Status { get; set; } = "Active"; // Active, Closed, Cancelled
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public ICollection<JoinRequest> JoinRequests { get; set; } = new List<JoinRequest>();
}






