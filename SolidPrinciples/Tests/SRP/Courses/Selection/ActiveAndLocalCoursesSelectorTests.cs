using System;
using Moq;
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
						Mock.Of<ICourse>(c => c.AddedBy == addedBy && c.Status == status)));
	}
}
