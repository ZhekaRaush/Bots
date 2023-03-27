using QuizBL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuizBL.Controller
{
    /// <summary>
    /// Контроллер для токена.
    /// </summary>

    public class TokenController
    {
        /// <summary>
        /// Токен приложения.
        /// </summary>
        public List<Token> Tokens { get; }

        public Token CurrentToken { get; }

        public bool IsNewToken { get; } = false;

        public TokenController(string tokenName)
        {
            if (string.IsNullOrWhiteSpace(tokenName))
            {
                throw new ArgumentNullException($"\"{nameof(tokenName)}\" не может быть пустым", nameof(tokenName));
            }

            Tokens = GetTokenData();

            CurrentToken = Tokens.SingleOrDefault(u => u.Name == tokenName);

            if (CurrentToken == null)
            {
                CurrentToken = new Token(tokenName);
                Tokens.Add(CurrentToken);
                IsNewToken = true;
                Save();
            }
        }

        private List<Token> GetTokenData()
        {
            var formatter = new BinaryFormatter();

#pragma warning disable IDE0063 // Использовать простой оператор using
            using (var fs = new FileStream("tokens.dat", FileMode.OpenOrCreate))
#pragma warning restore IDE0063 // Использовать простой оператор using
            {
                if (fs.Length > 0 && formatter.Deserialize(fs) is List<Token> tokens)
                {
                    return tokens;
                }
                else
                {
                    return new List<Token>();
                }
            }
        }

        public void SetNewTokenData(string valueName)
        {
            // TODO: Проверка.

            CurrentToken.Value = new Value(valueName);
            Save();
        }

        /// <summary>
        /// Сохранить данные токена.
        /// </summary>
        public void Save()
        {
            var formatter = new BinaryFormatter();

#pragma warning disable IDE0063 // Использовать простой оператор using
            using (var fs = new FileStream("tokens.dat", FileMode.OpenOrCreate))
#pragma warning restore IDE0063 // Использовать простой оператор using
            {
                formatter.Serialize(fs, Tokens);
            }
        }
    }
}
