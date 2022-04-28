using System;

namespace SRP
{
	public class ActiveAndLocalCoursesSelector : IMedicationCourseSelector
	{
		public Boolean ShouldEnd(MedicationCourse medicationCourse) =>
			medicationCourse.Status == CourseStatus.Active && medicationCourse.AddedBy == Source.Local;
	}
}
