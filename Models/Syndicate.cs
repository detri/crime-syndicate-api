namespace CrimeSyndicate.Models;

public class Syndicate
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Picture { get; set; }
    public User Owner { get; set; }
    public List<User> Members { get; set; }
}