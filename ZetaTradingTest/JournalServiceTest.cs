using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZetaTrading.API.Domain.Repositories;
using ZetaTrading.API.Domain.Services;
using ZetaTrading.API.Services;
using ZetaTrading.Exceptions;
using ZetaTrading.Models;

namespace ZetaTradingTest
{
    public class JournalServiceTest
    {
        [Fact]
        public void GetSingleRecordById_Returns_Correct_DTO_Object_Test()
        {
            //Arrange
            var fakeJournalRepository = A.Fake<IJournalRecordRepository>();
            IJournalRecordService service = new JournalRecordService(fakeJournalRepository);
            int id = 1;

            JournalRecord rec = new JournalRecord()
            {
                Id = id,
                RequestId = "Request id",
                CreatedDate = DateTime.Now,
                StackTrace = "Stack Trace",
                Path = "path",
                QueryParameters = "params",
                BodyParameters = "body"
            };

            string text = $@"  Request ID = {rec.RequestId}
                            Path = {rec.Path}
                            Params = {string.Join("\n\r", value: rec.QueryParameters)}
                            StackTrace = {rec.StackTrace}";

            A.CallTo(() => fakeJournalRepository.GetJournalRecordById(id)).Returns(rec);            

            //Act
            var result = service.GetSingleRecordById(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(rec.CreatedDate, result.CreatedAt);
            Assert.Equal(rec.RequestId, result.EventId);
            Assert.Equal(text, result.Text);
        }

        [Fact]
        public void GetExceptionToDisplay_Returns_Correct_DTO_Object_Test()
        {
            //Arrange
            var fakeJournalRepository = A.Fake<IJournalRecordRepository>();
            IJournalRecordService service = new JournalRecordService(fakeJournalRepository);

            var fakeNodeRepository = A.Fake<INodeRepository>();
            var fakeNodeLogger = A.Fake<ILogger<NodeService>>();
            INodeService nodeService = new NodeService(fakeNodeRepository, fakeNodeLogger);

            A.CallTo(() => fakeNodeRepository.GetNodeByIdAndTreeName("newTree", 3)).Returns(null);

            var ex = Assert.Throws<SecureException>(() => nodeService.DeleteNode("newTree", 3));

            //Act
            var result = service.GetExceptionToDisplay(ex);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("secure", result.Type);
            Assert.Equal(string.Empty, result.Id);
            Assert.Equal("Node is not found!", result.Data.Message);
        }

        [Fact]
        public void GetRangeRecords_Returns_Correct_DTO_Object()
        {
            //Arrange
            var fakeJournalRepository = A.Fake<IJournalRecordRepository>();
            IJournalRecordService service = new JournalRecordService(fakeJournalRepository);

            List<JournalRecord> rec = new List<JournalRecord>()
            {
                new JournalRecord() { Id = 1, RequestId = "Request 1", CreatedDate = DateTime.Now },
                new JournalRecord() { Id = 2, RequestId = "Request 2", CreatedDate = DateTime.Now },
                new JournalRecord() { Id = 3, RequestId = "Request 3", CreatedDate = DateTime.Now },
                new JournalRecord() { Id = 4, RequestId = "Request 4", CreatedDate = DateTime.Now }
              };

            A.CallTo(() => fakeJournalRepository.GetRangeJournalRecords(0, 4, null, null)).Returns(rec);

            //Act
            var result = service.GetRangeRecords(0, 4, new ZetaTrading.API.Domain.DTO.FilterDTO());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Skip);
            Assert.Equal(4, result.Count);
            Assert.Equal(4, result.Items.Count);
        }
    }
}
