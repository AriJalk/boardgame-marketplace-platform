using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Common
{
	public class Result
	{
		public bool IsValid { get; }
		public IEnumerable<string> Errors { get; }

		private Result(bool isValid, IEnumerable<string>? errors = null)
		{
			IsValid = isValid;
			Errors = errors ?? Enumerable.Empty<string>();
		}

		public static Result Success()
		{
			return new Result(true);
		}

		public static Result Failure(IEnumerable<string>? errors)
		{
			return new Result(false, errors);
		}

		public static Result Failure(params string[] errors)
		{
			return new Result(false, errors);
		}
	}
}
