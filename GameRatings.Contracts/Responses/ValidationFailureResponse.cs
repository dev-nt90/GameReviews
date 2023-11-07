using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRatings.Contracts.Responses
{
	public class ValidationFailureResponse
	{
		public IEnumerable<ValidationResponse> Errors { get; init; }
	}
}
