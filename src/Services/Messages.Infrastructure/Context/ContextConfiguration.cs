namespace Messages.Infrastructure.Context;

public class ContextConfiguration
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}