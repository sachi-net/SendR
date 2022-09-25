namespace SimplySoft.Core.SendR.Email.Models
{
    internal class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string BusinessName { get; set; }
        public int TimeOut { get; set; } = 10000;
        public bool EnableSsl { get; set; }
    }
}
