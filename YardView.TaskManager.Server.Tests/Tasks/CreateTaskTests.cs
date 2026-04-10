using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using YardView.TaskManager.Server.Contracts.Tasks;
using YardView.TaskManager.Server.Tests.Infrastructure;

namespace YardView.TaskManager.Server.Tests.Tasks;

public class CreateTaskTests
{
    CustomWebApplicationFactory _factory;

    public CreateTaskTests()
    {
        _factory = new CustomWebApplicationFactory();
    }

    [Fact]
    public async Task CreateTask_ReturnsCreated_ForValidRequest()
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
        Assert.Equal(request.Title, createdTask.Title);
        Assert.Equal(request.Description, createdTask.Description);
    }

    [Fact]
    public async Task CreateTask_ReturnsBadRequest_WhenTitleMissing()
    {
        var client = _factory.CreateClient();

        var request = new
        {
            Description = "This task has no title."
        };

        var response = await client.PostAsJsonAsync("/tasks", request);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateTask_ReturnsBadRequest_WhenStatusInvalid()
    {
        var client = _factory.CreateClient();

        var request = new
        {
            Title = "Test Task",
            Description = "This task has an invalid status.",
            Status = "InvalidStatus"
        };

        var response = await client.PostAsJsonAsync("/tasks", request);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

}
