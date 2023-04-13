using ZetaTrading.Models;

namespace ZetaTrading.API.Domain.Repositories
{
    public interface INodeRepository
    {
        TreeNode? GetRootNodeByName(string treeName);
        List<TreeNode> GetNodesByParentName(string parentName);
        TreeNode CreateNewNode(string nodeName, string treeName, int? parentNodeId = null);
        void RenameNode(int nodeId, string newName);
        void DeleteNode(int nodeId);
        TreeNode? GetNodeByIdAndTreeName(string treeName, int nodeId);
        bool IsNameUniqueWithinTheTree(string treeName, string name);
        bool DoesNodeExistInTheTree(string treeName, int nodeId);
        bool IsDeletionPossible(int nodeId);
    }
}
