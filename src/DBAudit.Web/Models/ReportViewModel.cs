namespace DBAudit.Web.Models;

public class ReportViewModel
{
    public string Title { get; set; }
    public List<(string Title, string Link)> Links { get; set; }
}