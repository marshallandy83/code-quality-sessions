using System;

namespace SRP.Courses.Selection
{
	public interface ICourseSelector
	{
		Boolean ShouldEnd(ICourse medicationCourse);
	}
}
