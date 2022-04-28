using System;

namespace SRP
{
	public class Issuance
	{
		public Issuance(IssuanceStatus status, Source addedBy)
		{
			Status = status;
			AddedBy = addedBy;
		}

		public IssuanceStatus Status { get; set; }
		public String ReasonForCancelling { get; set; }
		public Source AddedBy { get; }
	}
}
