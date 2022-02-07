namespace Core.ReportingData;

public class ReportingStakeholder
{
    public int Id { get; set; }
    public string Username { get; set; }

    public ReportingStakeholder(int id, string uname)
    {
        Id = id;
        Username = uname;
    }
}
