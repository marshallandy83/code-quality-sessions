using System;
using Xunit;

namespace SRP.Issuances.Selection
{
	public class ActiveAndLocalIssuancesSelectorTests
	{
		[Theory]
		[InlineData(Source.Local, Status.Active, true)]
		[InlineData(Source.Local, Status.Cancelled, false)]
		[InlineData(Source.External, Status.Active, false)]
		public void ShouldCancelTests(Source addedBy, Status status, Boolean expectedShouldCancel) =>
			Assert.Equal(
				expectedShouldCancel,
				new ActiveAndLocalIssuancesSelector()
					.ShouldCancel(
						new Issuance(status, addedBy)));
	}
}
