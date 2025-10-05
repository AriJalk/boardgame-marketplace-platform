namespace BoardGameMarket.UsersService.Core.Application.DTOs
{
	public record UserUpdateRequest(string? FirstName, string? LastName, string? DisplayName) { }
}
