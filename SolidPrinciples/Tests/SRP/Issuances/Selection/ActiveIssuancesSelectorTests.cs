using System;
using Xunit;

namespace SRP.Issuances.Selection
{
	public class ActiveIssuancesSelectorTests
	{
		[Theory]
		[InlineData(Status.Active, true)]
		[InlineData(Status.Cancelled, false)]
		public void ShouldCancelTests(Status status, Boolean expectedShouldCancel) =>
			Assert.Equal(
				expectedShouldCancel,
				new ActiveIssuancesSelector()
					.ShouldCancel(
						new Issuance(status, Source.Local)));
	}
}
