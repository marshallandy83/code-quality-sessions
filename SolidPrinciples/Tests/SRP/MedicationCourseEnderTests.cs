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
		[InlineData(CourseStatus.Active, CourseStatus.Ended, "Prescribing error", false, true)]
		[InlineData(CourseStatus.Ended, CourseStatus.Ended, null, true, false)]
		public void EndTests(
			CourseStatus status,
			CourseStatus expectedStatus,
			String expectedReasonForEnding,
			Boolean expectedToLogError,
			Boolean expectedToCancelIssuances)
		{
			var issuances = Enumerable.Empty<Issuance>();
			var course = new MedicationCourse("TestPreparation", status, issuances);
			var reasonForEnding = "Prescribing error";
			var mockLogger = new Mock<ILogger>();
			var mockIssuanceCanceller = new Mock<IMedicationIssuanceCanceller>();

			new MedicationCourseEnder(mockLogger.Object, mockIssuanceCanceller.Object).End(course, reasonForEnding);

			Assert.Equal(expectedStatus, course.Status);
			Assert.Equal(expectedReasonForEnding, course.ReasonForEnding);

			mockLogger.Verify(mock => mock.Log(It.Is<String>(match => match == "TestPreparation course cannot be ended.")), expectedToLogError ? Times.Once : Times.Never);

			mockIssuanceCanceller.Verify(mock =>
				mock.Cancel(
					It.Is<IEnumerable<Issuance>>(match => match == issuances),
					It.Is<String>(match => match == reasonForEnding)),
				expectedToCancelIssuances ? Times.Once : Times.Never);
		}
	}
}
