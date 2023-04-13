using ZetaTrading.API.Domain.DTO;
using ZetaTrading.API.Domain.Persistence.Contexts;
using ZetaTrading.API.Domain.Repositories;
using ZetaTrading.Models;

namespace ZetaTrading.API.Domain.Persistence.Repositories
{
    public class JournalRecordRepository : BaseRepository, IJournalRecordRepository
    {
        public JournalRecordRepository(AppDbContext context) : base(context)
        {

        }

        public void PushJournalRecordToDb(Exception ex)
        {
            _context.ChangeTracker.Clear();

            JournalRecord rec = new JournalRecord() 
            { 
                StackTrace = ex.StackTrace, 
                BodyParameters = ex.Data["body"].ToString(), 
                QueryParameters = ex.Data["queryString"].ToString(), 
                RequestId = ex.Data["traceId"].ToString(),
                Path = ex.Data["path"].ToString()
            };
            _context.JournalRecords.Add(rec);
            _context.SaveChanges();
        }

        public JournalRecord? GetJournalRecordById(int id)
        {
            return _context.JournalRecords.Find(id);
        }

        public List<JournalRecord> GetRangeJournalRecords(int skip, int take, DateTime? from, DateTime? to)
        {
            var res = _context.JournalRecords.AsEnumerable();

            if (from != null)
            {
                res = res.Where(x => x.CreatedDate > from);
            }

            if (to != null)
            {
                res = res.Where(x => x.CreatedDate < to);
            }

            return res.Skip(skip).Take(take).ToList();
        }
    }
}
