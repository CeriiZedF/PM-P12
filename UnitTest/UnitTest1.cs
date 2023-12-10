using App;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace UnitTest            // Тестовий проєкт "відзеркалює"
{                             // основний проєкт, його класи називають
    [TestClass]               // від імен класів проєкту,
    public class HelperTest   // додаючи "Test"
    {
        [TestMethod]
        public void FinalizeTest()
        {
            Helper helper = new Helper();
            Assert.IsNotNull(helper, " ERROR -> New Helper() should not be Null");
            Assert.AreEqual(
                "Hello, World.",
                helper.Finalize("Hello, World"));
            Assert.AreEqual(
                "Hello.",
                helper.Finalize("Hello"));
            Assert.AreEqual(
                "Hello, World.",
                helper.Finalize("Hello, World."));
            Assert.AreEqual(
                "Hello.",
                helper.Finalize("Hello."));
            Assert.AreEqual(
                "Hello, World!",
                helper.Finalize("Hello, World!"));
            Assert.AreEqual(
                "Hello?",
                helper.Finalize("Hello?"));
            Assert.AreEqual(
                "Hello, World,",
                helper.Finalize("Hello, World,"));
            Assert.AreEqual(
                "Hello,",
                helper.Finalize("Hello,"));
            Assert.AreEqual(
                "Hello, World?",
                helper.Finalize("Hello, World?"));
            Assert.AreEqual(
                "Hello!",
                helper.Finalize("Hello!"));
            Assert.AreEqual(
                "My nytraus get hsaf.",
                helper.Finalize("My nytraus get hsaf"));
            Assert.AreEqual(
                "Aligh Html!",
                helper.Finalize("Aligh Html!"));
            Assert.AreEqual(
                "Nuclear Reactor?",
                helper.Finalize("Nuclear Reactor?"));
        }

        [TestMethod]
        public void EllipsisTest()  // так само, як і методи
        {
            Helper helper = new();
            Assert.IsNotNull(helper, "new Helper() should not be null");
            Assert.AreEqual(
                "He...",
                helper.Ellipsis("Hello, World", 5));
            Assert.AreEqual(
                "Hel...",
                helper.Ellipsis("Hello, World", 6));
            Assert.AreEqual(
                "Test...",
                helper.Ellipsis("Test String", 7));
        }

        [TestMethod]
        public void EllipsisExceptionTest()
        {
            /* Тестування виключень має ряд особливостей.
             * - поява виключення у коді тестового проєкту вважається
             *    провалом тесту. Відповідно, безпосередній виклик
             *    методів із внутрішніми виключеннями неправильний.
             *    ! Методи "обгортаються" у лямбди
             * - Перевірка типу виключень відбувається у "суворому"
             *    порівнянні. Тобто узагальнені типи "Exception" не 
             *    зараховуються, якщо реальне виключення іншого типу
             *    (навіть, якщо це тип - нащадок Exception)
             * - Саме виключення, що виникло у лямбді, повертається
             *    з Assert, що дозволяє додати перевірки (тести) на
             *    його вміст чи будову.
             */
            Helper helper = new();
            // тест: helper.Ellipsis(null!, 1) має "викинути" виключення типу ArgumentNullException
            var ex =
                Assert.ThrowsException<ArgumentNullException>(
                    () => helper.Ellipsis(null!, 1)
                );
            // тест: повідомлення виключення (ex.Message) повинно містити назву аргументу (input)
            Assert.IsTrue(
                ex.Message.Contains("input"),
                "Exception message should contain 'input' substring"
            );

            var ex2 = Assert.ThrowsException<ArgumentException>(
                () => helper.Ellipsis("Hello, world", 1)
            );
            Assert.IsTrue(ex2.Message.Contains("len"));

            var ex3 = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => helper.Ellipsis("Hello, world", 100)
            );
            Assert.IsTrue(ex3.Message.Contains("len"));
        }

        [TestMethod]
        public void CombineUrlTest()
        {
            Helper helper = new();
            Dictionary<String[], String> testCases = new()
            {
                { new[] { "/home",  "index"   }, "/home/index"  },
                { new[] { "/shop/", "/cart"   }, "/shop/cart"   },
                { new[] { "auth/",  "logout"  }, "/auth/logout" },
                { new[] { "forum/",  "topic/" }, "/forum/topic" },
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(
                    testCase.Value,
                    helper.CombineUrl(testCase.Key[0], testCase.Key[1]),
                    $"{testCase.Value} -- {testCase.Key[0]} + {testCase.Key[1]}"
                );
            }
        }

        [TestMethod]
        public void CombineUrlMultiTest()
        {
            Helper helper = new();
            Dictionary<String[], String> testCases = new()
            {
                { new[] { "/home",  "/index", "/123"  }, "/home/index/123"  },
                { new[] { "/shop/", "/cart/", "123/"  }, "/shop/cart/123"   },
                { new[] { "auth/",  "logout", "/123/" }, "/auth/logout/123" },
                { new[] { "forum",  "topic/", "123"   }, "/forum/topic/123" },
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(
                    testCase.Value,
                    helper.CombineUrl(testCase.Key),
                    $"{testCase.Value} -- {testCase.Key[0]} + {testCase.Key[1]}"
                );
            }
        }

        [TestMethod]
        public void CombineUrlExceptionTest()
        {
            // /home     null     1) /home  2) Exception ??
            // Не треба виключення, у такому разі логічно ігнорувати null
            Helper helper = new();
            // користуємось тією особливістю, що поява виключення 
            // сама по собі провалить тест, додаткових Assert-ів не потрібно
            Assert.AreEqual("/home", helper.CombineUrl("/home", null!));

            // null!, null! -- Exception, шлях у нікуди не може валідним
            var ex = Assert.ThrowsException<ArgumentException>(
                () => helper.CombineUrl(null!, null!)
            );
            Assert.AreEqual("All arguments are null", ex.Message);

            // null  /subsection  -- наявність підкатегорії без категорії - Exception
            Assert.AreEqual(
                "Non-Null argument after Null one",
                Assert.ThrowsException<ArgumentException>(
                    () => helper.CombineUrl(null!, "/subsection")
                ).Message);
        }
    }
}
/* Д.З. Додати тестових умов і, за необхідності, модифікувати метод 
 * CombineUrl для проходження різних тестів
 * - всі елементи null з різною кількістю аргументів
 * - частина null, частина не null у різних комбінаціях
 * Критерій - якщо null у завершальній частині - це норма,
 * якщо після null іде не-null, то це виключення
 *  
 *  ***
 *  Якщо один з параметрів "..", то вилучається попередній фрагмент
 *  /shop/cart/user/../123 --> /shop/cart/123
 */
/* Т.З. Створити метод CombineUrl(part1, part2)
 * part1     part2      ret
 * /home     index      /home/index
 * /home/   /index      /home/index
 *  home/    index      /home/index
 *  
 * Розширити цей метод для довільної кількості параметрів 
 */
/* Д.З. Створити метод .Finalize(String) який буде додавати
 * точку до кінця рядка, якщо її там немає. Якщо є, то не додає
 * Скласти для нього тестовий метод з достатньою кількістю 
 * тверджень.
 * Додати скриншот з результатом тестування.
 */