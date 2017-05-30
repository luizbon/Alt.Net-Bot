using System;
using System.Threading.Tasks;
using AltNetBot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace AltNetBot.Dialogs
{
    [LuisModel("", "")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        private const string EntityGeographyCity = "builtin.geography.city";
        private const string EntityGeographyCountry = "builtin.geography.country";
        private const string EntityCategory = "category";

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hi! Try asking me things like 'search meet-ups in Sydney', 'search meet-ups in Australia'");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("SearchMeetups")]
        public async Task Search(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            await context.PostAsync($"Welcome to the Meet-ups finder! We are analyzing your message: '{message.Text}'...");

            if (FindCityOrCountry(result, out EntityRecommendation placeEntityRecommendation))
            {
                result.TryFindEntity(EntityCategory, out EntityRecommendation categoryEntityRecommendation);
                try
                {
                    await MeetupHelper.Search(context, placeEntityRecommendation.Entity, categoryEntityRecommendation?.Entity);
                }
                catch (Exception e)
                {
                    await context.PostAsync(e.Message);
                }
            }
            else
            {
                await context.PostAsync("Sorry, I couldn't find any meet-up");
            }
        }

        private static bool FindCityOrCountry(LuisResult result, out EntityRecommendation entityRecommendation)
        {
            return result.TryFindEntity(EntityGeographyCity, out entityRecommendation) || result.TryFindEntity(EntityGeographyCountry, out entityRecommendation);
        }
    }
}