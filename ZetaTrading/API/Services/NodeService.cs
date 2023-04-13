using ZetaTrading.API.Domain.DTO;
using ZetaTrading.API.Domain.Repositories;
using ZetaTrading.API.Domain.Services;
using ZetaTrading.Exceptions;
using ZetaTrading.Models;

namespace ZetaTrading.API.Services
{
    public class NodeService : INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private ILogger<NodeService> _logger;

        public NodeService(INodeRepository nodeRepository, ILogger<NodeService> logger)
        {
            _nodeRepository = nodeRepository;
            _logger = logger;
        }

        public NodeDTO? GetRootNodeByName(string treeName)
        {
            _logger.LogInformation("Start Getting Tree");
            List<TreeNode> allTreeNodes = _nodeRepository
                .GetNodesByParentName(treeName);
            List<NodeDTO> allNodes = allTreeNodes
                .Select(x => new NodeDTO() { Id = x.Id, Name = x.Name, Children = new List<NodeDTO>()})
                .ToList();

            NodeDTO? result = new NodeDTO();

            _logger.LogInformation("Finish Getting Tree");
            _logger.LogInformation("Start Building TreeObject to display");
            foreach (var node in allTreeNodes)
            {
                NodeDTO? current = allNodes.FirstOrDefault(x => x.Id == node.Id);
                if (node.ParentNode != null)
                {
                    var parent = allNodes.FirstOrDefault(x => x.Id == node.ParentNode.Id);
                    parent.Children.Add(current);
                }
                else
                {
                    result = current;
                }
            }

            _logger.LogInformation("Finish Building TreeObject to display");

            return result;
        }

        public void DeleteNode(string treeName, int nodeId)
        {
            _logger.LogInformation($"Start Delete node with id = {nodeId} from {treeName} tree");
            TreeNode? node = _nodeRepository.GetNodeByIdAndTreeName(treeName, nodeId);

            if (node == null)
            {
                throw new SecureException("Node is not found!");
            }

            if (!_nodeRepository.IsDeletionPossible(nodeId))
            {                
                throw new SecureException("Node cannot be deleted! It's still in use.");
            }

            _nodeRepository.DeleteNode(nodeId);

            _logger.LogInformation($"Finish Delete node with id = {nodeId} from {treeName} tree");
        }

        public void RenameNode(string treeName, int nodeId, string newName)
        {
            _logger.LogInformation($"Start Rename node with id = {nodeId} from {treeName} tree");
            TreeNode? node = _nodeRepository.GetNodeByIdAndTreeName(treeName, nodeId);

            if (node == null)
            {
               throw new SecureException("Node is not found!");
            }

            if (!_nodeRepository.IsNameUniqueWithinTheTree(treeName, newName))
            {
                throw new SecureException("Selected name already exists!");
            }

            _nodeRepository.RenameNode(nodeId, newName);
            _logger.LogInformation($"Finish Rename node with id = {nodeId} from {treeName} tree");
        }

        public void CreateNode(string treeName, int parentNodeId, string name)
        {
            _logger.LogInformation($"Start Create node with name = {name} to parentId={parentNodeId} to {treeName} tree");
            if (!_nodeRepository.IsNameUniqueWithinTheTree(treeName, name))
            {
                throw new SecureException("Selected name already exists!");
            }

            if (!_nodeRepository.DoesNodeExistInTheTree(treeName, parentNodeId))
            {
                throw new SecureException("There is no such node in the selected tree!");
            }

            _nodeRepository.CreateNewNode(name, treeName, parentNodeId);
            _logger.LogInformation($"Finish Create node with name = {name} to parentId = {parentNodeId} to {treeName} tree");
        }
    }
}
