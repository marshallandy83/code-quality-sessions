using System;

namespace SRP.Courses.Selection
{
	public class ActiveCoursesSelector : ICourseSelector
	{
		public Boolean ShouldEnd(ICourse medicationCourse) => medicationCourse.Status == Status.Active;
	}
}
