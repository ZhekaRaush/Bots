using QuizBL;
using QuizBL.Controller;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace BotTelegram
{
    class Program
    {
        private static GameObject Game;
        private static TelegramBotClient bot;

        static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Console.Title = "\t~~~ ZhekaGameBot v-1.0.0 ~~~";
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
            Console.WriteLine("\t\t\t  ZhekaGameBot v-1.0.0");
            Console.WriteLine("\n\n");

            Game = new GameObject();
            bot = new TelegramBotClient(accessToken);
#pragma warning disable CS0618 // Тип или член устарел
            bot.OnMessage += BotOnMessage;
            bot.StartReceiving();
#pragma warning restore CS0618 // Тип или член устарел

            Game.SendMessage = (l, s) => bot.SendTextMessageAsync(l, s);
            Console.ReadLine();
            Game.Finish();
        }

#pragma warning disable CS8632 // Аннотацию для ссылочных типов, допускающих значения NULL, следует использовать в коде только в контексте аннотаций "#nullable".
#pragma warning disable CS0618 // Тип или член устарел
        private static void BotOnMessage(object? sender, MessageEventArgs e)
#pragma warning restore CS0618 // Тип или член устарел
#pragma warning restore CS8632 // Аннотацию для ссылочных типов, допускающих значения NULL, следует использовать в коде только в контексте аннотаций "#nullable".
        {
            var chatId = e.Message.Chat.Id;
            var message = e.Message.Text;
            var fromId = e.Message.From.Id;
            Game.OnMessage(message, chatId, fromId);

        }
    }
}
