using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SRP.Courses.Selection;
using SRP.Issuances;
using SRP.Issuances.Cancelling;
using SRP.Logging;
using Xunit;

namespace SRP.Courses.Ending
{
	public class CourseEnderTests
	{
		[Theory]
		[InlineData(true, Status.Ended, "Prescribing error", false, true)]
		[InlineData(false, Status.Active, null, true, false)]
		public void EndTests(
			Boolean shouldEnd,
			Status expectedStatus,
			String expectedReasonForEnding,
			Boolean expectedToLogError,
			Boolean expectedToCancelIssuances)
		{
			var issuances = Enumerable.Empty<Issuance>();
			var course = new Course("TestPreparation", Status.Active, issuances, Source.Local);
			var reasonForEnding = "Prescribing error";
			var mockLogger = new Mock<ILogger>();
			var mockIssuanceCanceller = new Mock<IIssuanceCanceller>();
			var mockSelector = new Mock<ICourseSelector>();
			mockSelector.Setup(mock => mock.ShouldEnd(It.IsAny<Course>())).Returns(shouldEnd);

			new CourseEnder(mockLogger.Object, mockIssuanceCanceller.Object, mockSelector.Object).End(course, reasonForEnding);

			Assert.Equal(expectedStatus, course.Status);
			Assert.Equal(expectedReasonForEnding, course.ReasonForEnding);

			mockSelector.Verify(mock => mock.ShouldEnd(It.Is<Course>(match => match == course)));

			mockLogger.Verify(mock => mock.Log(It.Is<String>(match => match == "TestPreparation course cannot be ended.")), expectedToLogError ? Times.Once : Times.Never);

			mockIssuanceCanceller.Verify(mock =>
				mock.Cancel(
					It.Is<IEnumerable<Issuance>>(match => match == issuances),
					It.Is<String>(match => match == reasonForEnding)),
				expectedToCancelIssuances ? Times.Once : Times.Never);
		}
	}
}
