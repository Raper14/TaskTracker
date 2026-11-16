namespace TaskTracker.Tests;

[TestClass]
public class TaskServiceTests
{
    [TestMethod]
    public void Add_CreatesOpenTask()
    {
        var s = new TaskService();
        var task = s.Add("Сдать отчет");

        Assert.AreEqual(1, task.Id);
        Assert.IsFalse(task.IsDone);
    }

    [TestMethod]
    public void Add_RejectsBlankTitle() =>
        Assert.ThrowsException<ArgumentException>(() => new TaskService().Add(" "));

    [TestMethod]
    public void Complete_UpdatesExistingTask()
    {
        var s = new TaskService();
        s.Add("Сдать отчет");

        Assert.IsTrue(s.Complete(1));
        Assert.IsTrue(s.GetAll()[0].IsDone);
    }

    [TestMethod]
    public void Complete_UnknownIdReturnsFalse() =>
        Assert.IsFalse(new TaskService().Complete(99));
}
