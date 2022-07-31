using MudWasm.Shared;

namespace MudWasm.Server.Authentication
{
    public class UserService
    {
        private List<User> _users;

        public UserService()
        {
            _users = new List<User>() { 
                new User {Username="admin", Password="admin", Role=UserRole.Admin },
                new User {Username="user", Password="user", Role=UserRole.User }
            };
        }

        public User? GetUser(string username, string password)
        {
            return _users.FirstOrDefault(x => x.Username == username || x.Password == password);
        }
    }
}
