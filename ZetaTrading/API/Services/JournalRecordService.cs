using System.Web;
using ZetaTrading.API.Domain.DTO;
using ZetaTrading.API.Domain.Persistence.Repositories;
using ZetaTrading.API.Domain.Repositories;
using ZetaTrading.API.Domain.Services;
using ZetaTrading.Exceptions;
using ZetaTrading.Models;

namespace ZetaTrading.API.Services
{
    public class JournalRecordService : IJournalRecordService
    {
        private readonly IJournalRecordRepository _journalRecordRepository;

        public JournalRecordService(IJournalRecordRepository journalRecordRepository)
        {
            _journalRecordRepository = journalRecordRepository;
        }

        public void PushRecordToJournal(Exception ex)
        {
            _journalRecordRepository.PushJournalRecordToDb(ex);
        }

        public ExceptionDTO GetExceptionToDisplay(Exception ex)
        {
            string traceId = ex.Data["traceId"]?
                .ToString() ?? string.Empty;
            string message = ex is SecureException ? ex.Message : $"Internal server error ID = {traceId}.";
            string type = ex is SecureException ? "secure" : "internal";

            ExceptionDataDTO data = new ExceptionDataDTO() { Message = message };
            ExceptionDTO error = new ExceptionDTO() { Data = data, Id = traceId, Type = type };

            return error;
        }

        public JournalRecordExtendedDTO GetSingleRecordById(int id)
        {
            var fromDb = _journalRecordRepository.GetJournalRecordById(id);
            if (fromDb == null)
            {
                throw new SecureException("Can't find Record with provided ID.");
            }

            return BuildJournalRecordExtendedDTO(fromDb);
        }

        private string BuildText(JournalRecord? journalRecord)
        {
            string? queryString = journalRecord?.QueryParameters?.Replace("?", string.Empty);
            var pms = queryString?.Split('&');
            
            return $@"  Request ID = {journalRecord.RequestId}
                            Path = {journalRecord.Path}
                            Params = {string.Join("\n\r", value: pms)}
                            StackTrace = {journalRecord.StackTrace}";
        }

        private JournalRecordExtendedDTO BuildJournalRecordExtendedDTO(JournalRecord? journalRecord)
        {
            return new JournalRecordExtendedDTO()
            {
                Id = journalRecord.Id,
                EventId = journalRecord.RequestId,
                CreatedAt = journalRecord.CreatedDate,
                Text = BuildText(journalRecord)
            };
        }
        private JournalRecordDTO BuildJournalRecordDTO(JournalRecord journalRecord)
        {
            return new JournalRecordDTO()
            {
                Id = journalRecord.Id,
                EventId = journalRecord.RequestId,
                CreatedAt = journalRecord.CreatedDate
            };
        }
        public RangeResultDTO GetRangeRecords(int skip, int take, FilterDTO filter)
        {
            var rangeRes = _journalRecordRepository.GetRangeJournalRecords(skip, take, filter.From, filter.To);

            return new RangeResultDTO()
            {
                Skip = skip,
                Count = rangeRes.Count,
                Items = rangeRes.Select(BuildJournalRecordDTO).ToList()
            };
        }
    }
}
