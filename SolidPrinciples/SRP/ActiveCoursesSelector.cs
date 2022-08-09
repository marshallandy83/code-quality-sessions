using System;

namespace SRP
{
	public class ActiveCoursesSelector : IMedicationCourseSelector
	{
		public Boolean ShouldEnd(MedicationCourse medicationCourse) => medicationCourse.Status == CourseStatus.Active;
	}
}
