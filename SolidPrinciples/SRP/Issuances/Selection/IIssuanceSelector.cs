using System;

namespace SRP.Issuances.Selection
{
	public interface IIssuanceSelector
	{
		Boolean ShouldCancel(Issuance issuance);
	}
}
