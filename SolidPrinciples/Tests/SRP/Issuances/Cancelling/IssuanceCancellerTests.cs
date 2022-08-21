using System;
using Moq;
using SRP.Issuances.Selection;
using Xunit;

namespace SRP.Issuances.Cancelling
{
	public class IssuanceCancellerTests
	{
		[Theory]
		[InlineData(true, Status.Cancelled, "Prescribing error")]
		[InlineData(false, Status.Active, null)]
		public void CancelTests(Boolean shouldCancel, Status expectedStatus, String expectedReasonForCancelling)
		{
			var issuance = new Issuance(Status.Active, Source.Local);
			var reasonForCancelling = "Prescribing error";

			var mockSelector = new Mock<IIssuanceSelector>();
			mockSelector.Setup(mock => mock.ShouldCancel(It.IsAny<Issuance>())).Returns(shouldCancel);

			new IssuanceCanceller(mockSelector.Object).Cancel(new[] { issuance }, reasonForCancelling);

			Assert.Equal(expectedStatus, issuance.Status);
			Assert.Equal(expectedReasonForCancelling, issuance.ReasonForCancelling);
			mockSelector.Verify(mock => mock.ShouldCancel(It.Is<Issuance>(match => match == issuance)));
		}
	}
}
