using System;
using System.Collections.Generic;
using System.Linq;

namespace SRP
{
	public class MedicationIssuanceCanceller : IMedicationIssuanceCanceller
	{
		public void Cancel(IEnumerable<Issuance> issuances, String reasonForCancelling)
		{
			foreach (var issuance in issuances.Where(i => i.Status == IssuanceStatus.Active))
			{
				issuance.Status = IssuanceStatus.Cancelled;
				issuance.ReasonForCancelling = reasonForCancelling;
			}
		}
	}
}
