namespace SohatNotebook.Entities.DbSet;

public class HealthData : BaseEntity
{
    public Decimal Height { get; set; }
    public Decimal Weight { get; set; }
    public string BloodType { get; set; }   // TODO: Make this information on enum
    public string Race { get; set; }
    public bool UseGlasses { get; set; }

}
