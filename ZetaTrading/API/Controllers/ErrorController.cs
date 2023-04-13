using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZetaTrading.API.Domain.DTO;
using ZetaTrading.API.Domain.Repositories;
using ZetaTrading.API.Domain.Services;
using ZetaTrading.Exceptions;

namespace ZetaTrading.API.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IJournalRecordService _journalRecordService;
        public ErrorController(IJournalRecordService journamRecordService)
        {
            _journalRecordService = journamRecordService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            string body = string.Empty;
            using (var reader = new StreamReader(Request.Body))
            {
                body = reader.ReadToEnd();
            }

            var currEx = context.Error;
            currEx.Data.Add("body", body);
            currEx.Data.Add("queryString", HttpContext.Request.QueryString.Value?.ToString());
            currEx.Data.Add("traceId", HttpContext.TraceIdentifier);
            currEx.Data.Add("path", context.Path);

            _journalRecordService.PushRecordToJournal(context.Error);

            ExceptionDTO err = _journalRecordService.GetExceptionToDisplay(context.Error);
            err.Type = context.Error is SecureException ? "secure" : "internal"; ;
            
            return Json(err);
        }
    }
}
