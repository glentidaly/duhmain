using System;

namespace duhmain
{
    /// <summary>
    /// Encapsulates the result of a command
    /// </summary>
    public interface ICommandResult
    {
        /// <summary>
        /// Gets the exception details of any exceptions that occured during the execution of the command
        /// </summary>
        Exception Exception { get; }

        /// <summary>
        /// Gets the result of the command.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if attempt to 
        /// retrieve the result is made if there was an exception during the execution of the command.
        /// </exception>
        object Result { get; }
    }

    /// <summary>
    /// Encapsulates the result of a command
    /// </summary>
    /// <typeparam name="TResult">The concrete type of the result to be returned</typeparam>
    /// <remarks>This type is immutable</remarks>
    public class CommandResult<TResult> : ICommandResult
    {

        #region constructors

        public CommandResult(TResult result)
        {
            _result = result;
        }

        public CommandResult(Exception exception)
        {
            _exception = exception;
        }

        #endregion

        #region fields and properties

        private readonly Exception _exception;
        private readonly TResult _result;

        /// <summary>
        /// Gets the exception details of any exceptions that occured during the execution of the command
        /// </summary>
        public Exception Exception
        {
            get
            {
                return _exception;
            }
        }

        /// <summary>
        /// Gets the result of the command.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if attempt to 
        /// retrieve the result is made if there was an exception during the execution of the command.
        /// </exception>
        public TResult Result
        {
            get
            {
                if (!CompletedSucessfully)
                {
                    throw new InvalidOperationException(
                        "The command did not complete sucessfully. Check inner exception for more details.",
                        _exception);
                }

                return _result;
            }
        }

        object ICommandResult.Result
        {
            get
            {
                return this.Result;
            }
        }

        /// <summary>
        /// Returns true if there were no exceptions during execution.
        /// </summary>
        public bool CompletedSucessfully
        {
            get
            {
                bool completedSucessfully = (_exception == null);
                return completedSucessfully;
            }
        }

        #endregion

    }
}
