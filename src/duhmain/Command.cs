using System;

namespace duhmain
{
    /// <summary>
    /// Deterines the state of a currently executing command
    /// </summary>
    public enum CommandState
    {
        NotExecuted,
        Executing,
        CompletedWithError,
        CompletedSuccessfully
    }

    /// <summary>
    /// Defines a command to be executed.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The current state of the command.
        /// </summary>
        CommandState State { get; }

        /// <summary>
        /// The result of the command if it has been executed
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the command hasn't been executed yet or has not finished executing.</exception>
        ICommandResult Result { get; }

        /// <summary>
        /// Executes the current task syncronously.
        /// </summary>
        object Execute(IContext context);

    }

    /// <summary>
    /// Implements an abstract pattern for encapsulating tasks to be executed.
    /// </summary>
    /// <typeparam name="TResult">The type of the result to expect.</typeparam>
    public abstract class Command<TResult> : ICommand
    {

        #region fields and properties

        private CommandResult<TResult> _result;
        private CommandState _state = CommandState.NotExecuted;

        /// <summary>
        /// The result of the command if it has been executed
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the command hasn't been executed yet or has not finished executing.</exception>
        public CommandResult<TResult> Result
        {
            get
            {
                if (_state != CommandState.CompletedSuccessfully
                    && _state != CommandState.CompletedWithError)
                {
                    throw new InvalidOperationException("The command has not completed executing, so you cannot inspect the result.");
                }
                return _result;
            }
        }

        ICommandResult ICommand.Result
        {
            get { return this.Result; }
        }

        /// <summary>
        /// The current state of the command.
        /// </summary>
        public CommandState State
        {
            get
            {
                return _state;
            }
            private set
            {
                _state = value;
            }
        }

        #endregion


        /// <summary>
        /// Executes the current task syncronously.
        /// </summary>
        public TResult Execute(IContext context)
        {
            _state = CommandState.Executing;
            try
            {
                TResult executeResult = OnExecute(context);
                _result = new CommandResult<TResult>(executeResult);
                _state = CommandState.CompletedSuccessfully;
            }
            catch (Exception ex)
            {
                _state = CommandState.CompletedWithError;
                _result = new CommandResult<TResult>(ex);
                throw;
            }
            return _result.Result;
        }

        object ICommand.Execute(IContext context)
        {
            return this.Execute(context);
        }


        /// <summary>
        /// Overridden by derived classes to run the actual command.
        /// </summary>
        /// <returns>The result of the operation if sucessful</returns>
        /// <exception cref="System.Exception">Any type of exception thrown if the command was not sucessful.</exception>
        protected abstract TResult OnExecute(IContext context);

    }
}
