namespace QuizBL
{
    /// <summary>
    /// Статус вопроса.
    /// </summary>
    public class QuestionState
    {
        public QuestionItem CurentItem { get; set; }

        /// <summary>
        /// Счетчик открытых букв.
        /// </summary>
        public int Opened { get; set; }

        /// <summary>
        /// Вывод количество закрытых букв звездочками.
        /// </summary>
        public string AnswerHint => CurentItem.Answer
            .Substring(0, Opened)
            .PadRight(CurentItem.Answer.Length, '*');

        /// <summary>
        /// Вывод количества откпытых букв.
        /// </summary>
        public string DysplayQuestion => $"{CurentItem.Question}\n{CurentItem.Answer.Length} букв        {AnswerHint}";

        /// <summary>
        /// Проверка не все ли буквы открвты.
        /// </summary>
        public bool IsEnd => Opened == CurentItem.Answer.Length;
    }
}
