﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Intented to use for Learning & Demo purposes only

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace Microsoft.BotBuilderSamples
{
    // This bot will respond to the user's input with an Adaptive Card.
    // Adaptive Cards are a way for developers to exchange card content
    // in a common and consistent way. A simple open card format enables
    // an ecosystem of shared tooling, seamless integration between apps,
    // and native cross-platform performance on any device.
    // For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    // This is a Transient lifetime service. Transient lifetime services are created
    // each time they're requested. For each Activity received, a new instance of this
    // class is created. Objects that are expensive to construct, or have a lifetime
    // beyond the single turn, should be carefully managed.

    public class AdaptiveCardsBot : ActivityHandler 
    {
        private const string WelcomeText = @"This bot will introduce you to AdaptiveCards.
                                            Type anything to see an AdaptiveCard.";

        // This array contains the file location of our adaptive cards
        private readonly string[] _cards =
        {
            Path.Combine(".", "Resources", "FlightItineraryCard.json"),
            Path.Combine(".", "Resources", "ImageGalleryCard.json"),
            Path.Combine(".", "Resources", "LargeWeatherCard.json"),
            Path.Combine(".", "Resources", "FlightUpdate.json"),
            Path.Combine(".", "Resources", "AdaptiveCardDemo.json"),
            Path.Combine(".", "Resources", "FoodOrder.json"),
            Path.Combine(".", "Resources", "InputForm.json"),
            Path.Combine(".", "Resources", "RestaurantCard.json"),
            Path.Combine(".", "Resources", "SolitaireCard.json"),
            Path.Combine(".", "Resources", "InputChoiceCard.json"),
        };

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await SendWelcomeMessageAsync(turnContext, cancellationToken);
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            Random r = new Random();
            var activity = turnContext.Activity;
            var cardAttachment = CreateAdaptiveCardAttachment(_cards[r.Next(_cards.Length)]);

            if(activity.Text.Contains("weather", StringComparison.InvariantCultureIgnoreCase))
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(CreateAdaptiveCardAttachment(_cards[2])));
                await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
            }
            else if(activity.Text.Contains("flight", StringComparison.InvariantCultureIgnoreCase))
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(CreateAdaptiveCardAttachment(_cards[3])));
                await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
            }
            else if(activity.Text.Contains("demo", StringComparison.InvariantCultureIgnoreCase))
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(CreateAdaptiveCardAttachment(_cards[4])));
                await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
            }
            else if(activity.Text.Contains("Form", StringComparison.InvariantCultureIgnoreCase))
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(CreateAdaptiveCardAttachment(_cards[6])));
                await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
            }
            else if(activity.Text.Contains("Food", StringComparison.InvariantCultureIgnoreCase))
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(CreateAdaptiveCardAttachment(_cards[5])));
                await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
            }
            else
            {
                //turnContext.Activity.Attachments = new List<Attachment>() { cardAttachment };
                await turnContext.SendActivityAsync(MessageFactory.Text("Displaying a random card"), cancellationToken);
                await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
                await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
            }
        }

        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(
                        $"Welcome to Adaptive Cards Bot {member.Name}. {WelcomeText}",
                        cancellationToken: cancellationToken);
                }
            }
        }
   
        private static Attachment CreateAdaptiveCardAttachment(string filePath)
        {
            var adaptiveCardJson = File.ReadAllText(filePath);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }
    }
}