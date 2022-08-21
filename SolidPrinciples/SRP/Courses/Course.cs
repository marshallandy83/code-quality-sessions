using System;
using System.Collections.Generic;
using SRP.Issuances;

namespace SRP.Courses
{
	public class Course
	{
		public Course(
			String preparationTerm,
			Status status,
			IEnumerable<Issuance> issuances,
			Source addedBy)
		{
			PreparationTerm = preparationTerm;
			Status = status;
			Issuances = issuances;
			AddedBy = addedBy;
		}

		public String PreparationTerm { get; }
		public Status Status { get; set; }
		public IEnumerable<Issuance> Issuances { get; }
		public String ReasonForEnding { get; set; }
		public Source AddedBy { get; }
	}
}
