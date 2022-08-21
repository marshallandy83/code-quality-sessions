using System;
using Moq;
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
						Mock.Of<ICourse>(c => c.Status == status)));
	}
}
