namespace BoardGameMarket.MarketOrchestrator.BusinessLogic.DTOs.ServiceEntities
{
	public class BggItem
	{
		public int BggId { get; set; }
		public string? Name { get; set; }
		public IEnumerable<string> Categories { get; set; } = new List<string>();
	}
}
