using System;

namespace SRP.Courses.Selection
{
	public class ActiveCoursesSelector : ICourseSelector
	{
		public Boolean ShouldEnd(Course medicationCourse) => medicationCourse.Status == Status.Active;
	}
}
