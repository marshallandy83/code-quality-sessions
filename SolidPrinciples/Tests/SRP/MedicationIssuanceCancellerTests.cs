using System;
using Moq;
using SRP;
using Xunit;

namespace Tests.SRP
{
	public class MedicationIssuanceCancellerTests
	{
		[Theory]
		[InlineData(true, IssuanceStatus.Cancelled, "Prescribing error")]
		[InlineData(false, IssuanceStatus.Active, null)]
		public void CancelTests(Boolean shouldCancel, IssuanceStatus expectedStatus, String expectedReasonForCancelling)
		{
			var issuance = new Issuance(IssuanceStatus.Active);
			var reasonForCancelling = "Prescribing error";

			var mockSelector = new Mock<IMedicationIssuanceSelector>();
			mockSelector.Setup(mock => mock.ShouldCancel(It.IsAny<Issuance>())).Returns(shouldCancel);

			new MedicationIssuanceCanceller(mockSelector.Object).Cancel(new[] { issuance }, reasonForCancelling);

			Assert.Equal(expectedStatus, issuance.Status);
			Assert.Equal(expectedReasonForCancelling, issuance.ReasonForCancelling);
			mockSelector.Verify(mock => mock.ShouldCancel(It.Is<Issuance>(match => match == issuance)));
		}
	}
}
