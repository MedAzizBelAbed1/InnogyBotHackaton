using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductDataProcessing.Cards
{
	public class AdditionalCard
	{
		public static AdaptiveCard GetAdaptiveCard()
		{
			AdaptiveCard card = new AdaptiveCard();
			card.Body.Add(new TextBlock()
			{
				Text = "Do you have any other additional expenses ?",
				Size = TextSize.Large,
				Wrap = true,
				Weight = TextWeight.Bolder
			});
			card.Actions.Add(new SubmitAction()
			{
				Title = "No",
				Data = "Transport"
			});
			card.Actions.Add(new SubmitAction()
			{
				Title = "Transport",
				Data = "Transport"
			});
			card.Actions.Add(new SubmitAction()
			{
				Title = "Accommodation",
				Data = "Transport"
			});
			card.Actions.Add(new SubmitAction()
			{
				Title = "Transport",
				Data = "Transport"
			});


			return card;
		}
	}
}