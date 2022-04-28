using System;
using SRP;
using Xunit;

namespace Tests.SRP
{
	public class MedicationCourseEnderTests
	{
		[Theory]
		[InlineData(Source.Local, CourseStatus.Active, IssuanceStatus.Active, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Cancelled, "Prescribing error")]
		[InlineData(Source.Local, CourseStatus.Active, IssuanceStatus.Cancelled, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Cancelled, null)]
		[InlineData(Source.Local, CourseStatus.Ended, IssuanceStatus.Active, CourseStatus.Ended, null, IssuanceStatus.Active, null)]
		[InlineData(Source.External, CourseStatus.Active, IssuanceStatus.Active, CourseStatus.Active, null, IssuanceStatus.Active, null)]
		public void EndTests(
			Source courseAddedBy,
			CourseStatus courseStatus,
			IssuanceStatus issuanceStatus,
			CourseStatus expectedCourseStatus,
			String expectedReasonForEnding,
			IssuanceStatus expectedIssuanceStatus,
			String expectedReasonForCancelling)
		{
			var issuance = new Issuance(issuanceStatus);
			var course = new MedicationCourse("TestPreparation", courseStatus, new[] { issuance }, courseAddedBy);
			var reasonForEnding = "Prescribing error";

			new MedicationCourseEnder().End(course, reasonForEnding);

			Assert.Equal(expectedCourseStatus, course.Status);
			Assert.Equal(expectedReasonForEnding, course.ReasonForEnding);
			Assert.Equal(expectedIssuanceStatus, issuance.Status);
			Assert.Equal(expectedReasonForCancelling, issuance.ReasonForCancelling);

			// Assert error was logged?
		}
	}
}
