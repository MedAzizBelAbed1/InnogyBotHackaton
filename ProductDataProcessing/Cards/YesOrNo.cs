using AdaptiveCards;
using ProductDataProcessing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductDataProcessing.Cards
{
	public class YesOrNo
	{
		public static AdaptiveCard GetAdaptiveCard(UserModel user)
		{
			AdaptiveCard card = new AdaptiveCard();
			card.Body.Add(new TextBlock()
			{
				Text = $"First Name: {user.firstName}",
				Size = TextSize.Large,
				Wrap = true,
				Weight = TextWeight.Bolder

			});
			card.Body.Add(new TextBlock()
			{
				Text = $"Cost Centre: {user.costCentre}",
				Size = TextSize.Normal,
				Wrap = true,
				Weight = TextWeight.Normal

			});
			card.Body.Add(new TextBlock()
			{
				Text = $"Details  {user.details}",
				Size = TextSize.Normal,
				Wrap = true,
				Weight = TextWeight.Normal

			});
			card.Body.Add(new TextBlock()
			{
				Text = $"Date:  {user.date}",
				Size = TextSize.Normal,
				Wrap = true,
				Weight = TextWeight.Normal

			});
			card.Body.Add(new TextBlock()
			{
				Text = $"timeFrom:  { user.timeFrom}",
				Size = TextSize.Normal,
				Wrap = true,
				Weight = TextWeight.Normal

			});
			card.Body.Add(new TextBlock()
			{
				Text = $"timeTo: { user.timeTo}",
				Size = TextSize.Normal,
				Wrap = true,
				Weight = TextWeight.Normal

			});
			card.Body.Add(new TextBlock()
			{
				Text = "Is this correct? ",
				Size = TextSize.Normal,
				Wrap = true,
				Weight = TextWeight.Normal
			});
			card.Actions.Add(new SubmitAction()
			{
				Title = "Yes",
				Data = "Yes"
			});
			card.Actions.Add(new SubmitAction()
			{
				Title = "No",
				Data = "Yes"
			});
			return card;
		}
	}
}