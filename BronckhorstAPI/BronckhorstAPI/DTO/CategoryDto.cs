namespace BronckhorstAPI.DTO;

public class CategoryDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? TranslatedName { get; set; }

    public int? ParentId { get; set; }
}
