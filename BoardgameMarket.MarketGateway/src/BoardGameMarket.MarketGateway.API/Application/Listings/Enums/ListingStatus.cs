namespace BoardGameMarket.MarketGateway.API.Application.Listings.Enums
{
	public enum ListingStatus
	{
		Active, // general active status
		Reserved, // awaiting confirmation for head of queue
		Sold,
		Cancelled
	}
}
