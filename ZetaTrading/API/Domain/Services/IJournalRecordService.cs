using ZetaTrading.API.Domain.DTO;

namespace ZetaTrading.API.Domain.Services
{
    public interface IJournalRecordService
    {
        public void PushRecordToJournal(Exception ex);
        ExceptionDTO GetExceptionToDisplay(Exception ex);
        JournalRecordExtendedDTO GetSingleRecordById(int id);
        RangeResultDTO GetRangeRecords(int skip, int take, FilterDTO filter);
    }
}
