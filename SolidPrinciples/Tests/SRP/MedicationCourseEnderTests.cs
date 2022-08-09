using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SRP;
using Xunit;

namespace Tests.SRP
{
	public class MedicationCourseEnderTests
	{
		[Theory]
		[InlineData(true, CourseStatus.Ended, "Prescribing error", false, true)]
		[InlineData(false, CourseStatus.Active, null, true, false)]
		public void EndTests(
			Boolean shouldEnd,
			CourseStatus expectedStatus,
			String expectedReasonForEnding,
			Boolean expectedToLogError,
			Boolean expectedToCancelIssuances)
		{
			var issuances = Enumerable.Empty<Issuance>();
			var course = new MedicationCourse("TestPreparation", CourseStatus.Active, issuances);
			var reasonForEnding = "Prescribing error";
			var mockLogger = new Mock<ILogger>();
			var mockIssuanceCanceller = new Mock<IMedicationIssuanceCanceller>();
			var mockSelector = new Mock<IMedicationCourseSelector>();
			mockSelector.Setup(mock => mock.ShouldEnd(It.IsAny<MedicationCourse>())).Returns(shouldEnd);

			new MedicationCourseEnder(mockLogger.Object, mockIssuanceCanceller.Object, mockSelector.Object).End(course, reasonForEnding);

			Assert.Equal(expectedStatus, course.Status);
			Assert.Equal(expectedReasonForEnding, course.ReasonForEnding);

			mockSelector.Verify(mock => mock.ShouldEnd(It.Is<MedicationCourse>(match => match == course)));

			mockLogger.Verify(mock => mock.Log(It.Is<String>(match => match == "TestPreparation course cannot be ended.")), expectedToLogError ? Times.Once : Times.Never);

			mockIssuanceCanceller.Verify(mock =>
				mock.Cancel(
					It.Is<IEnumerable<Issuance>>(match => match == issuances),
					It.Is<String>(match => match == reasonForEnding)),
				expectedToCancelIssuances ? Times.Once : Times.Never);
		}
	}
}
