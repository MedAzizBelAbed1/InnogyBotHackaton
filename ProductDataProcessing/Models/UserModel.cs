using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductDataProcessing.Models
{
	public class UserModel
	{
		public string firstName { get; set; }
		public string costCentre { get; set; }
		public string location { get; set; }
		public string date  { get; set; }
		public string timeFrom { get; set; }
		public string timeTo { get; set; }
		public string amount { get; set; }
		public string details { get; set; }
		public string linkOfReceipt { get; set; }
		public string nextState { get; set; }



	}
}