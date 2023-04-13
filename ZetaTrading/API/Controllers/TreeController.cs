using Microsoft.AspNetCore.Mvc;
using ZetaTrading.Models;
using ZetaTrading.API.Domain.Services;
using ZetaTrading.API.Domain.Persistence.Contexts;
using ZetaTrading.API.Domain.DTO;
using System.ComponentModel.DataAnnotations;

namespace ZetaTrading.API.Controllers
{
    public class TreeController : Controller
    {
        private INodeService _nodeService;

        public TreeController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpPost]
        [Route("/api.user.tree.get")]
        public IActionResult Get([Required] string treeName)
        {
                if (string.IsNullOrEmpty(treeName))
            {
                return NotFound();
            }

            NodeDTO? tree = _nodeService.GetRootNodeByName(treeName);

            return Json(tree);
        }
    }
}
