using System;

namespace SRP
{
	public interface IMedicationCourseSelector
	{
		Boolean ShouldEnd(MedicationCourse medicationCourse);
	}
}
