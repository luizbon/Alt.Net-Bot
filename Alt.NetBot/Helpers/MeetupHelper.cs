using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltNetBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AltNetBot.Helpers
{
    public class MeetupHelper
    {
        public static async Task Search(IDialogContext context, string location, string category = null)
        {
            var service = new MeetupService();

            var categoryId = await GetCategoryId(category);

            var groups = await service.FindGroups(location, categoryId);

            await context.PostAsync($"I found {groups.Count} meet-ups:");

            var resultMessage = context.MakeMessage();
            resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            resultMessage.Attachments = new List<Attachment>();

            foreach (var group in groups)
            {
                var heroCard = new HeroCard
                {
                    Title = group.Name,
                    Subtitle = $"{group.Rating} starts. With {group.Members} {group.Who}",
                    Images = new List<CardImage>
                    {
                        new CardImage { Url = group.GroupPhoto?.PhotoLink }
                    },
                    Buttons = new List<CardAction>
                    {
                        new CardAction
                        {
                            Title = "More details",
                            Type = ActionTypes.OpenUrl,
                            Value = group.Link
                        }
                    }
                };

                resultMessage.Attachments.Add(heroCard.ToAttachment());
            }

            await context.PostAsync(resultMessage);
        }

        private static async Task<int?> GetCategoryId(string category)
        {
            if (string.IsNullOrEmpty(category))
                return null;

            var service = new MeetupService();

            var categories = await service.ListCategories();

            return categories.SingleOrDefault(
                x => category.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase) ||
                     category.Equals(x.Shortname, StringComparison.CurrentCultureIgnoreCase))?.Id;
        }
    }
}