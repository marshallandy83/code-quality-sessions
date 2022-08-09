using System;

namespace SRP
{
	public class ActiveIssuancesSelector : IMedicationIssuanceSelector
	{
		public Boolean ShouldCancel(Issuance issuance) => issuance.Status == IssuanceStatus.Active;
	}
}
