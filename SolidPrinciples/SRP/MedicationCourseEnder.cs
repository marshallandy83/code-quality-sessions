using System;

namespace SRP
{
	public class MedicationCourseEnder
	{
		private readonly ILogger _logger;
		private readonly IMedicationIssuanceCanceller _issuanceCanceller;

		public MedicationCourseEnder(ILogger logger, IMedicationIssuanceCanceller issuanceCanceller)
		{
			_logger = logger;
			_issuanceCanceller = issuanceCanceller;
		}

		public void End(MedicationCourse medicationCourse, String reasonForEnding)
		{
			if (medicationCourse.Status == CourseStatus.Active)
			{
				medicationCourse.Status = CourseStatus.Ended;
				medicationCourse.ReasonForEnding = reasonForEnding;
				_issuanceCanceller.Cancel(medicationCourse.Issuances, reasonForEnding);
			}
			else
			{
				_logger.Log($"{medicationCourse.PreparationTerm} course cannot be ended.");
				return;
			}
		}
	}
}
