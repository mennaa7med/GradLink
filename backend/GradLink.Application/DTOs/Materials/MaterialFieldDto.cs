namespace GradLink.Application.DTOs.Materials;

public class MaterialFieldDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateMaterialFieldRequest
{
    public required string Name { get; set; }
}

public class UpdateMaterialFieldRequest
{
    public string? Name { get; set; }
}

