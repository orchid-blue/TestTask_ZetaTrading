using ZetaTrading.API.Domain.DTO;

namespace ZetaTrading.API.Domain.Services
{
    public interface INodeService
    {
        NodeDTO? GetRootNodeByName(string treeName);
        void DeleteNode(string treeName, int nodeId);
        void RenameNode(string treeName, int nodeId, string newName);
        void CreateNode(string treeName, int parentNodeId, string name);
    }
}
