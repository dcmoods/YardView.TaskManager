using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using YardView.TaskManager.Server.Contracts.Tasks;
using YardView.TaskManager.Server.Tests.Infrastructure;

namespace YardView.TaskManager.Server.Tests.Tasks
{
    public class GetTaskTests
    {
        CustomWebApplicationFactory _factory;

        public GetTaskTests()
        {
            _factory = new CustomWebApplicationFactory();
        }

        [Fact]
        public async Task GetTasks_FilterByStatus()
        {
            var client = _factory.CreateClient();

            var request = new
            {
                Title = "Test Task",
                Description = "This is a test task."
            };

            var response = await client.PostAsJsonAsync("/tasks", request);

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

            var createdTask = await response.Content.ReadFromJsonAsync<TaskResponse>();

            Assert.NotNull(createdTask);

            var getResponse = await client.GetAsync("/tasks?status=todo");

            Assert.Equal(System.Net.HttpStatusCode.OK, getResponse.StatusCode);

            var tasks = await getResponse.Content.ReadFromJsonAsync<List<TaskResponse>>();

            Assert.NotNull(tasks);
            Assert.Single(tasks);
        }
    }
}
