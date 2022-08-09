using System;
using SRP;
using Xunit;

namespace Tests.SRP
{
	public class ActiveIssuancesSelectorTests
	{
		[Theory]
		[InlineData(IssuanceStatus.Active, true)]
		[InlineData(IssuanceStatus.Cancelled, false)]
		public void ShouldCancelTests(IssuanceStatus status, Boolean expectedShouldCancel) =>
			Assert.Equal(
				expectedShouldCancel,
				new ActiveIssuancesSelector()
					.ShouldCancel(
						new Issuance(status)));
	}
}
