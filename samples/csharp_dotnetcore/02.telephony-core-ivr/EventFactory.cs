﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Schema;

namespace Microsoft.Bot.Builder
{
    /// <summary>
    /// Contains utility methods for creating various event types.
    /// </summary>
    public static class EventFactory
    {
        /// <summary>
        /// Create handoff initiation event.
        /// </summary>
        /// <param name="turnContext">turn context.</param>
        /// <param name="handoffContext">agent hub-specific context.</param>
        /// <param name="transcript">transcript of the conversation.</param>
        /// <returns>handoff event.</returns>
        public static IEventActivity CreateHandoffInitiation(ITurnContext turnContext, object handoffContext, Transcript transcript = null)
        {
            var handoffEvent = CreateHandoffEvent(HandoffEventNames.InitiateHandoff, handoffContext, turnContext.Activity.Conversation);

            var conversationReference = turnContext.Activity.GetConversationReference();

            handoffEvent.From = turnContext.Activity.From;
            handoffEvent.RelatesTo = turnContext.Activity.GetConversationReference();
            handoffEvent.ReplyToId = turnContext.Activity.Id;
            handoffEvent.ServiceUrl = turnContext.Activity.ServiceUrl;
            handoffEvent.ChannelId = turnContext.Activity.ChannelId;

            if (transcript != null)
            {
                var bufferedActivities = transcript.Activities.Select(a => a.ApplyConversationReference(conversationReference)).ToArray();
                var attchment = new Attachment
                {
                    Content = new Transcript(bufferedActivities),
                    ContentType = "application/json",
                    Name = "Transcript",
                };
                handoffEvent.Attachments.Add(attchment);
            }

            return handoffEvent;
        }

        /// <summary>
        /// Create handoff status event.
        /// </summary>
        /// <param name="conversation">Conversation being handed over.</param>
        /// <param name="state">State, possible values are: "accepted", "failed", "completed".</param>
        /// <param name="message">Additional message for failed handoff.</param>
        /// <returns>handoff event.</returns>
        public static IEventActivity CreateHandoffStatus(ConversationAccount conversation, string state, string message = null)
        {
            object value;

            if (string.IsNullOrEmpty(message))
            {
                value = new { state };
            }
            else
            {
                value = new { state, message };
            }

            var handoffEvent = CreateHandoffEvent(HandoffEventNames.HandoffStatus, value, conversation);
            return handoffEvent;
        }

        private static Activity CreateHandoffEvent(string name, object value, ConversationAccount conversation)
        {
            var handoffEvent = Activity.CreateEventActivity() as Activity;

            handoffEvent.Name = name;
            handoffEvent.Value = value;
            handoffEvent.Id = Guid.NewGuid().ToString();
            handoffEvent.Timestamp = DateTime.UtcNow;
            handoffEvent.Conversation = conversation;
            handoffEvent.Attachments = new List<Attachment>();
            handoffEvent.Entities = new List<Entity>();
            return handoffEvent;
        }
    }
}