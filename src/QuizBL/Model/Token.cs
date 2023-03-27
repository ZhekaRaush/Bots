using System;

namespace QuizBL.Model
{
    /// <summary>
    /// Токен.
    /// </summary>

    [Serializable]
    public class Token
    {
        /// <summary>
        /// Наименование бота.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Токен.
        /// </summary>
        public Value Value { get; set; }

        /// <summary>
        /// Токен бота.
        /// </summary>
        /// <param name="name">Имя бота</param>
        /// <param name="value">токен бота</param>
        public Token(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException($"\"{nameof(name)}\" не может быть пустым", nameof(name));
            }

            Name = name;
        }

        public override string ToString()
        {
            return Name + "\n" + Value;
        }
    }
}
