using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models.Client;

public class ClientSalaryProjectViewModel
{
    public List<SalaryProject> AvailableSalaryProjects = new();
    public List<SalaryProjectEmployee> ConnectedSalaryProjects = new();
}