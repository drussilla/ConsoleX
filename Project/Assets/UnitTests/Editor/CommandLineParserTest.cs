using NUnit.Framework;

namespace ConsoleX.UnitTests
{
    [TestFixture]
    public class CommandLineParserTest : UnityUnitTest
    {
        [Test]
        public void Parse_NoArguments_EmptyArgs()
        {
            var target = GetParser();
            string command;
            string[] args;
            
            var result = target.TryParse("test", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.IsEmpty(args);
        }

        [Test]
        public void Parse_NoArgumentsOneSpaceAtTheEnd_EmptyArgs()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("test ", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.IsEmpty(args);
        }

        [Test]
        public void Parse_NoArgumentsMultipleSpacesAtTheEnd_EmptyArgs()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("test    ", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.IsEmpty(args);
        }

        [Test]
        public void Parse_NoArgumentsMultipleSpacesAtTheBegin_EmptyArgs()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("    test", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.IsEmpty(args);
        }

        [Test]
        public void Parse_OneSimpleArgument_OneArg()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("test arg1", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.AreEqual(new [] {"arg1"}, args);
        }

        [Test]
        public void Parse_OneSpaceArgument_OneArg()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("test \"arg1 with space\"", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.AreEqual(new[] { "arg1 with space" }, args);
        }

        [Test]
        public void Parse_TwoSimpleArgument_TwoArg()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("test arg1 arg2", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.AreEqual(new[] { "arg1", "arg2" }, args);
        }

        [Test]
        public void Parse_TwoSpaceArgument_TwoArg()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("test \"a 1\" \"a 2\"", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.AreEqual(new[] { "a 1", "a 2" }, args);
        }

        [Test]
        public void Parse_TwoSimpleArgumentWithMultipleSpacesBetween_TwoArg()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("test arg1     arg2", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.AreEqual(new[] { "arg1", "arg2" }, args);
        }

        [Test]
        public void Parse_ThreeSpaceArgumentNotClosed_ThreeArg()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("test \"a 1\" a \"a 2 2", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.AreEqual(new[] { "a 1", "a", "a 2 2" }, args);
        }

        [Test]
        public void Parse_MixedArguments_AllReturned()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("test \"a 1\" a \"a 2 2\" a", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.AreEqual(new[] { "a 1", "a", "a 2 2", "a" }, args);
        }

        [Test]
        public void Parse_QuotInTheMiddleArgument_AllReturned()
        {
            var target = GetParser();
            string command;
            string[] args;

            var result = target.TryParse("test \"a 1\"test\"a 2 2\" a", out command, out args);

            Assert.IsTrue(result);
            Assert.AreEqual("test", command);
            Assert.IsNotNull(args);
            CollectionAssert.AreEqual(new[] { "a 1testa 2 2", "a" }, args);
        }

        private CommandLineParser GetParser()
        {
            var go = CreateGameObject();
            var parser = go.AddComponent<CommandLineParser>();
            return parser;
        }
    }
}