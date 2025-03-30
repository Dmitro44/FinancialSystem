namespace FinancialSystem.Domain.Entities;

public class SalaryProjectEmployee
{
    public int Id { get; private set; }
    public int SalaryProjectId { get; private set; }
    public int UserId { get; private set; }
    public DateTime JoinDate { get; private set; }
    
    public SalaryProject SalaryProject { get; private set; }
    public User User { get; private set; }
    
    private SalaryProjectEmployee() {}
    
    public SalaryProjectEmployee(SalaryProject salaryProject, User user)
    {
        SalaryProjectId = salaryProject.Id;
        SalaryProject = salaryProject;
        UserId = user.Id;
        User = user;
        JoinDate = DateTime.UtcNow.AddHours(3);
    }
}