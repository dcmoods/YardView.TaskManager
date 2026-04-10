using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using YardView.TaskManager.Server.Tests.Infrastructure;

namespace YardView.TaskManager.Server.Tests.Tasks
{
    public class UpdateTaskTests
    {
        CustomWebApplicationFactory _factory;

        public UpdateTaskTests()
        {
            _factory = new CustomWebApplicationFactory();
        }

        [Fact]
        public async Task UpdateTask_ReturnsNotFound_WhenTaskMissing()
        {
            var client = _factory.CreateClient();

            var response = await client.PutAsJsonAsync("/tasks/999", new
            {
                Title = "Updated Task",
                Description = "Updated Description",
            });

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
