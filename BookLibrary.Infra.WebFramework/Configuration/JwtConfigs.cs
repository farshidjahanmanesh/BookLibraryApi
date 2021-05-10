namespace BookLibrary.Infra.WebFramework.Configuration
{
    public class JwtConfigs
    {
        public const string JwtAuth = "JwtAuth";
        public string Key { get; set; }
        public string Issuer { get; set; }
        public int ExpireMin { get; set; }
    }
}
