using System;
using System.Linq;
using SRP;
using Xunit;

namespace Tests.SRP
{
	public class ActiveAndLocalCoursesSelectorTests
	{
		[Theory]
		[InlineData(Source.Local, CourseStatus.Active, true)]
		[InlineData(Source.Local, CourseStatus.Ended, false)]
		[InlineData(Source.External, CourseStatus.Active, false)]
		public void ShouldEndTests(Source addedBy, CourseStatus status, Boolean expectedShouldEnd) =>
			Assert.Equal(
				expectedShouldEnd,
				new ActiveAndLocalCoursesSelector()
					.ShouldEnd(
						new MedicationCourse(
							preparationTerm: String.Empty,
							status: status,
							issuances: Enumerable.Empty<Issuance>(),
							addedBy: addedBy)));
	}
}
