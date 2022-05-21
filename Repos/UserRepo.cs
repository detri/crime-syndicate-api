using CrimeSyndicate.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimeSyndicate.Repos;

public class UserRepo
{
    private readonly DbSet<User> _users;

    public UserRepo(DbSet<User> users)
    {
        _users = users;
    }

    public async Task<User?> FindUserByName(string name)
    {
        return await _users.Where(u => u.Name == name).FirstOrDefaultAsync();
    }

    public void Add(User user)
    {
        _users.Add(user);
    }
}
