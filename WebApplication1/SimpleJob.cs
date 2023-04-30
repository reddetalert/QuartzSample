using Quartz;

public class SimpleJob : IJob
{
    private readonly IUser _userService;

    public SimpleJob(IUser userService)
    {
        _userService = userService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = await _userService.GetUserName();
        Console.WriteLine(user);
    }
}