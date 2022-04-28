using System;
using Moq;
using SRP;
using Xunit;

namespace Tests.SRP
{
	public class MedicationCourseEnderTests
	{
		[Theory]
		[InlineData(CourseStatus.Active, IssuanceStatus.Active, CourseStatus.Ended, "Prescribing error", false, IssuanceStatus.Cancelled, "Prescribing error")]
		[InlineData(CourseStatus.Active, IssuanceStatus.Cancelled, CourseStatus.Ended, "Prescribing error", false, IssuanceStatus.Cancelled, null)]
		[InlineData(CourseStatus.Ended, IssuanceStatus.Active, CourseStatus.Ended, null, true, IssuanceStatus.Active, null)]
		public void EndTests(
			CourseStatus courseStatus,
			IssuanceStatus issuanceStatus,
			CourseStatus expectedCourseStatus,
			String expectedReasonForEnding,
			Boolean expectedToLogError,
			IssuanceStatus expectedIssuanceStatus,
			String expectedReasonForCancelling)
		{
			var issuance = new Issuance(issuanceStatus);
			var course = new MedicationCourse("TestPreparation", courseStatus, new[] { issuance });
			var reasonForEnding = "Prescribing error";
			var mockLogger = new Mock<ILogger>();

			new MedicationCourseEnder(mockLogger.Object).End(course, reasonForEnding);

			Assert.Equal(expectedCourseStatus, course.Status);
			Assert.Equal(expectedReasonForEnding, course.ReasonForEnding);
			Assert.Equal(expectedIssuanceStatus, issuance.Status);
			Assert.Equal(expectedReasonForCancelling, issuance.ReasonForCancelling);

			mockLogger.Verify(mock => mock.Log(It.Is<String>(match => match == "TestPreparation course cannot be ended.")), expectedToLogError ? Times.Once : Times.Never);
		}
	}
}
