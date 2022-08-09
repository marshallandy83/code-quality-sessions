using System;
using System.Linq;
using SRP;
using Xunit;

namespace Tests.SRP
{
	public class ActiveCoursesSelectorTests
	{
		[Theory]
		[InlineData(CourseStatus.Active, true)]
		[InlineData(CourseStatus.Ended, false)]
		public void ShouldEndTests(CourseStatus status, Boolean expectedShouldEnd) =>
			Assert.Equal(
				expectedShouldEnd,
				new ActiveCoursesSelector()
					.ShouldEnd(
						new MedicationCourse(
							preparationTerm: String.Empty,
							status: status,
							issuances: Enumerable.Empty<Issuance>())));
	}
}
