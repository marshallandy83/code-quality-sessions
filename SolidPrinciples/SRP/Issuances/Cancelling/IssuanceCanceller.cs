using System;
using System.Collections.Generic;
using System.Linq;
using SRP.Issuances.Selection;

namespace SRP.Issuances.Cancelling
{
	public class IssuanceCanceller : IIssuanceCanceller
	{
		private readonly IIssuanceSelector _selector;

		public IssuanceCanceller(IIssuanceSelector selector) => _selector = selector;

		public void Cancel(IEnumerable<Issuance> issuances, String reasonForCancelling)
		{
			foreach (var issuance in issuances.Where(_selector.ShouldCancel))
			{
				issuance.Status = Status.Cancelled;
				issuance.ReasonForCancelling = reasonForCancelling;
			}
		}
	}
}
