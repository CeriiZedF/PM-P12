using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Helper
    {
        public String CombineUrl(params String[] parts)
        {
            StringBuilder sb = new();
            bool wasNull = false;
            foreach (String part in parts)
            {
                if (part is null)
                {
                    wasNull = true;
                    continue;
                }
                if (wasNull)
                {
                    throw new ArgumentException("Non-Null argument after Null one");
                }
                String p = part;
                if (!p.StartsWith('/')) p = '/' + p;
                if (p.EndsWith("/")) p = p[..^1];
                sb.Append(p);
            }
            if(sb.Length == 0)
            {
                throw new ArgumentException("All arguments are null");
            }
            return sb.ToString();
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
