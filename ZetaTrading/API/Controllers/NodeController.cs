using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ZetaTrading.API.Domain.Services;

namespace ZetaTrading.API.Controllers
{
    public class NodeController : Controller
    {
        private INodeService _nodeService;

        public NodeController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpPost]
        [Route("/api.user.tree.node.create")]
        public IActionResult Create([Required] string treeName, [Required] int parentNodeId, [Required] string nodeName)
        {
            _nodeService.CreateNode(treeName, parentNodeId, nodeName);

            return Ok();
        }

        [HttpPost]
        [Route("/api.user.tree.node.rename")]
        public IActionResult Rename([Required] string treeName, [Required] int nodeId, [Required] string newNodeName)
        {
            _nodeService.RenameNode(treeName, nodeId, newNodeName);

            return Ok();
        }

        [HttpPost]
        [Route("/api.user.tree.node.delete")]
        public IActionResult Delete([Required] string treeName, [Required] int nodeId)
        {
            _nodeService.DeleteNode(treeName, nodeId);

            return Ok();
        }
    }
}
