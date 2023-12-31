﻿using Azure.AI.OpenAI;
using Azure;

namespace OpenAIRecommendationAppMaui.Services
{
    public class OpenAIService
    {
        OpenAIClient client;
        static readonly char[] trimChars = new char[] { '\n', '?' };

        public void Initialize(string openAIKey, string? openAIEndpoint = null)
        {
            client = !string.IsNullOrWhiteSpace(openAIEndpoint)
                ? new OpenAIClient(
                    new Uri(openAIEndpoint),
                    new AzureKeyCredential(openAIKey))
                : new OpenAIClient(openAIKey);
        }

        internal async Task<string> CallOpenAI(string userInput)
        {
            string prompt = GeneratePrompt(userInput);
            Response<Completions> response = await client.GetCompletionsAsync(
                "text-davinci-003", // assumes a matching model deployment or model name
                userInput);
            StringWriter sw = new StringWriter();
            foreach (Choice choice in response.Value.Choices)
            {
                var text = choice.Text.TrimStart(trimChars);
                sw.WriteLine(text);
            }
            var message = sw.ToString();
            return message;
        }


        internal async Task<string> CallOpenAIChat(string userInput)
        {
            string prompt = GeneratePrompt(userInput);
            ChatCompletionsOptions options = new ChatCompletionsOptions();
            options.ChoiceCount = 1;
            options.Messages.Add(new ChatMessage(ChatRole.User, prompt));
            var response = await client.GetChatCompletionsAsync(
                "gpt-3.5-turbo-16k", 
                options);
            StringWriter sw = new StringWriter();
            foreach (ChatChoice choice in response.Value.Choices)
            {
                var text = choice.Message.Content.TrimStart(trimChars);
                sw.WriteLine(text);
            }
            var message = sw.ToString();
            return message;
        }

        private static string GeneratePrompt(string userInput)
        {
            return userInput;
        }

    }
}