using System;
using System.Linq;
using SRP.Issuances;
using Xunit;

namespace SRP.Courses.Selection
{
	public class ActiveAndLocalCoursesSelectorTests
	{
		[Theory]
		[InlineData(Source.Local, Status.Active, true)]
		[InlineData(Source.Local, Status.Ended, false)]
		[InlineData(Source.External, Status.Active, false)]
		public void ShouldEndTests(Source addedBy, Status status, Boolean expectedShouldEnd) =>
			Assert.Equal(
				expectedShouldEnd,
				new ActiveAndLocalCoursesSelector()
					.ShouldEnd(
						new Course(
							preparationTerm: String.Empty,
							status: status,
							issuances: Enumerable.Empty<Issuance>(),
							addedBy: addedBy)));
	}
}
