using System;

namespace SRP.Issuances.Selection
{
	public class ActiveAndLocalIssuancesSelector : IIssuanceSelector
	{
		public Boolean ShouldCancel(Issuance issuance) =>
			issuance.Status == Status.Active && issuance.AddedBy == Source.Local;
	}
}
