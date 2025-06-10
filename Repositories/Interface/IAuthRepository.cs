namespace project_API.Repositories.Interface;


    public interface IAuthRepository
    {
        Task<bool> Register(User user);
        Task<bool> Login(string phoneNumber, string code);
        Task<bool> SendVerificationCode( string phoneNumber);
        Task<bool> CheckUser(string phoneNumber);
        Task<List<User>> GetAllUsers();
    }

