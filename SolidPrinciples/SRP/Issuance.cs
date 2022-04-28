using System;

namespace SRP
{
	public class Issuance
	{
		public Issuance(IssuanceStatus status) => Status = status;

		public IssuanceStatus Status { get; set; }
		public String ReasonForCancelling { get; set; }
	}
}
