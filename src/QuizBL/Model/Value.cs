using System;

namespace QuizBL.Model
{
    /// <summary>
    /// Токен.
    /// </summary>
    
    [Serializable]
    public class Value
    {
        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Создать новый токен.
        /// </summary>
        /// <param name="name">Название бота.</param>
        public Value(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"\"{ nameof(name) }\" не может быть пустым.", nameof(name));
            }

            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
