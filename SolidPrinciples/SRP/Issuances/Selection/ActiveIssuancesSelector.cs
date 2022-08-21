using System;

namespace SRP.Issuances.Selection
{
	public class ActiveIssuancesSelector : IIssuanceSelector
	{
		public Boolean ShouldCancel(Issuance issuance) => issuance.Status == Status.Active;
	}
}
