namespace FinancialSystem.Domain.Entities;

public class BaseEnterprise
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }

    protected BaseEnterprise() { }
    protected BaseEnterprise(string name, string address)
    {
        Name = name;
        Address = address;
    }
}