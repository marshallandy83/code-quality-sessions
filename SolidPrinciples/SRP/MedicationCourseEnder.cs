using System;
using System.Linq;

namespace SRP
{
	public class MedicationCourseEnder
	{
		public void End(MedicationCourse medicationCourse, String reasonForEnding, Boolean allowExternalEnding)
		{
			if (medicationCourse.Status == CourseStatus.Active && (medicationCourse.AddedBy == Source.Local || allowExternalEnding))
			{
				medicationCourse.Status = CourseStatus.Ended;
				medicationCourse.ReasonForEnding = reasonForEnding;

				foreach (var issuance in medicationCourse.Issuances.Where(i => i.Status == IssuanceStatus.Active && i.AddedBy == Source.Local))
				{
					issuance.Status = IssuanceStatus.Cancelled;
					issuance.ReasonForCancelling = reasonForEnding;
				}
			}
			else
			{
				Console.WriteLine($"{medicationCourse.PreparationTerm} course cannot be ended.");
				return;
			}
		}
	}
}
