using System;
using System.Linq;

namespace SRP
{
	public class MedicationCourseEnder
	{
		private readonly ILogger _logger;

		public MedicationCourseEnder(ILogger logger) => _logger = logger;

		public void End(MedicationCourse medicationCourse, String reasonForEnding)
		{
			if (medicationCourse.Status == CourseStatus.Active)
			{
				medicationCourse.Status = CourseStatus.Ended;
				medicationCourse.ReasonForEnding = reasonForEnding;

				foreach (var issuance in medicationCourse.Issuances.Where(i => i.Status == IssuanceStatus.Active))
				{
					issuance.Status = IssuanceStatus.Cancelled;
					issuance.ReasonForCancelling = reasonForEnding;
				}
			}
			else
			{
				_logger.Log($"{medicationCourse.PreparationTerm} course cannot be ended.");
				return;
			}
		}
	}
}
