using System;
using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ProductDataProcessing.Cards;
using ProductDataProcessing.Models;
using ProductDataProcessing.Services;

namespace ProductDataProcessing.Dialogs
{
	[Serializable]
	public class RootDialog : IDialog<object>
	{
		public Task StartAsync(IDialogContext context)
		{
			context.Wait(MessageReceivedAsync);
			return Task.CompletedTask;
		}

		private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
		{
			var activity = await result as Activity;
			// small delay show that bot is typing
			await Task.Delay(1000);

			// upload the recipe
			if (activity.Attachments.Count > 0)
			{
				await context.PostAsync($"Great. I see you stayed in Duck House from xx til xxx. And you had breakfast included. ");
				await context.PostAsync($"On what dates did you start and end your trip? (dd / mm / yyyy – dd / mm / yyyy) ");
			}
			else
			{
				UserModel user = new UserModel();
				context.UserData.TryGetValue<UserModel>("SaveUserInfo", out user);
				switch(user.nextState)
				{
					case "costCentre":
						user.costCentre = activity.Text;
						user.nextState = "details";
						await context.PostAsync("Give a name to your trip");
						break;
					case "details":
						user.details = activity.Text;
						user.nextState = "location";
						await context.PostAsync($"Ok. Thanks, {user.firstName}.");
						await context.PostAsync($"Where did you travel to?");
						break;
					case "location":
						user.location = activity.Text;
						user.nextState = "date";
						await context.PostAsync($"Why on earth would you go there?! ;) Just kidding.");
						await context.PostAsync($"Please upload your reservation confirmation.");
						break;
					case "date":
						user.date = activity.Text;
						user.nextState = "timeFrom";
						await context.PostAsync($"What time did you start your trip? (00:00 24h)");
						break;
					case "timeFrom":
						user.timeFrom = activity.Text;
						user.nextState = "timeTo";
						await context.PostAsync($"And what time did you arrive back? (00:00 24h)");
						break;
					case "timeTo":
						user.timeTo = activity.Text;
						user.nextState = "Transport";
						var card = AdditionalCard.GetAdaptiveCard();
						Attachment attachment = new Attachment()
						{
							ContentType = AdaptiveCard.ContentType,
							Content = card
						};
						IMessageActivity messageActivity = context.MakeMessage();
						messageActivity.Attachments.Add(attachment);
						await context.PostAsync(messageActivity);
						break;
					case "Transport":
						user.nextState = "Yes";
						await context.PostAsync("Well done. You‘re almost there.Can you please check if the information displayed is correct?");
						var yesCard = YesOrNo.GetAdaptiveCard(user);
						Attachment yesAttachment = new Attachment()
						{
							ContentType = AdaptiveCard.ContentType,
							Content = yesCard
						};
						IMessageActivity yesMessageActivity = context.MakeMessage();
						yesMessageActivity.Attachments.Add(yesAttachment);
						await context.PostAsync(yesMessageActivity);
						break;
					case "Yes":
						user.nextState = "email";
						await context.PostAsync("Perfect!You‘re my hero, you made my life so much easier!");
						await context.PostAsync("Please can you type your email I will send all informations via Email!");
						break;
					case "email":
						user.nextState = "end";
						string data = user.firstName +
						user.costCentre + "   " +
						user.details + "   " +
						user.location + "   " +
						user.date + "   " +
						user.timeFrom + "   " +
						user.timeTo + "   ";
						Email.SendMail(user.firstName, data, activity.Text);
						await context.PostAsync($"Email was sent :)");
						break;
					default:
						await context.PostAsync("Sorry , I couldn't understand you , can you try again?");
						break;
				}
				context.UserData.SetValue<UserModel>("SaveUserInfo", user);
			}

			context.Wait(MessageReceivedAsync);
		}
	}

}