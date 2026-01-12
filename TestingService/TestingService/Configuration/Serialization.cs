using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestingService.Configuration
{
	public static class Serialization
	{
		public static void ConfigureJson(this JsonSerializerOptions options)
		{
			options.PropertyNameCaseInsensitive = true;

			options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
		}
	}
}
