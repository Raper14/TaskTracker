using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskTracker.Api;

namespace TaskTracker.Tests;

[TestClass]
public class ApiTests
{
    private readonly WebApplicationFactory<TaskRequest> _factory;
    private readonly HttpClient _client;

    public ApiTests()
    {
        _factory = new WebApplicationFactory<TaskRequest>();
        _client = _factory.CreateClient();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestMethod]
    public async Task Tasks_EndToEnd()
    {
        var initial = await _client.GetFromJsonAsync<TaskItem[]>("/tasks") ?? [];
        Assert.AreEqual(0, initial.Length);

        var blank = await _client.PostAsJsonAsync("/tasks", new TaskRequest(" "));
        Assert.AreEqual(HttpStatusCode.BadRequest, blank.StatusCode);

        var created = await _client.PostAsJsonAsync("/tasks", new TaskRequest("Сдать отчет"));
        Assert.AreEqual(HttpStatusCode.Created, created.StatusCode);
        var task = await created.Content.ReadFromJsonAsync<TaskItem>()
            ?? throw new AssertFailedException("Ответ не содержит задачу");
        Assert.IsFalse(task.IsDone);

        var completed = await _client.PostAsync($"/tasks/{task.Id}/complete", null);
        Assert.AreEqual(HttpStatusCode.OK, completed.StatusCode);

        var missing = await _client.PostAsync("/tasks/999/complete", null);
        Assert.AreEqual(HttpStatusCode.NotFound, missing.StatusCode);

        var tasks = await _client.GetFromJsonAsync<TaskItem[]>("/tasks") ?? [];
        Assert.AreEqual(1, tasks.Length);
        Assert.IsTrue(tasks[0].IsDone);
    }
}
