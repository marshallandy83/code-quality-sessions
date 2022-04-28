using System;

namespace SRP
{
	public class ActiveAndLocalIssuancesSelector : IMedicationIssuanceSelector
	{
		public Boolean ShouldCancel(Issuance issuance) =>
			issuance.Status == IssuanceStatus.Active && issuance.AddedBy == Source.Local;
	}
}
