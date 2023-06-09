﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuizBL
{
    /// <summary>
    /// Викторина.
    /// </summary>
    public class Quiz
    {
        /// <summary>
        /// Список воросов.
        /// </summary>
        public List<QuestionItem> Questions { get; set; }

        /// <summary>
        /// Случайный порядок задания вопросов.
        /// </summary>
        private readonly Random random;
        private int count;

        /// <summary>
        /// Конструктор для викторины.
        /// </summary>
        /// <param name="path">Файл с вопросами.</param>
        public Quiz(string path = "data.txt")
        {
            var lines = File.ReadAllLines(path);
            Questions = lines
                .Select(line => line.Split('|'))
                .Select(line => new QuestionItem
                {
                    Question = line[0],
                    Answer = line[1]
                })
                .ToList();
            random = new Random();
            count = Questions.Count;
        }

        /// <summary>
        /// Метод для отправки следующего вопроса.
        /// </summary>
        /// <returns></returns>
        public QuestionItem NextQuestion()
        {
            if (count < 1)
            {
                count = Questions.Count;
            }

            var index = random.Next(count - 1);
            var question = Questions[index];

            Questions.RemoveAt(index);
            Questions.Add(question);
            count--;

            return question;
        }
    }
}
