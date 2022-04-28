using System;
using SRP;
using Xunit;

namespace Tests.SRP
{
	public class ActiveAndLocalIssuancesSelectorTests
	{
		[Theory]
		[InlineData(Source.Local, IssuanceStatus.Active, true)]
		[InlineData(Source.Local, IssuanceStatus.Cancelled, false)]
		[InlineData(Source.External, IssuanceStatus.Active, false)]
		public void ShouldCancelTests(Source addedBy, IssuanceStatus status, Boolean expectedShouldCancel) =>
			Assert.Equal(
				expectedShouldCancel,
				new ActiveAndLocalIssuancesSelector()
					.ShouldCancel(
						new Issuance(status, addedBy)));
	}
}
