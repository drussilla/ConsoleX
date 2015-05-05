using System;
using NUnit.Framework;

namespace ConsoleX.UnitTests
{
    [TestFixture]
    public class ConsoleCommandTest : UnityUnitTest
    {
        [Test]
        public void Ctor_SimpleCommand_Created()
        {
            var action = new Action<string[]>(s => { });

            var target = new ConsoleCommand("test", action);

            Assert.AreEqual("test", target.Name);

            Assert.AreEqual(target.Action, action);
            
            Assert.IsNotNull(target.IsFinished);
            Assert.IsTrue(target.IsFinished());

            Assert.IsNotNull(target.AutoComplete);
            CollectionAssert.IsEmpty(target.AutoComplete(""));
        }

        [Test]
        public void Ctor_AutoCompleteCommand_Created()
        {
            var action = new Action<string[]>(s => { });
            var autoComplete = new Func<string, string[]>(s => new [] {s});

            var target = new ConsoleCommand("test", action, autoComplete);

            Assert.AreEqual("test", target.Name);

            Assert.AreEqual(target.Action, action);

            Assert.IsNotNull(target.IsFinished);
            Assert.IsTrue(target.IsFinished());

            Assert.IsNotNull(target.AutoComplete);
            Assert.AreEqual(1, target.AutoComplete("test").Length);
            CollectionAssert.AreEqual(new [] {"test"}, target.AutoComplete("test"));
        }

        [Test]
        public void Ctor_IsFinishedCommand_Created()
        {
            var action = new Action<string[]>(s => { });
            var isFinised = new Func<bool>(() => false);

            var target = new ConsoleCommand("test", action, isFinised);

            Assert.AreEqual("test", target.Name);

            Assert.AreEqual(target.Action, action);

            Assert.IsNotNull(target.IsFinished);
            Assert.IsFalse(target.IsFinished());

            Assert.IsNotNull(target.AutoComplete);
            CollectionAssert.IsEmpty(target.AutoComplete(""));
        }

        [Test]
        public void Ctor_CompleteCommand_Created()
        {
            var action = new Action<string[]>(s => { });
            var isFinised = new Func<bool>(() => false);
            var autoComplete = new Func<string, string[]>(s => new[] { s });

            var target = new ConsoleCommand("test", action, isFinised, autoComplete);

            Assert.AreEqual("test", target.Name);

            Assert.AreEqual(target.Action, action);

            Assert.IsNotNull(target.IsFinished);
            Assert.IsFalse(target.IsFinished());

            Assert.IsNotNull(target.AutoComplete);
            Assert.AreEqual(1, target.AutoComplete("test").Length);
            CollectionAssert.AreEqual(new[] { "test" }, target.AutoComplete("test"));
        }
    }
}