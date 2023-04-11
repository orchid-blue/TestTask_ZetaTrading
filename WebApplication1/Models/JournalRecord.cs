using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class JournalRecord
    {
        [Key]
        public string RequestId { get; set; }

        public DateTime CreatedDate { get; set;}

        public string StackTrace { get; set; }

        public string BodyParameters { get; set; }

        public string QueryParameters { get; set; }
    }
}
