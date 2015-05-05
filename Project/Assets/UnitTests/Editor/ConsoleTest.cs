using ConsoleX.Exceptions;
using NUnit.Framework;
using UnityEngine;

namespace ConsoleX.UnitTests
{
    [TestFixture]
    public class ConsoleTest : UnityUnitTest
    {
        #region RegisterCommand

        [Test]
        public void RegisterCommand_DelegatesNotExistingCommand_Registered()
        {
            var console = GetConsole();

            console.RegisterCommand("test", CommandAction);

            Assert.IsTrue(console.IsRegistered("test"));
        }

        [Test]
        [ExpectedException(typeof(ConsoleCommandAlreadyRegistered), ExpectedMessage = "'test' command is already registered.")]
        public void RegisterCommand_DelegatesExistingCommand_Registered()
        {
            var console = GetConsole();

            console.RegisterCommand("test", CommandAction, s => new string[0]);
            console.RegisterCommand("test", CommandAction);
        }

        [Test]
        [ExpectedException(typeof(ConsoleCommandAlreadyRegistered), ExpectedMessage = "'Test' command is already registered.")]
        public void RegisterCommand_DelegatesExistingDifCaseCommand_Registered()
        {
            var console = GetConsole();

            console.RegisterCommand("test", CommandAction, s => new string[0]);
            console.RegisterCommand("Test", CommandAction);
        }

        [Test]
        public void RegisterCommand_ObjectNotExistingCommand_Registered()
        {
            var console = GetConsole();
            var command = new ConsoleCommand("test", strings => { });

            console.RegisterCommand(command);

            Assert.IsTrue(console.IsRegistered(command));
        }

        [Test]
        [ExpectedException(typeof(ConsoleCommandAlreadyRegistered), ExpectedMessage = "'test' command is already registered.")]
        public void RegisterCommand_ObjectExistingCommand_Registered()
        {
            var console = GetConsole();
            var command = new ConsoleCommand("test", strings => { });

            console.RegisterCommand(command);
            console.RegisterCommand(command);
        }

        [Test]
        public void RegisterAsyncCommand_DelegatesNotExistingCommand_Registered()
        {
            var console = GetConsole();

            console.RegisterAsyncCommand("test", CommandAction, () => true);

            Assert.IsTrue(console.IsRegistered("Test"));
        }

        [Test]
        [ExpectedException(typeof(ConsoleCommandAlreadyRegistered), ExpectedMessage = "'test' command is already registered.")]
        public void RegisterAsyncCommand_DelegatesExistingCommand_Registered()
        {
            var console = GetConsole();

            console.RegisterAsyncCommand("test", CommandAction, () => true, s => new string[0]);
            console.RegisterAsyncCommand("test", CommandAction, () => true);
        }

        [Test]
        public void TryRegisterCommand_DelegatesNotExisting_ReturnTrue()
        {
            var console = GetConsole();

            var result = console.TryRegisterCommand("test", CommandAction);

            Assert.IsTrue(result);
            Assert.IsTrue(console.IsRegistered("test"));
        }

        [Test]
        public void TryRegisterCommand_DelegatesExisting_ReturnFalse()
        {
            var console = GetConsole();
            console.TryRegisterCommand("test", CommandAction, s => new string[0]);
            
            var result = console.TryRegisterCommand("test", CommandAction, s => new string[0]);

            Assert.IsFalse(result);
            Assert.IsTrue(console.IsRegistered("test"));
        }

        [Test]
        public void TryRegisterAsyncCommand_DelegatesNotExisting_ReturnTrue()
        {
            var console = GetConsole();

            var result = console.TryRegisterAsyncCommand("test", CommandAction, () => false);

            Assert.IsTrue(result);
            Assert.IsTrue(console.IsRegistered("test"));
        }

        [Test]
        public void TryRegisterAsyncCommand_DelegatesExisting_ReturnFalse()
        {
            var console = GetConsole();
            console.TryRegisterAsyncCommand("test", CommandAction, () => false);

            var result = console.TryRegisterAsyncCommand("test", CommandAction, () => false, s => new string[0]);

            Assert.IsFalse(result);
            Assert.IsTrue(console.IsRegistered("test"));
        }

        [Test]
        public void TryRegisterCommand_ObjectNotExistingCommand_ReturnTrue()
        {
            var console = GetConsole();
            var command = new ConsoleCommand("test", strings => { });

            var result = console.TryRegisterCommand(command);

            Assert.IsTrue(result);
            Assert.IsTrue(console.IsRegistered(command));
        }

