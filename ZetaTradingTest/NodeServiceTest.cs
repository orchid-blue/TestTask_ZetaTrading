using FakeItEasy;
using Microsoft.Extensions.Logging;
using ZetaTrading.API.Domain.Repositories;
using ZetaTrading.API.Domain.Services;
using ZetaTrading.API.Services;
using ZetaTrading.Models;

namespace ZetaTradingTest
{
    public class NodeServiceTest
    {
        [Fact]
        public void GetRootNodeByName_Returns_The_Correct_DTO_Object_Test()
        {
            //Arrange
            var fakeNodeRepository = A.Fake<INodeRepository>();
            var fakeNodeLogger = A.Fake<ILogger<NodeService>>();
            INodeService service = new NodeService(fakeNodeRepository, fakeNodeLogger);
            string treeName = "tree";

            List<TreeNode> nodes = new List<TreeNode>()
            {
                new TreeNode() { Id = 1, Name = treeName, TreeName = treeName },
                new TreeNode() { Id = 2, Name = "child1", TreeName = treeName },
                new TreeNode() { Id = 3, Name = "child2", TreeName = treeName },
                new TreeNode() { Id = 4, Name = "child3", TreeName = treeName },
                new TreeNode() { Id = 5, Name = "child4", TreeName = treeName }
            };

            nodes.ElementAt(1).ParentNode = nodes.ElementAt(0);
            nodes.ElementAt(2).ParentNode = nodes.ElementAt(0);
            nodes.ElementAt(3).ParentNode = nodes.ElementAt(2);
            nodes.ElementAt(4).ParentNode = nodes.ElementAt(2);

            A.CallTo(() => fakeNodeRepository.GetNodesByParentName(treeName)).Returns(nodes);

            //Act
            var result = service.GetRootNodeByName(treeName);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(treeName, result.Name);
            Assert.Equal(2, result.Children.Count);
            Assert.Equal(3, result.Children.ElementAt(1).Id);
            Assert.Equal(2, result.Children.ElementAt(1).Children.Count);
        }

        //[Fact]
        //public void DeleteTest()
        //{
        //    //Arrange
        //    var fakeNodeRepository = A.Fake<INodeRepository>();
        //    var fakeNodeLogger = A.Fake<ILogger<NodeService>>();
        //    string treeName = "tree";

        //    List<TreeNode> nodes = new List<TreeNode>()
        //    {
        //        new TreeNode() { Id = 1, Name = treeName, TreeName = treeName },
        //        new TreeNode() { Id = 2, Name = "child1", TreeName = treeName },
        //        new TreeNode() { Id = 3, Name = "child2", TreeName = treeName },
        //        new TreeNode() { Id = 4, Name = "child3", TreeName = treeName },
        //        new TreeNode() { Id = 5, Name = "child4", TreeName = treeName }
        //    };

        //    nodes.ElementAt(1).ParentNode = nodes.ElementAt(0);
        //    nodes.ElementAt(2).ParentNode = nodes.ElementAt(0);
        //    nodes.ElementAt(3).ParentNode = nodes.ElementAt(2);
        //    nodes.ElementAt(4).ParentNode = nodes.ElementAt(2);

        //    A.CallTo(() => fakeNodeRepository.GetNodeByIdAndTreeName("newTree", 3)).Returns(nodes.ElementAt(2));
        //    A.CallTo(() => fakeNodeRepository.IsDeletionPossible(3)).Returns(false);

        //    INodeService service = new NodeService(fakeNodeRepository, fakeNodeLogger);

        //    //Act
        //    var ex = Assert.Throws<SecureException>(() => service.DeleteNode(treeName, 3));

        //    //Assert
        //    Assert.Equal("Node cannot be deleted! It's still in use.", ex.Message);
        //}

        //[Fact]
        //public void DeleteNode()
        //{
        //    //Arrange
        //    var fakeNodeRepository = A.Fake<INodeRepository>();
        //    var fakeNodeLogger = A.Fake<ILogger<NodeService>>();
        //    A.CallTo(() => fakeNodeRepository.GetNodeByIdAndTreeName("newTree", 3)).Returns(null);

        //    INodeService service = new NodeService(fakeNodeRepository, fakeNodeLogger);

        //    //Act
        //    var ex = Assert.Throws<SecureException>(() => service.DeleteNode("newTree", 3));

        //    //Assert
        //    Assert.Equal("Node is not found!", ex.Message);
        //}
    }
}