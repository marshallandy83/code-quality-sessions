using System;
using System.Collections.Generic;

namespace SRP.Issuances.Cancelling
{
	public interface IIssuanceCanceller
	{
		void Cancel(IEnumerable<Issuance> issuances, String reasonForCancelling);
	}
}
