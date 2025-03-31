namespace Sample.Client.Models;

public class AuthResponseModel
{
    public string token { get; set; } = null!;
    public DateTime expiration { get; set; }
}