        [Test]
        public void TryRegisterCommand_ObjectExistingCommand_ReturnFalse()
        {
            var console = GetConsole();
            var command = new ConsoleCommand("test", strings => { });
            console.TryRegisterCommand(command);
            
            var result = console.TryRegisterCommand(command);

            Assert.IsFalse(result);
            Assert.IsTrue(console.IsRegistered(command));
        }

        #endregion

        #region ComapleteCommandLine
        
        [Test]
        public void CompleteCommandLine_ExistingFullCommand_ReturnCommand()
        {
            var console = GetConsole();
            console.RegisterCommand("test", CommandAction);

            var result = console.CompleteCommandLine("test");

            Assert.AreEqual(new[] {"test"}, result);
        }

        [Test]
        public void CompleteCommandLine_SingleExistingHalfCommand_ReturnCommand()
        {
            var console = GetConsole();
            console.RegisterCommand("test", CommandAction);

            var result = console.CompleteCommandLine("te");

            Assert.AreEqual(new[] {"test"}, result);
        }

        [Test]
        public void CompleteCommandLine_TwoExistingHalfCommand_ReturnTwoCommands()
        {
            var console = GetConsole();
            console.RegisterCommand("test", CommandAction);
            console.RegisterCommand("test2", CommandAction);

            var result = console.CompleteCommandLine("te");

            Assert.AreEqual(new[] {"test", "test2"}, result);
        }

        [Test]
        public void CompleteCommandLine_MultipleExistingHalfCommand_ReturnTwoCommands()
        {
            var console = GetConsole();
            console.RegisterCommand("lol", CommandAction);
            console.RegisterCommand("test", CommandAction);
            console.RegisterCommand("test2", CommandAction);

            var result = console.CompleteCommandLine("te");

            Assert.AreEqual(new[] {"test", "test2"}, result);
        }

        [Test]
        public void CompleteCommandLine_MultipleExistingFullSmallerCommand_ReturnTwoCommands()
        {
            var console = GetConsole();
            console.RegisterCommand("lol", CommandAction);
            console.RegisterCommand("test", CommandAction);
            console.RegisterCommand("test2", CommandAction);

            var result = console.CompleteCommandLine("test");

            Assert.AreEqual(new[] { "test", "test2" }, result);
        }

        [Test]
        public void CompleteCommandLine_CommandWithSpace_ReturnAutoCompleteList()
        {
            var console = GetConsole();
            bool isAutoCompleteCalled = false;
            string recievedParam = null;
            console.RegisterCommand("test", CommandAction, s =>
            {
                isAutoCompleteCalled = true;
                recievedParam = s;
                return new[] {"param1", "param2"};
            });

            var result = console.CompleteCommandLine("test ");

            Assert.IsTrue(isAutoCompleteCalled);
            Assert.AreEqual("", recievedParam);
            CollectionAssert.AreEqual(new[] { "param1", "param2" }, result);
        }

        [Test]
        public void CompleteCommandLine_CommandWithArguments_ReturnAutoCompleteList()
        {
            var console = GetConsole();
            bool isAutoCompleteCalled = false;
            string recievedParam = null;
            console.RegisterCommand("test", CommandAction, s =>
            {
                isAutoCompleteCalled = true;
                recievedParam = s;
                return new[] { "param1", "param2" };
            });

            var result = console.CompleteCommandLine("test inParam1 inParam2");

            Assert.IsTrue(isAutoCompleteCalled);
            Assert.AreEqual("inParam1 inParam2", recievedParam);
            CollectionAssert.AreEqual(new[] { "param1", "param2" }, result);
        }

