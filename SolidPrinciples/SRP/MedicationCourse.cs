using System;
using System.Collections.Generic;

namespace SRP
{
	public class MedicationCourse
	{
		public MedicationCourse(
			String preparationTerm,
			CourseStatus status,
			IEnumerable<Issuance> issuances)
		{
			PreparationTerm = preparationTerm;
			Status = status;
			Issuances = issuances;
		}

		public String PreparationTerm { get; }
		public CourseStatus Status { get; set; }
		public IEnumerable<Issuance> Issuances { get; }
		public String ReasonForEnding { get; set; }
	}
}
