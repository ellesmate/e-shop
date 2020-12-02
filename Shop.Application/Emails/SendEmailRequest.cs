namespace Shop.Application.Emails
{
    public class SendEmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool Html { get; set; } = true;
        public short Failed { get; set; } = 0;
    }
}
