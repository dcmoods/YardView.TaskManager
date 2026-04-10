using System;
using System.Collections.Generic;
using System.Text;
using YardView.TaskManager.Server.Tests.Infrastructure;

namespace YardView.TaskManager.Server.Tests.Tasks
{
    public class DeleteTaskTests
    {
        CustomWebApplicationFactory _factory;

        public DeleteTaskTests()
        {
            _factory = new CustomWebApplicationFactory();
        }

        [Fact]
        public async Task DeleteTask_ReturnsNotFound_WhenTaskMissing()
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/tasks/999");
            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

        }
    }
}
