using System;
using SRP.Courses;
using SRP.Courses.Ending;
using SRP.Courses.Selection;
using SRP.Issuances.Cancelling;
using SRP.Issuances.Selection;
using SRP.Logging;

namespace SRP
{
	internal class Program
	{
		// An example of how the MedicationCourseEnder could be instantiated for different customers
		static void Main(String[] args)
		{
			Int32 customerNumber = Int32.Parse(args[0]);

			var courseEnder = new CourseEnder(
				new ConsoleLogger(),
				new IssuanceCanceller(
					customerNumber == 1 ? (IIssuanceSelector)new ActiveAndLocalIssuancesSelector() : new ActiveIssuancesSelector()),
				customerNumber == 1 ? (ICourseSelector)new ActiveAndLocalCoursesSelector() : new ActiveCoursesSelector());
		}
	}
}
