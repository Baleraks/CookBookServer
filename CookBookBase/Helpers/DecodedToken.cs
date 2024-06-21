namespace CookBookBase.Helpers
{
    public record DecodedToken(string KeyId,
        string Issuer,
        List<string> Audience,
        List<(string Type, string value)> Claims,
        DateTime Expiration,
        string SigningAlgorithm,
        string RawData,
        string Subject,
        DateTime ValidForm,
        string Header,
        string Payload);
}
