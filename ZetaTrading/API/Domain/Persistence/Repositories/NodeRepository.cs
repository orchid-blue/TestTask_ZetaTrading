using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Text.Json.Nodes;
using ZetaTrading.API.Domain.Persistence.Contexts;
using ZetaTrading.API.Domain.Repositories;
using ZetaTrading.Models;

namespace ZetaTrading.API.Domain.Persistence.Repositories
{
    public class NodeRepository : BaseRepository, INodeRepository
    {
        public NodeRepository(AppDbContext context) : base(context)
        {

        }

        public TreeNode? GetRootNodeByName(string treeName)
        {
            return _context.TreeNodes
                .FirstOrDefault(x => x.ParentNode == null && string.Equals(x.Name, treeName));
        }

        public List<TreeNode> GetNodesByParentName(string parentName)
        {
            List<TreeNode> nodes = _context.TreeNodes
                .Where(x => string.Equals(x.TreeName, parentName)).ToList();

            if (nodes.Count == 0)
            {
                return new List<TreeNode>() { CreateNewNode(parentName, parentName)};
            }

            return nodes;
        }

        public TreeNode CreateNewNode(string nodeName, string treeName, int? parentNodeId = null)
        {
            TreeNode? parentNode = _context.TreeNodes.Find(parentNodeId);
            TreeNode newNode = new TreeNode() { Name = nodeName, TreeName = treeName, ParentNode = parentNode};

            _context.TreeNodes.Add(newNode);
            _context.SaveChanges();

            return newNode;
        }

        public void RenameNode(int nodeId, string newName)
        {
            TreeNode? node = _context.TreeNodes.Find(nodeId);
            if (node != null)
            {
                node.Name = newName;
                _context.TreeNodes.Update(node);
                _context.SaveChanges();
            }
        }

        public void DeleteNode(int nodeId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    TreeNode? node = _context.TreeNodes.Find(nodeId);
                    if (node != null)
                    {
                        _context.TreeNodes.Remove(node);
                        _context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
           
        }

        public TreeNode? GetNodeByIdAndTreeName(string treeName, int nodeId)
        {
            return _context.TreeNodes.FirstOrDefault(x => string.Equals(x.TreeName, treeName) && x.Id == nodeId);
        }

        public bool IsNameUniqueWithinTheTree(string treeName, string name)
        {
            return !_context.TreeNodes
                .Where(x => string.Equals(x.TreeName, treeName) && string.Equals(x.Name, name))
                .Any();
        }

        public bool DoesNodeExistInTheTree(string treeName, int nodeId)
        {
            return _context.TreeNodes
                .Any(x => string.Equals(x.TreeName, treeName) && x.Id == nodeId);                
        }

        public bool IsDeletionPossible(int nodeId)
        {
            return !_context.TreeNodes
                .Any(x => x.ParentNode != null && x.ParentNode.Id == nodeId);
        }
    }
}
