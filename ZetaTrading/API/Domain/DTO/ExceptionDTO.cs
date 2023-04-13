namespace ZetaTrading.API.Domain.DTO
{
    public class ExceptionDTO
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public ExceptionDataDTO Data { get; set; }
    }
}
