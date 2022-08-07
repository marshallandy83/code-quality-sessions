using System;
using SRP;
using Xunit;

namespace Tests.SRP
{
	public class MedicationCourseEnderTests
	{
		[Theory]
		[InlineData(Source.Local, CourseStatus.Active, Source.Local, IssuanceStatus.Active, false, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Cancelled, "Prescribing error")]
		[InlineData(Source.Local, CourseStatus.Active, Source.Local, IssuanceStatus.Cancelled, false, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Cancelled, null)]
		[InlineData(Source.Local, CourseStatus.Ended, Source.Local, IssuanceStatus.Active, false, CourseStatus.Ended, null, IssuanceStatus.Active, null)]
		[InlineData(Source.External, CourseStatus.Active, Source.Local, IssuanceStatus.Active, false, CourseStatus.Active, null, IssuanceStatus.Active, null)]
		[InlineData(Source.Local, CourseStatus.Active, Source.External , IssuanceStatus.Active, false, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Active, null)]
		[InlineData(Source.External, CourseStatus.Active, Source.Local, IssuanceStatus.Active, true, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Cancelled, "Prescribing error")]
		[InlineData(Source.Local, CourseStatus.Active, Source.External, IssuanceStatus.Active, true, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Cancelled, "Prescribing error")]
		public void EndTests(
			Source courseAddedBy,
			CourseStatus courseStatus,
			Source issuanceAddedBy,
			IssuanceStatus issuanceStatus,
			Boolean allowExternalEnding,
			CourseStatus expectedCourseStatus,
			String expectedReasonForEnding,
			IssuanceStatus expectedIssuanceStatus,
			String expectedReasonForCancelling)
		{
			var issuance = new Issuance(issuanceStatus, issuanceAddedBy);
			var course = new MedicationCourse("TestPreparation", courseStatus, new[] { issuance }, courseAddedBy);
			var reasonForEnding = "Prescribing error";

			new MedicationCourseEnder().End(course, reasonForEnding, allowExternalEnding);

			Assert.Equal(expectedCourseStatus, course.Status);
			Assert.Equal(expectedReasonForEnding, course.ReasonForEnding);
			Assert.Equal(expectedIssuanceStatus, issuance.Status);
			Assert.Equal(expectedReasonForCancelling, issuance.ReasonForCancelling);

			// Assert error was logged?
		}
	}
}
