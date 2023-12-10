using App;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace UnitTest            // �������� ����� "����������"
{                             // �������� �����, ���� ����� ���������
    [TestClass]               // �� ���� ����� ������,
    public class HelperTest   // ������� "Test"
    {
        [TestMethod]
        public void ContainsAttributesTest()
        {
            Helper helper = new();
            Assert.IsNotNull(helper, "new Helper() should not be null");  

            Assert.IsTrue(helper.ContainsAttributes("<div style=\"\"></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<i style=\"code\" ></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<i style=\"code\"� required ></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<i style='code'� required></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<i required style=\"code\" ></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<i required style=\"code\"></div>"));
            Assert.IsTrue(helper.ContainsAttributes("<img onload=\"dangerCode()\" src=\"puc.png\"/>"));
            Assert.IsTrue(helper.ContainsAttributes("<img width=100 />"));
            Assert.IsTrue(helper.ContainsAttributes("<img width=100/>"));
            Assert.IsTrue(helper.ContainsAttributes("<img width=100>"));
            Assert.IsTrue(helper.ContainsAttributes("<img width=500 required/>"));
            Assert.IsTrue(helper.ContainsAttributes("<img      width=500    required   />"));

            Assert.IsFalse(helper.ContainsAttributes("<div></div>"));
            Assert.IsFalse(helper.ContainsAttributes("<div ></div>"));
            Assert.IsFalse(helper.ContainsAttributes("<br/>"));
            Assert.IsFalse(helper.ContainsAttributes("<br />"));
            Assert.IsFalse(helper.ContainsAttributes("<div required ></div>"));
            Assert.IsFalse(helper.ContainsAttributes("<div required>x=5</div>"));
            Assert.IsFalse(helper.ContainsAttributes("<div required checked></div>"));
            Assert.IsFalse(helper.ContainsAttributes("<div>2=2</div>"));
        }

        [TestMethod]
        public void EscapeHtmlTest()
        {
            Helper helper = new();

            Assert.IsNotNull(helper, "new Helper() should not be null");
            Assert.IsNotNull(helper.EscapeHtml(">"), "EscapeHtml should not return null!");
            Assert.IsNotNull(helper.EscapeHtml("<"), "EscapeHtml should not return null!");

            Assert.AreEqual(
                "&lt;div class=\"container\"&gt;&lt;p&gt;Hello, &amp; world!&lt;/p&gt;&lt;/div&gt;",
                helper.EscapeHtml("<div class=\"container\"><p>Hello, & world!</p></div>")
            );
            Assert.AreEqual("&lt;Hello world!&gt;", helper.EscapeHtml("<Hello world!>"));
            Assert.AreEqual("&lt;p&gt;Hello &amp; Goodbye&lt;/p&gt;", helper.EscapeHtml("<p>Hello & Goodbye</p>"));
            Assert.AreEqual("", helper.EscapeHtml(""));
        }

        [TestMethod]
        public void EscapeHtmlExceptionTest()
        {
            Helper helper = new();
            Assert.IsNotNull(helper, "new Helper() should not be null"); 

            var ex = Assert.ThrowsException<ArgumentException>(  
                () => helper.EscapeHtml(null!)
            );
            Assert.AreEqual("Argument 'html' is null", ex.Message);
        }

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
        public void EllipsisTest()  // ��� ����, �� � ������
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
            /* ���������� ��������� �� ��� ������������.
             * - ����� ���������� � ��� ��������� ������ ���������
             *    �������� �����. ³�������, ������������ ������
             *    ������ �� ��������� ������������ ������������.
             *    ! ������ "������������" � ������
             * - �������� ���� ��������� ���������� � "��������"
             *    ��������. ����� ���������� ���� "Exception" �� 
             *    �������������, ���� ������� ���������� ������ ����
             *    (�����, ���� �� ��� - ������� Exception)
             * - ���� ����������, �� ������� � �����, �����������
             *    � Assert, �� �������� ������ �������� (�����) ��
             *    ���� ���� �� ������.
             */
            Helper helper = new();
            // ����: helper.Ellipsis(null!, 1) �� "��������" ���������� ���� ArgumentNullException
            var ex =
                Assert.ThrowsException<ArgumentNullException>(
                    () => helper.Ellipsis(null!, 1)
                );
            // ����: ����������� ���������� (ex.Message) ������� ������ ����� ��������� (input)
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

            Dictionary<string[], string> testCases = new()
            {
                { new[] { "/home", "/index", "/gmail" }, "/home/index/gmail" },
                { new[] { "/shop", "/cart/", "com" }, "/shop/cart/com" },
                { new[] { "auth/", "logout" }, "/auth/logout" },
                { new[] { "forum", "topic/", "/com/" }, "/forum/topic/com" },
                { new[] { "//forum////", "topic////", "///com" }, "/forum/topic/com" },
                { new[] { "forum", "topic", "com" }, "/forum/topic/com" },
                { new[] { "/forum/", "/topic///////////", "//com////////////////" }, "/forum/topic/com" },
                { new[] { "/shop", "/cart", "/user", "..", "/123" }, "/shop/cart/123" },
                { new[] { "/shop///", "///cart", "user", "..", "////123///" }, "/shop/cart/123" },
                { new[] { "/shop///", "///cart", "user", "..", "////123///", "456" }, "/shop/cart/123/456" },
                { new[] { "/shop///", "///cart", "..", "user//", "///123", "456//" }, "/shop/user/123/456" },
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(
                    testCase.Value, 
                    helper.CombineUrl(testCase.Key),  
                    $"{testCase.Key[0]} + {testCase.Key[1]}" 
                );
            }
        }

        [TestMethod]
        public void CombineUrlExceptionTest()
        {
            Helper helper = new();

            Assert.AreEqual("/home", helper.CombineUrl("/home", null!));
            Assert.AreEqual("/home/path", helper.CombineUrl("/home", "///path//", null!));
            Assert.AreEqual("/home/user", helper.CombineUrl("/home", "///path//", "..", "user//", null!));

            var ex = Assert.ThrowsException<ArgumentException>(
                () => helper.CombineUrl(null!, null!)
            );
            Assert.AreEqual("All arguments are null", ex.Message);

            ex = Assert.ThrowsException<ArgumentException>(
                () => helper.CombineUrl(null!, null!, null!, null!, null!, null!)
            );
            Assert.AreEqual("All arguments are null", ex.Message);

            ex = Assert.ThrowsException<ArgumentException>(
                () => helper.CombineUrl()
            );
            Assert.AreEqual("Parts is empty", ex.Message);

            var ex2 = Assert.ThrowsException<NullReferenceException>(
                () => helper.CombineUrl(null!)
            );
            Assert.AreEqual("Parts is null", ex2.Message);

            var ex3 = Assert.ThrowsException<ArgumentException>(
                () => helper.CombineUrl(null!, "/subsection")
            );
            Assert.AreEqual("Non-Null argument after Null one", ex3.Message);

            ex3 = Assert.ThrowsException<ArgumentException>(
                () => helper.CombineUrl("/path", null!, "/subsection")
            );
            Assert.AreEqual("Non-Null argument after Null one", ex3.Message);

            ex3 = Assert.ThrowsException<ArgumentException>(
                () => helper.CombineUrl("/path", "/path2", null!, "/subsection")
            );
            Assert.AreEqual("Non-Null argument after Null one", ex3.Message);
        }
    }
}
/* �.�. ������ �������� ���� �, �� �����������, ������������ ����� 
 * CombineUrl ��� ����������� ����� �����
 * - �� �������� null � ����� ������� ���������
 * - ������� null, ������� �� null � ����� ����������
 * ������� - ���� null � ����������� ������ - �� �����,
 * ���� ���� null ��� ��-null, �� �� ����������
 *  
 *  ***
 *  ���� ���� � ��������� "..", �� ���������� ��������� ��������
 *  /shop/cart/user/../123 --> /shop/cart/123
 */
/* �.�. �������� ����� CombineUrl(part1, part2)
 * part1     part2      ret
 * /home     index      /home/index
 * /home/   /index      /home/index
 *  home/    index      /home/index
 *  
 * ��������� ��� ����� ��� ������� ������� ��������� 
 */
/* �.�. �������� ����� .Finalize(String) ���� ���� ��������
 * ����� �� ���� �����, ���� �� ��� ����. ���� �, �� �� ����
 * ������� ��� ����� �������� ����� � ���������� ������� 
 * ���������.
 * ������ �������� � ����������� ����������.
 */