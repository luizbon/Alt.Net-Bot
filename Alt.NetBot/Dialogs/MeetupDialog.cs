using System;
using System.Threading.Tasks;
using AltNetBot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AltNetBot.Dialogs
{
    [Serializable]
    public class MeetupDialog: IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Where you want to find meet-ups?");

            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            try
            {
                await MeetupHelper.Search(context, message.Text);
            }
            catch (Exception e)
            {
                await context.PostAsync(e.Message);
            }

            context.Done<object>(null);
        }
    }
}