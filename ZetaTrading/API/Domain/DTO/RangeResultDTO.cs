namespace ZetaTrading.API.Domain.DTO
{
    public class RangeResultDTO
    {
        public int Skip { get; set; }
        public int Count { get; set; }
        public List<JournalRecordDTO>? Items { get; set; }
    }
}
