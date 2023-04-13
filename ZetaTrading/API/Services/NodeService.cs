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

        public NodeService(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        public NodeDTO? GetRootNodeByName(string treeName)
        {
            List<TreeNode> allTreeNodes = _nodeRepository
                .GetNodesByParentName(treeName);
            List<NodeDTO> allNodes = allTreeNodes
                .Select(x => new NodeDTO() { Id = x.Id, Name = x.Name, Children = new List<NodeDTO>()})
                .ToList();

            NodeDTO? result = new NodeDTO();

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

            return result;
        }

        public void DeleteNode(string treeName, int nodeId)
        {
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
        }

        public void RenameNode(string treeName, int nodeId, string newName)
        {
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
        }

        public void CreateNode(string treeName, int parentNodeId, string name)
        {
            if (!_nodeRepository.IsNameUniqueWithinTheTree(treeName, name))
            {
                throw new SecureException("Selected name already exists!");
            }

            if (!_nodeRepository.DoesNodeExistInTheTree(treeName, parentNodeId))
            {
                throw new SecureException("There is no such node in the selected tree!");
            }

            _nodeRepository.CreateNewNode(name, treeName, parentNodeId);
        }
    }
}
