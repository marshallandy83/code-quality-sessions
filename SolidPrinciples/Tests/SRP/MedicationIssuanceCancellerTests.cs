using System;
using SRP;
using Xunit;

namespace Tests.SRP
{
	public class MedicationIssuanceCancellerTests
	{
		[Theory]
		[InlineData(IssuanceStatus.Active, IssuanceStatus.Cancelled, "Prescribing error")]
		[InlineData(IssuanceStatus.Cancelled, IssuanceStatus.Cancelled, null)]
		public void CancelTests(IssuanceStatus status, IssuanceStatus expectedStatus, String expectedReasonForCancelling)
		{
			var issuance = new Issuance(status);
			var reasonForCancelling = "Prescribing error";

			new MedicationIssuanceCanceller().Cancel(new[] { issuance }, reasonForCancelling);

			Assert.Equal(expectedStatus, issuance.Status);
			Assert.Equal(expectedReasonForCancelling, issuance.ReasonForCancelling);
		}
	}
}
