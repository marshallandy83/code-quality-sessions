using System;
using SRP.Courses.Selection;
using SRP.Issuances.Cancelling;
using SRP.Logging;

namespace SRP.Courses.Ending
{
	public class CourseEnder
	{
		private readonly ILogger _logger;
		private readonly IIssuanceCanceller _issuanceCanceller;
		private readonly ICourseSelector _selector;

		public CourseEnder(ILogger logger, IIssuanceCanceller issuanceCanceller, ICourseSelector selector)
		{
			_logger = logger;
			_issuanceCanceller = issuanceCanceller;
			_selector = selector;
		}

		public void End(ICourse medicationCourse, String reasonForEnding)
		{
			if (_selector.ShouldEnd(medicationCourse))
			{
				medicationCourse.Status = Status.Ended;
				medicationCourse.ReasonForEnding = reasonForEnding;
				_issuanceCanceller.Cancel(medicationCourse.Issuances, reasonForEnding);
			}
			else
			{
				_logger.Log($"{medicationCourse.PreparationTerm} course cannot be ended.");
			}
		}
	}
}
