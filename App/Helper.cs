using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App
{
    public class Helper
    {
        public bool ContainsAttributes(String html)
        {
            string pattern = @"<(\w+\s+[^=>])*(\w+=[^>]+)+>";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(html);
        }

        // заменяет активные HTML символы на сущность
        public string EscapeHtml(string html, Dictionary<string, string>? map = null)
        {
            if (html is null) { throw new ArgumentException("Argument 'html' is null"); }
            if (html.Length == 0) { return html; }

            Dictionary<string, string> htmlSpecSymbols = map ?? new()  // если map null, то создаётся новый словарь
            {
                { "&", "&amp;" },
                { "<", "&lt;" },
                { ">", "&gt;" }
            };
            foreach (var pair in htmlSpecSymbols)
            {
                html = html.Replace(pair.Key, pair.Value);
            }
            return html;
        }

        // объединяет элементы массива в адресную строку
        public string CombineUrl(params string[] parts)
        {
            if (parts is null) { throw new NullReferenceException("Parts is null"); }
            if (parts.Length == 0) { throw new ArgumentException("Parts is empty"); }

            StringBuilder result = new();  // будет много работы с добавлением строк, поэтому используем StringBuilder
            string temp;
            bool wasNull = false;  // для проверки, что если после null идёт строка, то это ошибка
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] is null)
                {
                    wasNull = true;  // устанавливаем флаг
                    continue;
                }
                if (wasNull)
                {
                    throw new ArgumentException("Non-Null argument after Null one");
                }

                if (parts[i] == "..") { continue; }  // игнорируем строку
                temp = "/" + parts[i].TrimStart('/').TrimEnd('/');  // удаляем все '/' и добавляем один в начало

                if ((i != parts.Length - 1) && parts[i + 1] == "..") { continue; }  // если строка не последняя в массиве и следующая строка это '..'
                result.Append(temp);
            }
            if (result.Length == 0)
            {
                throw new ArgumentException("All arguments are null");
            }
            return result.ToString();
        }

        public String Ellipsis(String input, int len)
        {
            if(input == null)
            {
                throw new ArgumentNullException("Null detected in parameter: " + nameof(input));
            }
            if(len < 3)
            {
                throw new ArgumentException("Argument 'len' could not be less than 3");
            }
            if(input.Length < len)
            {
                throw new ArgumentOutOfRangeException("Argument 'len' could not be greater than input length");
            }
            // return "He...";
            // return (len == 5) ? "He..." : "Hel...";
            // return "Hel"[..(len-3)]+"...";
            return input[..(len - 3)] + "...";
        }

        /*Д.З.Створити метод.Finalize(String) який буде додавати
        точку до кінця рядка, якщо її там немає.Якщо є, то не додає
        Скласти для нього тестовий метод з достатньою кількістю
        тверджень.
        Додати скриншот з результатом тестування.*/
        
        public String Finalize(String str)
        {
            Char[] symbols = { '.', ',', '?', '!' };
            return symbols.Contains(str.Last()) ? str : str + '.';
        }
    }
}
/* Розробити метод String EscapeHtml(String html)
 * який замінює активні HTML символи на сутності
 * '<' -> &lt;
 * '>' -> &gt;
 * '&' -> &amp;
 * 1. Скласти декілька тестових тверджень
 * 
 */
