namespace GradLink.Application.DTOs.TeamRequests;

public class TeamRequestDto
{
    public int Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> SkillsNeeded { get; set; } = new();
    public int MembersNeeded { get; set; }
    public int CurrentMembers { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerAvatar { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string PostedDate { get; set; } = string.Empty;
}

public class CreateTeamRequestDto
{
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string SkillsNeeded { get; set; } = string.Empty;
    public int MembersNeeded { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
}

public class UpdateTeamRequestDto
{
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? SkillsNeeded { get; set; }
    public int? MembersNeeded { get; set; }
    public int? CurrentMembers { get; set; }
    public string? Status { get; set; }
}

public class JoinRequestDto
{
    public int Id { get; set; }
    public int TeamRequestId { get; set; }
    public string TeamRequestName { get; set; } = string.Empty;
    public string ApplicantId { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string ApplicantEmail { get; set; } = string.Empty;
    public List<string> ApplicantSkills { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateJoinRequestDto
{
    public int TeamRequestId { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public string ApplicantEmail { get; set; } = string.Empty;
    public string ApplicantSkills { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class UpdateJoinRequestStatusDto
{
    public string Status { get; set; } = string.Empty; // Accepted, Rejected
}






