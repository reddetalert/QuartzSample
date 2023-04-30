public class UserService : IUser
{
    public async Task<string> GetUserName()
    {
        return "Eric" + DateTime.Now;
    }
}