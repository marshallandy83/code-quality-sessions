using Xunit;

namespace OCP
{
	public class ViewModelTests
	{
		private readonly ViewModel _viewModel = new();

		[Fact]
		public void TestSetSelectedMedicationCourses_SingleActiveLocalCourseSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Active, Source = Source.Local }
				};

			Assert.True(_viewModel.IsEndCourseButtonEnabled);
			Assert.False(_viewModel.IsRestartCourseButtonEnabled);
			Assert.True(_viewModel.IsRequestIssueButtonEnabled);
			Assert.True(_viewModel.IsReprintButtonEnabled);
			Assert.True(_viewModel.IsEditButtonEnabled);
			Assert.True(_viewModel.IsIssueButtonEnabled);
			Assert.True(_viewModel.IsViewCourseHistoryButtonEnabled);
		}

		[Fact]
		public void TestSetSelectedMedicationCourses_MultipleActiveLocalCoursesSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Active, Source = Source.Local },
					new MedicationCourse { Status = Status.Active, Source = Source.Local }
				};

			Assert.True(_viewModel.IsEndCourseButtonEnabled);
			Assert.False(_viewModel.IsRestartCourseButtonEnabled);
			Assert.True(_viewModel.IsRequestIssueButtonEnabled);
			Assert.True(_viewModel.IsReprintButtonEnabled);
			Assert.False(_viewModel.IsEditButtonEnabled);
			Assert.False(_viewModel.IsIssueButtonEnabled);
			Assert.False(_viewModel.IsViewCourseHistoryButtonEnabled);
		}

		[Fact]
		public void TestSetSelectedMedicationCourses_SingleEndedLocalCourseSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Ended, Source = Source.Local }
				};

			Assert.False(_viewModel.IsEndCourseButtonEnabled);
			Assert.True(_viewModel.IsRestartCourseButtonEnabled);
			Assert.True(_viewModel.IsRequestIssueButtonEnabled);
			Assert.True(_viewModel.IsReprintButtonEnabled);
			Assert.False(_viewModel.IsEditButtonEnabled);
			Assert.False(_viewModel.IsIssueButtonEnabled);
			Assert.True(_viewModel.IsViewCourseHistoryButtonEnabled);
		}

		[Fact]
		public void TestSetSelectedMedicationCourses_MultipleEndedLocalCourseSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Ended, Source = Source.Local },
					new MedicationCourse { Status = Status.Ended, Source = Source.Local }
				};

			Assert.False(_viewModel.IsEndCourseButtonEnabled);
			Assert.True(_viewModel.IsRestartCourseButtonEnabled);
			Assert.True(_viewModel.IsRequestIssueButtonEnabled);
			Assert.True(_viewModel.IsReprintButtonEnabled);
			Assert.False(_viewModel.IsEditButtonEnabled);
			Assert.False(_viewModel.IsIssueButtonEnabled);
			Assert.False(_viewModel.IsViewCourseHistoryButtonEnabled);
		}

		[Fact]
		public void TestSetSelectedMedicationCourses_SingleActiveExternalCourseSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Active, Source = Source.External }
				};

			Assert.False(_viewModel.IsEndCourseButtonEnabled);
			Assert.False(_viewModel.IsRestartCourseButtonEnabled);
			Assert.False(_viewModel.IsRequestIssueButtonEnabled);
			Assert.False(_viewModel.IsReprintButtonEnabled);
			Assert.False(_viewModel.IsEditButtonEnabled);
			Assert.False(_viewModel.IsIssueButtonEnabled);
			Assert.True(_viewModel.IsViewCourseHistoryButtonEnabled);
		}

		[Fact]
		public void TestSetSelectedMedicationCourses_MultipleActiveExternalCourseSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Active, Source = Source.External },
					new MedicationCourse { Status = Status.Active, Source = Source.External }
				};

			Assert.False(_viewModel.IsEndCourseButtonEnabled);
			Assert.False(_viewModel.IsRestartCourseButtonEnabled);
			Assert.False(_viewModel.IsRequestIssueButtonEnabled);
			Assert.False(_viewModel.IsReprintButtonEnabled);
			Assert.False(_viewModel.IsEditButtonEnabled);
			Assert.False(_viewModel.IsIssueButtonEnabled);
			Assert.False(_viewModel.IsViewCourseHistoryButtonEnabled);
		}

		[Fact]
		public void TestSetSelectedMedicationCourses_SingleEndedExternalCourseSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Ended, Source = Source.External }
				};

			Assert.False(_viewModel.IsEndCourseButtonEnabled);
			Assert.False(_viewModel.IsRestartCourseButtonEnabled);
			Assert.False(_viewModel.IsRequestIssueButtonEnabled);
			Assert.False(_viewModel.IsReprintButtonEnabled);
			Assert.False(_viewModel.IsEditButtonEnabled);
			Assert.False(_viewModel.IsIssueButtonEnabled);
			Assert.True(_viewModel.IsViewCourseHistoryButtonEnabled);
		}

		[Fact]
		public void TestSetSelectedMedicationCourses_MultipleEndedExternalCourseSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Ended, Source = Source.External },
					new MedicationCourse { Status = Status.Ended, Source = Source.External }
				};

			Assert.False(_viewModel.IsEndCourseButtonEnabled);
			Assert.False(_viewModel.IsRestartCourseButtonEnabled);
			Assert.False(_viewModel.IsRequestIssueButtonEnabled);
			Assert.False(_viewModel.IsReprintButtonEnabled);
			Assert.False(_viewModel.IsEditButtonEnabled);
			Assert.False(_viewModel.IsIssueButtonEnabled);
			Assert.False(_viewModel.IsViewCourseHistoryButtonEnabled);
		}

		[Fact]
		public void TestSetSelectedMedicationCourses_MixedStatusLocalCoursesSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Active, Source = Source.Local },
					new MedicationCourse { Status = Status.Ended, Source = Source.Local }
				};

			Assert.False(_viewModel.IsEndCourseButtonEnabled);
			Assert.False(_viewModel.IsRestartCourseButtonEnabled);
			Assert.True(_viewModel.IsRequestIssueButtonEnabled);
			Assert.True(_viewModel.IsReprintButtonEnabled);
			Assert.False(_viewModel.IsEditButtonEnabled);
			Assert.False(_viewModel.IsIssueButtonEnabled);
			Assert.False(_viewModel.IsViewCourseHistoryButtonEnabled);
		}

		[Fact]
		public void TestSetSelectedMedicationCourses_MixedStatusExternalCoursesSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Active, Source = Source.External },
					new MedicationCourse { Status = Status.Ended, Source = Source.External }
				};

			Assert.False(_viewModel.IsEndCourseButtonEnabled);
			Assert.False(_viewModel.IsRestartCourseButtonEnabled);
			Assert.False(_viewModel.IsRequestIssueButtonEnabled);
			Assert.False(_viewModel.IsReprintButtonEnabled);
			Assert.False(_viewModel.IsEditButtonEnabled);
			Assert.False(_viewModel.IsIssueButtonEnabled);
			Assert.False(_viewModel.IsViewCourseHistoryButtonEnabled);
		}
	}
}
