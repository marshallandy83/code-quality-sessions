using System;
using System.Collections.Generic;

namespace SRP
{
	public interface IMedicationIssuanceCanceller
	{
		void Cancel(IEnumerable<Issuance> issuances, String reasonForCancelling);
	}
}
