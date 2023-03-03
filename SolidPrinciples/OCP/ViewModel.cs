using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace OCP
{
	public class ViewModel : INotifyPropertyChanged
	{
		public ViewModel() => MedicationCourses = TestDataFactory.Create();

		public void UpdateActionsFromSelectedMedicationCourses()
		{
			IEnumerable<MedicationCourse> selectedMedicationCourses = _selectedMedicationCourses.Cast<MedicationCourse>();

			Boolean isSingleMedicationCourseSelected = selectedMedicationCourses.Count() == 1;

			if (selectedMedicationCourses.All(course => course.Status == Status.Active))
			{
				UpdateActionsFromActiveMedicationCourses(isSingleMedicationCourseSelected);
			}
			else if (selectedMedicationCourses.All(course => course.Status == Status.Ended))
			{
				UpdateActionsFromEndedMedicationCourses(isSingleMedicationCourseSelected);
			}
			else
			{
				UpdateActionsFromMixedMedicationCourses();
			}

			IsRequestIssueButtonEnabled = true;
			IsReprintButtonEnabled = true;
		}

		private void UpdateActionsFromActiveMedicationCourses(Boolean isSingleMedicationCourseSelected)
		{
			IsEndCourseButtonEnabled = true;
			IsRestartCourseButtonEnabled = false;
			IsEditButtonEnabled = isSingleMedicationCourseSelected;
			IsIssueButtonEnabled = isSingleMedicationCourseSelected;
			IsViewCourseHistoryButtonEnabled = isSingleMedicationCourseSelected;
		}

		private void UpdateActionsFromEndedMedicationCourses(Boolean isSingleMedicationCourseSelected)
		{
			IsEndCourseButtonEnabled = false;
			IsRestartCourseButtonEnabled = true;
			IsEditButtonEnabled = false;
			IsIssueButtonEnabled = false;
			IsViewCourseHistoryButtonEnabled = isSingleMedicationCourseSelected;
		}

		private void UpdateActionsFromMixedMedicationCourses()
		{
			IsEndCourseButtonEnabled = false;
			IsRestartCourseButtonEnabled = false;
			IsEditButtonEnabled = false;
			IsIssueButtonEnabled = false;
			IsViewCourseHistoryButtonEnabled = false;
		}

		private Boolean _isEndCourseButtonEnabled;
		public Boolean IsEndCourseButtonEnabled
		{
			get => _isEndCourseButtonEnabled;
			set
			{
				_isEndCourseButtonEnabled = value;
				OnPropertyChanged(nameof(IsEndCourseButtonEnabled));
			}
		}

		private Boolean _isRestartCourseButtonEnabled;
		public Boolean IsRestartCourseButtonEnabled
		{
			get => _isRestartCourseButtonEnabled;
			set
			{
				_isRestartCourseButtonEnabled = value;
				OnPropertyChanged(nameof(IsRestartCourseButtonEnabled));
			}
		}

		private Boolean _isRequestIssueButtonEnabled;
		public Boolean IsRequestIssueButtonEnabled
		{
			get => _isRequestIssueButtonEnabled;
			set
			{
				_isRequestIssueButtonEnabled = value;
				OnPropertyChanged(nameof(IsRequestIssueButtonEnabled));
			}
		}

		private Boolean _isReprintButtonEnabled;
		public Boolean IsReprintButtonEnabled
		{
			get => _isReprintButtonEnabled;
			set
			{
				_isReprintButtonEnabled = value;
				OnPropertyChanged(nameof(IsReprintButtonEnabled));
			}
		}

		private Boolean _isEditButtonEnabled;
		public Boolean IsEditButtonEnabled
		{
			get => _isEditButtonEnabled;
			set
			{
				_isEditButtonEnabled = value;
				OnPropertyChanged(nameof(IsEditButtonEnabled));
			}
		}

		private Boolean _isIssueButtonEnabled;
		public Boolean IsIssueButtonEnabled
		{
			get => _isIssueButtonEnabled;
			set
			{
				_isIssueButtonEnabled = value;
				OnPropertyChanged(nameof(IsIssueButtonEnabled));
			}
		}

		private Boolean _isViewCourseHistoryButtonEnabled;
		public Boolean IsViewCourseHistoryButtonEnabled
		{
			get => _isViewCourseHistoryButtonEnabled;
			set
			{
				_isViewCourseHistoryButtonEnabled = value;
				OnPropertyChanged(nameof(IsViewCourseHistoryButtonEnabled));
			}
		}

		public ObservableCollection<MedicationCourse> MedicationCourses { get; }

		private IList _selectedMedicationCourses = new List<MedicationCourse>();

		public IList SelectedMedicationCourses
		{
			get => _selectedMedicationCourses;
			set
			{
				if (value == null)
					return;

				_selectedMedicationCourses = value;
				OnPropertyChanged(nameof(SelectedMedicationCourses));

				UpdateActionsFromSelectedMedicationCourses();
			}
		}

		public void OnPropertyChanged(string name)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
