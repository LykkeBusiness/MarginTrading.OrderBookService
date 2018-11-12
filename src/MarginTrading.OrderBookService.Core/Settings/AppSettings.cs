using JetBrains.Annotations;

namespace MarginTrading.OrderBookService.Core.Settings
{
    [UsedImplicitly]
    public class AppSettings
    {
        public OrderBookServiceSettings OrderBookService { get; set; }
    }
}
