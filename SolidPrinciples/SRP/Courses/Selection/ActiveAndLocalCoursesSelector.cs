using System;

namespace SRP.Courses.Selection
{
	public class ActiveAndLocalCoursesSelector : ICourseSelector
	{
		public Boolean ShouldEnd(Course medicationCourse) =>
			medicationCourse.Status == Status.Active && medicationCourse.AddedBy == Source.Local;
	}
}
