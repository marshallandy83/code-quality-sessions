using System;

namespace SRP.Courses.Selection
{
	public interface ICourseSelector
	{
		Boolean ShouldEnd(Course medicationCourse);
	}
}
