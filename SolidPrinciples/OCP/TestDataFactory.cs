using System;
using System.Collections.ObjectModel;

namespace OCP
{
	internal static class TestDataFactory
	{
		public static ObservableCollection<MedicationCourse> Create() =>
			new ObservableCollection<MedicationCourse>(
				new[]
				{
					new MedicationCourse
					{
						PreparationName = "Aspirin 75mg tablets",
						Status = Status.Active
					},
					new MedicationCourse
					{
						PreparationName = "Paracetamol 500mg capsules",
						Status = Status.Active
					},
					new MedicationCourse
					{
						PreparationName = "Strepsils",
						Status = Status.Active
					},
					new MedicationCourse
					{
						PreparationName = "Fluoxetine 20mg capsules",
						Status = Status.Active
					},
					new MedicationCourse
					{
						PreparationName = "Labetalol 100mg tablets",
						Status = Status.Ended
					},
					new MedicationCourse
					{
						PreparationName = "Doxazosin 10mg tablets",
						Status = Status.Ended
					}
				});
	}
}
