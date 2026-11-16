namespace TaskTracker.Api;

public record TaskItem(int Id, string Title, bool IsDone);

public sealed class TaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public IReadOnlyList<TaskItem> GetAll() => _tasks;

    public TaskItem Add(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название обязательно", nameof(title));

        var item = new TaskItem(_nextId++, title.Trim(), false);
        _tasks.Add(item);
        return item;
    }

    public bool Complete(int id)
    {
        var index = _tasks.FindIndex(x => x.Id == id);
        if (index < 0) return false;

        _tasks[index] = _tasks[index] with { IsDone = true };
        return true;
    }
}
