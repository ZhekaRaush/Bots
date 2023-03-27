using QuizBL;
using QuizBL.Controller;
using System;
using VkBotFramework;
using VkBotFramework.Models;

namespace VkontakteBot
{
    class Program
    {
        private static GameObject Game;
        private static VkBot bot;
        private static readonly string GroupUrl = "https://vk.com/club218865666";

        static void Main(string[] args)
        {
            Console.Title = "~~~ VKQuizApp v-1.0.0 ~~~";
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Console.WriteLine("\n\n");
            Console.Write("\tНазвание бота: ");
            var name = Console.ReadLine();

            var tokenController = new TokenController(name);
            if (tokenController.IsNewToken)
            {
                Console.Write("\tТокен бота: ");
                var value = Console.ReadLine();

                tokenController.SetNewTokenData(value);
            }

            var accessToken = $"{tokenController.CurrentToken.Value}";

            Console.WriteLine("\n\n");
            Console.WriteLine("\t\tVKQuizApp v-1.0.0");
            Console.WriteLine("\n\n");

            Game = new GameObject();
            bot = new VkBot(accessToken, GroupUrl);
            bot.OnMessageReceived += BotMessageReceived;
            Game.SendMessage = (l, s) => bot.Api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams()
            {
                Message = s,
                PeerId = l,
                RandomId = Environment.TickCount
            });
            bot.Start();

            Console.ReadLine();
            Game.Finish();
        }

#pragma warning disable CS8632 // Аннотацию для ссылочных типов, допускающих значения NULL, следует использовать в коде только в контексте аннотаций "#nullable".
        private static void BotMessageReceived(object? sender, MessageReceivedEventArgs e)
#pragma warning restore CS8632 // Аннотацию для ссылочных типов, допускающих значения NULL, следует использовать в коде только в контексте аннотаций "#nullable".
        {
            var chatId = e.Message.PeerId.Value;
            var message = e.Message.Text;
            var fromId = e.Message.FromId.Value;
            Game.OnMessage(message, chatId, fromId);
        }
    }
}
