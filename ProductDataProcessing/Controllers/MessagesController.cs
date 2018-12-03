using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ProductDataProcessing
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
		{
			if (activity.Type == ActivityTypes.Message)
			{
				//Bot typing Indicator 
				ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
				var replyTyping = activity.CreateReply(String.Empty);
				replyTyping.Type = ActivityTypes.Typing;
				await connector.Conversations.ReplyToActivityAsync(replyTyping);

				await Conversation.SendAsync(activity, () => new Dialogs.BasicLuisDialog());
			}
			else
			{
				ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
				var reply = HandleSystemMessage(activity);
				IConversationUpdateActivity update = activity;
				if (update.MembersAdded.Count != 0)
				{
					foreach (var newMember in update.MembersAdded)
					{
						if (newMember.Id == activity.Recipient.Id)
						{
							await connector.Conversations.ReplyToActivityAsync(activity.CreateReply("Hello, I‘m HaRry, your HR expert. What‘s your name?"));
						}
					}
				}
			}
			var response = Request.CreateResponse(HttpStatusCode.OK);
			return response;
		}

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
				// Handle add/remove from contact lists
				// Activity.From + Activity.Action represent what happened
				return message.CreateReply("Hello");

			}
			else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}