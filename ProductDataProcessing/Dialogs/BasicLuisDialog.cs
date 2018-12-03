using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using ProductDataProcessing.Cards;
using ProductDataProcessing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
namespace ProductDataProcessing.Dialogs
{
	[LuisModel("3a3b29ac-e173-4f4b-a9aa-942d32294e8e", "bcf1b71423d94825bec1e9df48d35ca4", LuisApiVersion.V2, domain: "westeurope.api.cognitive.microsoft.com")]
	[Serializable]
	public class BasicLuisDialog : LuisDialog<object>
	{


		[LuisIntent("fillInForm")]
		[LuisIntent("removeReceipt")]
		[LuisIntent("updateReceipt")]
		[LuisIntent("None")]
		public async Task NoneIntent(IDialogContext context, LuisResult result)
		{
			await context.PostAsync("Sorry , I couldn't understand you , can you try again?");
			context.Wait(MessageReceived);
		}
		[LuisIntent("")]
		public async Task ReturnedResult(IDialogContext context,IAwaitable<Object> obj ,  LuisResult result)
		{
			UserModel user = new UserModel();
			context.UserData.TryGetValue<UserModel>("SaveUserInfo", out user);
			var activity = await obj as Activity;
			dynamic dynamicResult = Newtonsoft.Json.JsonConvert.DeserializeObject(activity.Value.ToString());
			if ((string)dynamicResult.result != null)
			{
				if((string)dynamicResult.result == "change")
				{
					await context.PostAsync("Please type in the new cost centre.");
					user.nextState = "costCentre";
					context.UserData.SetValue<UserModel>("SaveUserInfo", user);
					// we finished from LuisDialog == > now call the RootDialog
					context.Call(new RootDialog(), ResumeAfterDialog);
				}
				else
				{
					await context.PostAsync("Ok Thanks.");
				}
			}

		}

		[LuisIntent("askForName")]
		public async Task AskForNameIntent(IDialogContext context, LuisResult result)
		{
			UserModel user = new UserModel();
			// get user name entity from luis
			EntityRecommendation UserNameEntity = result.Entities.Where(item => item.Type == "builtin.personName").FirstOrDefault();
			// we already have enity
			if (UserNameEntity != null)
			{
				user.firstName = UserNameEntity.Entity;
				context.UserData.SetValue<UserModel>("SaveUserInfo", user);
				await context.PostAsync($"Alright, {user.firstName} . What can I help you with?");
				context.Wait(MessageReceived);
			}
			// no entity name
			else
			{
				await context.PostAsync($"PLease , Can you type again I didn't understand what you mean .");
				context.Wait(MessageReceived);

			}

		}

		[LuisIntent("businessExpenses")]
		public async Task BusinessExpensesIntent(IDialogContext context, IAwaitable<object> obj, LuisResult result)
		{
			UserModel user = new UserModel();
			context.UserData.TryGetValue<UserModel>("SaveUserInfo", out user);
			if (user == null)
			{
				await context.PostAsync($"Please,can you enter your First name before ");

			}
			else
			{
				await context.PostAsync($"I know it‘s annoying but please fill in the form about your travel expenses");
				var card = CostCenterCard.GetAdaptiveCard();
				await this.ShowResult(context, card);
			}

		}
		private async Task ResumeAfterDialog(IDialogContext context, IAwaitable<object> result)
		{
			context.Done<object>(null);
		}

		private async Task ShowResult(IDialogContext context, AdaptiveCard card)
		{
			Attachment attachment = new Attachment()
			{
				ContentType = AdaptiveCard.ContentType,
				Content = card
			};
			IMessageActivity messageActivity = context.MakeMessage();
			messageActivity.Attachments.Add(attachment);
			await context.PostAsync(messageActivity);
			context.Wait(MessageReceived);
		}



	}
}