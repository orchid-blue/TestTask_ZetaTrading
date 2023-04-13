namespace ZetaTrading.Exceptions
{
    public class SecureException : Exception
    {
        public string Type { get; set; }
        public override string Message { get; }
        public string StackTrace { get; set; }
        public bool Status { get; set; }
        public int StatusCode { get; set; }

        public SecureException(string message)
        {
            StatusCode = 500;
            Type = "secure";
            Message = message;
        }
    }
}
