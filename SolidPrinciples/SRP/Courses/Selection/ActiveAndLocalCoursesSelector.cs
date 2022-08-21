using System;

namespace SRP.Courses.Selection
{
	public class ActiveAndLocalCoursesSelector : ICourseSelector
	{
		public Boolean ShouldEnd(ICourse medicationCourse) =>
			medicationCourse.Status == Status.Active && medicationCourse.AddedBy == Source.Local;
	}
}
