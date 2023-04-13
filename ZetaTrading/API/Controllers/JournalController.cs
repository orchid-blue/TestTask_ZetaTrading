using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ZetaTrading.API.Domain.DTO;
using ZetaTrading.API.Domain.Services;

namespace ZetaTrading.API.Controllers
{
    public class JournalController : Controller
    {
        private readonly IJournalRecordService _journalRecordService;
        public JournalController(IJournalRecordService journalRecordService)
        {
            _journalRecordService = journalRecordService;
        }

        [HttpPost]
        [Route("/api.user.journal.getSingle")]
        public IActionResult GetSingle([Required] int id)
        {
            return Json(_journalRecordService.GetSingleRecordById(id));
        }

        [HttpPost]
        [Route("/api.user.journal.getRange")]
        public IActionResult GetRange([Required] int skip, [Required] int take, [Required][FromBody] FilterDTO filter)
        {
            return Json(_journalRecordService.GetRangeRecords(skip, take, filter));
        }
    }
}
