using System;
using System.Collections.Generic;
using SRP.Issuances;

namespace SRP.Courses
{
	public interface ICourse
	{
		String PreparationTerm { get; }
		Status Status { get; set; }
		IEnumerable<Issuance> Issuances { get; }
		String ReasonForEnding { get; set; }
		Source AddedBy { get; }
	}
}
