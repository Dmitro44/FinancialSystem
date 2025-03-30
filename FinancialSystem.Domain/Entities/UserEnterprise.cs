namespace FinancialSystem.Domain.Entities;

public class UserEnterprise
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public User User { get; private set; }
    public int EnterpriseId { get; private set; }
    public Enterprise Enterprise { get; private set; }
    
    public UserEnterprise() {}

    public UserEnterprise(User user, Enterprise enterprise)
    {
        User = user;
        UserId = user.Id;
        Enterprise = enterprise;
        EnterpriseId = enterprise.Id;
    }
}