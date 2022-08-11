using System;

namespace SRP
{
	internal class Program
	{
		// An example of how the MedicationCourseEnder could be instantiated for different customers
		static void Main(String[] args)
		{
			Int32 customerNumber = Int32.Parse(args[0]);

			var courseEnder = new MedicationCourseEnder(
				new ConsoleLogger(),
				new MedicationIssuanceCanceller(
					customerNumber == 1 ? (IMedicationIssuanceSelector)new ActiveAndLocalIssuancesSelector() : new ActiveIssuancesSelector()),
				customerNumber == 1 ? (IMedicationCourseSelector)new ActiveAndLocalCoursesSelector() : new ActiveCoursesSelector());
		}
	}
}
