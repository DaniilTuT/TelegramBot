using Quartz;

namespace Infrastructure1.Jobs;

public class TestJob: IJob
{
    public Task Execute(IJobExecuteContext context)
    {
        Console.WriteLine();
        return Task.CompletedTask;
    }
}