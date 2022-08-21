using System;

namespace SRP.Issuances
{
	public class Issuance
	{
		public Issuance(Status status, Source addedBy)
		{
			Status = status;
			AddedBy = addedBy;
		}

		public Status Status { get; set; }
		public String ReasonForCancelling { get; set; }
		public Source AddedBy { get; }
	}
}
