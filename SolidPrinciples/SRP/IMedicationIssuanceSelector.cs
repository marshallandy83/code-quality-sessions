using System;

namespace SRP
{
	public interface IMedicationIssuanceSelector
	{
		Boolean ShouldCancel(Issuance issuance);
	}
}
