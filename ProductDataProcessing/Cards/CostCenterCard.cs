using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductDataProcessing.Cards
{
	public class CostCenterCard
	{
		public static AdaptiveCard GetAdaptiveCard()
		{
			AdaptiveCard card = new AdaptiveCard();
			card.Body.Add(new TextBlock()
			{
				Text = "Your default cost centre is 010101. Is this correct or would you like to change it?",
				Size = TextSize.Large,
				Wrap = true,
				Weight = TextWeight.Bolder
			});
			card.Actions.Add(new SubmitAction()
			{
				Title = "correct",
				DataJson = Newtonsoft.Json.JsonConvert.SerializeObject(new {result = "change" })
			});
			card.Actions.Add(new SubmitAction()
			{
				Title = "change",
				DataJson = Newtonsoft.Json.JsonConvert.SerializeObject(new { result = "change" })
			});

			return card;
		}
	}
}