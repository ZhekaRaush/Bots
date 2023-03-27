using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuizBL
{
    public class GameObject
    {
        /// <summary>
        /// Викторина.
        /// </summary>
        private readonly Quiz quiz;

        /// <summary>
        /// Статистика.
        /// </summary>
        private static Dictionary<long, QuestionState> States;
        private static Dictionary<long, int> UserScores;
        private static readonly string StateFileName = "state.json";
        private static readonly string ScoreFileName = "score.json";

        public GameObject()
        {
            quiz = new Quiz("data.txt");

            // Сохраняем в файл послендний не разгаданный вопрос с сохранением неугаданных и открытых букв.
            if (File.Exists(StateFileName))
            {
                var json = File.ReadAllText(StateFileName);
                States = JsonConvert.DeserializeObject<Dictionary<long, QuestionState>>(json);
            }
            else
            {
                States = new Dictionary<long, QuestionState>();
            }

            // Сохраняем в файл количество очков.
            if (File.Exists(ScoreFileName))
            {
                var json = File.ReadAllText(ScoreFileName);
                UserScores = JsonConvert.DeserializeObject<Dictionary<long, int>>(json);
            }
            else
            {
                UserScores = new Dictionary<long, int>();
            }
        }

        public void Finish()
        {
            var stateJson = JsonConvert.SerializeObject(States);
            File.WriteAllText(StateFileName, stateJson);

            var scoreJson = JsonConvert.SerializeObject(UserScores);
            File.WriteAllText(ScoreFileName, scoreJson);
        }

        /// <summary>
        /// Задаем новый вопрос.
        /// </summary>
        /// <param name="chatId">Идентификационный номер чата.</param>
        public void NewRound(long chatId)
        {
            if (!States.TryGetValue(chatId, out var state))
            {
                state = new QuestionState();
                States[chatId] = state;
            }

            state.CurentItem = quiz.NextQuestion();
            state.Opened = 0;
            SendMessage(chatId, state.DysplayQuestion);
        }

        public Action<long, string> SendMessage;

        public void OnMessage(string message, long chatId, long fromId)
        {
            if (message == null)
            {
                return;
            }

            if (message == "/start")
            {
                NewRound(chatId);
            }
            else
            {
                if (!States.TryGetValue(chatId, out var state))
                {
                    state = new QuestionState();
                    States[chatId] = state;
                }

                if (state.CurentItem == null)
                {
                    state.CurentItem = quiz.NextQuestion();
                }

                var question = state.CurentItem;
                var tryAnswer = message.ToLower().Replace('ё', 'е');
                if (tryAnswer == question.Answer)
                {

                    if (UserScores.ContainsKey(fromId))
                    {
                        UserScores[fromId]++;
                    }
                    else
                    {
                        UserScores[fromId] = 1;
                    }
                    NewRound(chatId);
                    SendMessage(
                        chatId,
                        $"Правильно!\nУ вас {UserScores[fromId]} очков"
                    );
                }
                else
                {
                    state.Opened++;
                    if (state.IsEnd)
                    {
                        SendMessage(
                            chatId,
                            $"Не отгадал! Это - {question.Answer}"
                        );
                        Console.WriteLine(
                            $"Не отгадал! Это - {question.Answer}"
                        );
                        NewRound(chatId);
                    }

                    SendMessage(
                        chatId,
                        state.DysplayQuestion
                    );
                    Console.WriteLine("Не правильно!");
                }
            }
        }
    }
}
