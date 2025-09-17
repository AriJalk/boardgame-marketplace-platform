using BoardGameMarket.ListingsService.Core.Domain.Common;

namespace BoardGameMarket.ListingsService.API.Helpers
{
	internal static class CursorParser
	{
		public static Cursor? Parse(string? base64CursorString)
		{
			Cursor? cursorObject = null;
			if (base64CursorString != null)
			{
				byte[] data = Convert.FromBase64String(base64CursorString);
				string decodedCursor = System.Text.Encoding.UTF8.GetString(data);
				var parts = decodedCursor.Split('|');
				if (parts.Length == 2 &&
					DateTime.TryParse(parts[0], null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime dt) &&
					Guid.TryParse(parts[1], out Guid id))
				{
					cursorObject = new Cursor(dt, id);
				}
				else
				{
					throw new ArgumentException("Invalid cursor");
				}
			}
			return cursorObject;
		}
	}
}
