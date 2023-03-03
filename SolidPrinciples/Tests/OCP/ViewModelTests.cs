using Xunit;

namespace OCP
{
	public class ViewModelTests
	{
		private readonly ViewModel _viewModel = new();

		[Fact]
		public void TestSetSelectedMedicationCourses_SingleActiveCourseSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Active }
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
		public void TestSetSelectedMedicationCourses_MultipleActiveCoursesSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Active },
					new MedicationCourse { Status = Status.Active }
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
		public void TestSetSelectedMedicationCourses_SingleEndedCourseSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Ended }
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
		public void TestSetSelectedMedicationCourses_MultipleEndedCourseSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Ended },
					new MedicationCourse { Status = Status.Ended }
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
		public void TestSetSelectedMedicationCourses_MixedStatusCoursesSelected()
		{
			_viewModel.SelectedMedicationCourses =
				new[]
				{
					new MedicationCourse { Status = Status.Active },
					new MedicationCourse { Status = Status.Ended }
				};

			Assert.False(_viewModel.IsEndCourseButtonEnabled);
			Assert.False(_viewModel.IsRestartCourseButtonEnabled);
			Assert.True(_viewModel.IsRequestIssueButtonEnabled);
			Assert.True(_viewModel.IsReprintButtonEnabled);
			Assert.False(_viewModel.IsEditButtonEnabled);
			Assert.False(_viewModel.IsIssueButtonEnabled);
			Assert.False(_viewModel.IsViewCourseHistoryButtonEnabled);
		}
	}
}
