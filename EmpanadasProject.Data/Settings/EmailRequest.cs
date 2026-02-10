namespace EmpanadasProject.Data.Settings
{
    public class EmailRequest
    {
        public required string Subject { get; set; }
        public required string HtmlBody { get; set; }
        public required List<string> To { get; set; } = [];
    }
}
