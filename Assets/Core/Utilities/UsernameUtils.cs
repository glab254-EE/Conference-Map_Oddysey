using System;
using System.Text;
using System.Text.RegularExpressions;

public static class UsernameUtils
{
    private static readonly string[] Cyrillic =
    {
        "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й",
        "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф",
        "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я"
    };

    private static readonly string[] Latin =
    {
        "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y",
        "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f",
        "h", "ts", "ch", "sh", "sch", "", "y", "", "e", "yu", "ya"
    };

    public static string MakeUsername(string input, int maxLength = 16)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "";

        input = input.Trim().ToLowerInvariant();

        // Кириллица -> латиница
        for (int i = 0; i < Cyrillic.Length; i++)
        {
            input = input.Replace(Cyrillic[i], Latin[i]);
        }

        // Разрешаем только a-z, 0-9 и _
        StringBuilder result = new StringBuilder();

        foreach (char c in input)
        {
            if ((c >= 'a' && c <= 'z') ||
                (c >= '0' && c <= '9') ||
                c == '_')
            {
                result.Append(c);
            }
        }

        // Убираем несколько "_" подряд
        string username = Regex.Replace(result.ToString(), "_+", "_");

        // "_" в начале и конце
        username = username.Trim('_');

        // Ограничение длины
        if (username.Length > maxLength)
            username = username.Substring(0, maxLength);

        return username;
    }
}