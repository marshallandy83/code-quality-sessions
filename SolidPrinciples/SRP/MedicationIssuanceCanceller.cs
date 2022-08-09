using System;
using System.Collections.Generic;
using System.Linq;

namespace SRP
{
	public class MedicationIssuanceCanceller : IMedicationIssuanceCanceller
	{
		private readonly IMedicationIssuanceSelector _selector;

		public MedicationIssuanceCanceller(IMedicationIssuanceSelector selector) => _selector = selector;

		public void Cancel(IEnumerable<Issuance> issuances, String reasonForCancelling)
		{
			foreach (var issuance in issuances.Where(_selector.ShouldCancel))
			{
				issuance.Status = IssuanceStatus.Cancelled;
				issuance.ReasonForCancelling = reasonForCancelling;
			}
		}
	}
}
