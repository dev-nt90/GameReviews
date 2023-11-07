using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRatings.Contracts.Responses
{
	public class ValidationResponse
	{
		public required string PropertyName { get; init; }
		public required string Message { get; init; }
	}
}
