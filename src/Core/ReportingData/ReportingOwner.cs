namespace Core.ReportingData;

public class ReportingOwner
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ReportingOwner(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
