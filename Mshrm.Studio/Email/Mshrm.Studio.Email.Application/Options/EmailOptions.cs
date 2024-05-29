namespace Mshrm.Studio.Email.Api.Models.Options
{
    public class EmailOptions
    {
        public string ApplicationUrl { get; set; }
        public string CompanyEmailServer { get; set; }
        public int CompanyEmailPort { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyEmailPassword { get; set; }
        public bool Enabled { get; set; }
        public string AdminEmailAddress { get; set; }
        public string CompanyEmailDisplayName { get; set; }
    }
}
