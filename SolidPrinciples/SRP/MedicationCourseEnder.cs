using System;

namespace SRP
{
	public class MedicationCourseEnder
	{
		private readonly ILogger _logger;
		private readonly IMedicationIssuanceCanceller _issuanceCanceller;
		private readonly IMedicationCourseSelector _selector;

		public MedicationCourseEnder(ILogger logger, IMedicationIssuanceCanceller issuanceCanceller, IMedicationCourseSelector selector)
		{
			_logger = logger;
			_issuanceCanceller = issuanceCanceller;
			_selector = selector;
		}

		public void End(MedicationCourse medicationCourse, String reasonForEnding)
		{
			if (_selector.ShouldEnd(medicationCourse))
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
