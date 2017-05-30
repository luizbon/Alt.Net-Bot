using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AltNetBot.Dialogs
{
    [Serializable]
    public class RootDialog: IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi there!");

            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text.ToLower().Contains("meetup"))
            {
                context.Call(new MeetupDialog(), Resume);
            }
            else
            {
                await context.PostAsync("Sorry, I can't understand");

                context.Wait(MessageReceivedAsync);
            }
        }

        private Task Resume(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }
    }
}