        [Test]
        public void CompleteCommandLine_NotRegisteredCommandWithArguments_EmptyList()
        {
            var console = GetConsole();
            
            var result = console.CompleteCommandLine("test inParam1 inParam2");

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void CompleteCommandLine_CommandWithoutAutoComplete_EmptyList()
        {
            var console = GetConsole();
            console.RegisterCommand("test", CommandAction);

            var result = console.CompleteCommandLine("test inParam1 inParam2");

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void CompleteCommandLine_CommandUpperCase_Command()
        {
            var console = GetConsole();
            console.RegisterCommand("test", CommandAction);

            var result = console.CompleteCommandLine("Tes");

            CollectionAssert.AreEqual(new[] {"test"}, result);
        }

        [Test]
        public void CompleteCommandLine_CommandUpperCaseWithArguments_Command()
        {
            var console = GetConsole();

            bool isAutoCompleteCalled = false;
            string recievedParam = null;
            console.RegisterCommand("test", CommandAction, s =>
            {
                isAutoCompleteCalled = true;
                recievedParam = s;
                return new[] { "param1", "param2" };
            });

            var result = console.CompleteCommandLine("Test arg");

            Assert.IsTrue(isAutoCompleteCalled);
            Assert.AreEqual("arg", recievedParam);
            CollectionAssert.AreEqual(new[] { "param1", "param2" }, result);
        }

        #endregion

        [Test]
        [ExpectedException(typeof(CommandLineParseException))]
        public void ExecuteCommandLine_EmptyCommand_ThrowException()
        {
            var console = GetConsole();
            console.ExecuteCommandLine("");
        }

        [Test]
        [ExpectedException(typeof(CommandLineParseException))]
        public void ExecuteCommandLine_NullCommand_ThrowException()
        {
            var console = GetConsole();
            console.ExecuteCommandLine(null);
        }

        [Test]
        [ExpectedException(typeof(ConsoleCommandNotRegistered), ExpectedMessage = "'test' command is not registered")]
        public void ExecuteCommandLine_NotExistingCommand_ThrowException()
        {
            var console = GetConsole();
            console.ExecuteCommandLine("test");
        }

        [Test]
        public void ExecuteCommandLine_ExistingCommandWithoutArguments_Executed()
        {
            var isExecuted = false;
            string[] args = null;
            var console = GetConsole();
            console.RegisterCommand("test",
                strings =>
                {
                    isExecuted = true;
                    args = strings;
                });

            console.ExecuteCommandLine("test");
            console.Update();

            Assert.IsTrue(isExecuted);
            Assert.IsNotNull(args);
            CollectionAssert.IsEmpty(args);
        }

        [Test]
        public void ExecuteCommandLine_ExistingCommandWithArguments_Executed()
        {
            var isExecuted = false;
            string[] args = null;
            var console = GetConsole();
            console.RegisterCommand(
                "test",
                strings =>
                {
                    isExecuted = true;
                    args = strings;
                });

            console.ExecuteCommandLine("test a 1 \"test with space\" 'test test' \"not closed");
            console.Update();

            Assert.IsTrue(isExecuted);
            Assert.IsNotNull(args);
            CollectionAssert.AreEqual(new[] { "a", "1", "test with space", "'test", "test'", "not closed" }, args);
        }

        #region DeregisterCommand

        [Test]
        [ExpectedException(typeof (ConsoleCommandNotRegistered))]
        public void DeregisterCommand_StringNotExistingCommand_ThrowException()
        {
            var target = GetConsole();
            
            target.DeregisterCommand("notExisting");
        }

        [Test]
        public void DeregisterCommand_StringExistingCommand_Deregister()
        {
            var target = GetConsole();
            target.RegisterCommand("test", strings => {});

            target.DeregisterCommand("test");
            Assert.IsFalse(target.IsRegistered("test"));
        }

        [Test]
        public void TryDeregisterCommand_StringNotExistingCommand_ReturnFalse()
        {
            var target = GetConsole();

            var result = target.TryDeregisterCommand("notExisting");

            Assert.IsFalse(result);
        }

        [Test]
        public void TryDeregisterCommand_StringExistingCommand_ReturnTrue()
        {
            var target = GetConsole();
            target.RegisterCommand("test", strings => { });

            var result = target.TryDeregisterCommand("test");

            Assert.IsTrue(result);
            Assert.IsFalse(target.IsRegistered("test"));
        }

        [Test]
        [ExpectedException(typeof(ConsoleCommandNotRegistered))]
        public void DeregisterCommand_ObjectNotExistingCommand_ThrowException()
        {
            var target = GetConsole();
            
            target.DeregisterCommand(new ConsoleCommand("Test", strings => { }));
        }

        [Test]
        public void DeregisterCommand_ObjectExistingCommand_Deregister()
        {
            var target = GetConsole();
            var command = new ConsoleCommand("test", strings => { });
            target.RegisterCommand(command);

            target.DeregisterCommand(command);
            Assert.IsFalse(target.IsRegistered(command));
        }

        [Test]
        public void TryDeregisterCommand_ObjectNotExistingCommand_ReturnFalse()
        {
            var target = GetConsole();
            
            var result = target.TryDeregisterCommand(new ConsoleCommand("test", strings => { }));

            Assert.IsFalse(result);
        }

        [Test]
        public void TryDeregisterCommand_ObjectExistingCommand_ReturnTrue()
        {
            var target = GetConsole();
            var command = new ConsoleCommand("test", strings => { });
            target.RegisterCommand(command);

            var result = target.TryDeregisterCommand(command);

            Assert.IsTrue(result);
            Assert.IsFalse(target.IsRegistered(command));
        }

        #endregion

        private Console GetConsole()
        {
            var go = CreateGameObject();
            var console = go.AddComponent<Console>();
            console.Awake();
            return console;
        }

        private void CommandAction(string[] arguments)
        {
            Debug.Log("Action :" + string.Join(" ", arguments));
        }
    }

}