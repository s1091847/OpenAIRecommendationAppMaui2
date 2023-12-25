using OpenAIRecommendationAppMaui.Services;
using System;
using System.Collections.Generic;
namespace OpenAIRecommendationAppMaui2
{
    public partial class MainPage : ContentPage
    {
        OpenAIService openAIService;
        string character;
        bool nextQuestionClicked = false;

        public MainPage(OpenAIService svc)
        {
            openAIService = svc;
            InitializeComponent();
        }

        private async void OnGetAnswerClicked(object sender, EventArgs e)
        {
            // 清除上一個 GPT 回答的內容
            AnswerLabel.Text = "";

            string userQuestion = QuestionEntry.Text;

            if (!nextQuestionClicked)
            {
                
                List<string> characters = new List<string>
                {
                    "魯夫","多啦A夢","漩渦鳴人","海綿寶寶","派大星"


                };

               
                Random random = new Random();
                int index = random.Next(characters.Count);
                character = characters[index];

                nextQuestionClicked = true;
            }

            
            string modifiedQuestion = "在故事中" + character + "是" + userQuestion + "嗎？ 你只能用是或否來回答我";

          
            string response = await openAIService.CallOpenAIChat(modifiedQuestion);

            AnswerLabel.Text = response;

           
            if (userQuestion == character)
            {
                await DisplayAlert("恭喜！", "答對了！", "確定");
            }
        }

        private void OnDisplayCharacterClicked(object sender, EventArgs e)
        {
            
            DisplayCharacterLabel.Text = "答案：" + character;
        }

        private void OnNextQuestionClicked(object sender, EventArgs e)
        {
            
            nextQuestionClicked = false;
            character = null;
            DisplayCharacterLabel.Text = "答案："; 
            AnswerLabel.Text = ""; 
        }
    }
}


