using System;
using System.Linq;
using SRP.Issuances;
using Xunit;

namespace SRP.Courses.Selection
{
	public class ActiveCoursesSelectorTests
	{
		[Theory]
		[InlineData(Status.Active, true)]
		[InlineData(Status.Ended, false)]
		public void ShouldEndTests(Status status, Boolean expectedShouldEnd) =>
			Assert.Equal(
				expectedShouldEnd,
				new ActiveCoursesSelector()
					.ShouldEnd(
						new Course(
							preparationTerm: String.Empty,
							status: status,
							issuances: Enumerable.Empty<Issuance>(),
							Source.Local)));
	}
}
