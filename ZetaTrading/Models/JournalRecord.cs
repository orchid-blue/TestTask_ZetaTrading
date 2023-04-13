using System.ComponentModel.DataAnnotations;

namespace ZetaTrading.Models
{
    public class JournalRecord
    {
        [Key]
        public int Id { get; set; }

        public string RequestId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? StackTrace { get; set; }

        public string? BodyParameters { get; set; }

        public string? QueryParameters { get; set; }

        public string? Path { get; set; }
    }
}
