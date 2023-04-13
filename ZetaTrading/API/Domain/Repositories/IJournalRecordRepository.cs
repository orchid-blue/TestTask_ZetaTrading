using ZetaTrading.API.Domain.DTO;
using ZetaTrading.Models;

namespace ZetaTrading.API.Domain.Repositories
{
    public interface IJournalRecordRepository
    {
        void PushJournalRecordToDb(Exception ex);
        JournalRecord? GetJournalRecordById(int id);
        List<JournalRecord> GetRangeJournalRecords(int skip, int take, DateTime? from, DateTime? to);
    }
}
