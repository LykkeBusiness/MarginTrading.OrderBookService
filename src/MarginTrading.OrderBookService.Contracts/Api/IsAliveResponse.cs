namespace Lykke.MarginTrading.OrderBookService.Contracts.Api
{
    public class IsAliveResponse
    {
        public string Version { get; set; }
        public string Env { get; set; }
        public bool IsDebug { get; set; }
        public string Name { get; set; }
    }
}