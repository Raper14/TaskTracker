var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<TaskService>();

var app = builder.Build();

app.MapGet("/tasks", (TaskService s) => Results.Ok(s.GetAll()));
app.MapPost("/tasks", (TaskRequest r, TaskService s) =>
{
    try { return (IResult)Results.Created("/tasks", s.Add(r.Title)); }
    catch (ArgumentException e) { return Results.BadRequest(new { error = e.Message }); }
});
app.MapPost("/tasks/{id:int}/complete", (int id, TaskService s) =>
    s.Complete(id) ? Results.Ok() : Results.NotFound());

app.Run();

public record TaskRequest(string Title);
