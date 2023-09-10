namespace Point.Of.Sale.Shared.Configuration;

public record Smtp
{
    public string Server { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
}
