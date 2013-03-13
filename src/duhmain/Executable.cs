using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duhmain
{
    /// <summary>
    /// Specifies the current state of an executable thing.
    /// </summary>
    public enum ExecutionState
    {
        NotExecuted,
        Executing,
        CompletedWithError,
        CompletedSuccessfully
    }

    /// <summary>
    /// Represents something that is executable.
    /// </summary>
    /// <typeparam name="TResult">The type of result that is generated.</typeparam>
    public interface IExecutable<TResult>
    {

        /// <summary>
        /// Executes the logic contained in this instance.
        /// </summary>
        /// <param name="context">The execution context currently being used.</param>
        TResult Execute(IContext context);

        /// <summary>
        /// The result of the execution.
        /// </summary>
        IExecutionResult<TResult> Result { get; }

        /// <summary>
        /// The current state of execution.
        /// </summary>
        ExecutionState ExecutionState { get; }

    }

    /// <summary>
    /// A non-generic result of an execution.
    /// </summary>


    /// <summary>
    /// Represents a result of an executable piece of logic. Contains generated values or exception details.
    /// </summary>
    /// <typeparam name="TResult">The type of result generated.</typeparam>
    public interface IExecutionResult<TResult>
    {
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if attempt to 
        /// retrieve the result is made if there was an exception during the execution of the command.
        /// </exception>
        TResult Result { get; }

        /// <summary>
        /// Gets the exception details of any exceptions that occured during the execution of the command. Will be null if no exception happened.
        /// </summary>
        Exception Exception { get; }

        /// <summary>
        /// Returns true if there were no exceptions during execution.
        /// </summary>
        bool CompletedSuccessfully { get; }
    }

    /// <summary>
    /// Base implementation of a generic execution result
    /// </summary>
    /// <typeparam name="TResult">The result of the execution.</typeparam>
    public class ExecutionResult<TResult> : IExecutionResult<TResult>
    {
        #region constructors

        public ExecutionResult(TResult result)
        {
            _result = result;
        }

        public ExecutionResult(Exception exception)
        {
            _exception = exception;
        }

        #endregion

        #region fields and properties

        private readonly Exception _exception;
        private readonly TResult _result;

        /// <summary>
        /// Gets the exception details of any exceptions that occured during the execution of the command. Will be null if no exception happened.
        /// </summary>
        public Exception Exception
        {
            get
            {
                return _exception;
            }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if attempt to 
        /// retrieve the result is made if there was an exception during the execution of the command.
        /// </exception>
        public TResult Result
        {
            get
            {
                if (!CompletedSuccessfully)
                {
                    throw new InvalidOperationException(
                        "The execution did not complete sucessfully. Check inner exception for more details.",
                        _exception);
                }

                return _result;
            }
        }


        /// <summary>
        /// Returns true if there were no exceptions during execution.
        /// </summary>
        public bool CompletedSuccessfully
        {
            get
            {
                bool completedSucessfully = (_exception == null);
                return completedSucessfully;
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents sommething that can be executed and has a result.
    /// </summary>
    public abstract class Executable<TResult> : IExecutable<TResult>
    {

        #region fields and properties

        private ExecutionState _state = ExecutionState.NotExecuted;
        private IExecutionResult<TResult> _result;

        /// <summary>
        /// The current state of execution.
        /// </summary>
        public ExecutionState ExecutionState
        {
            get
            {
                return _state;
            }
        }

        /// <summary>
        /// The result of the execution.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the execution has not completed or didn't complete sucessfully.</exception>
        public IExecutionResult<TResult> Result
        {
            get
            {
                return _result;
            }
        }

        #endregion

        /// <summary>
        /// Executes, using an ambient context.
        /// </summary>
        /// <returns>The result of the execution.</returns>
        public TResult Execute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes and returns the results or throws the exception
        /// </summary>
        /// <param name="context">The context to execute with.</param>
        /// <returns></returns>
        public TResult Execute(IContext context)
        {
            _state = ExecutionState.Executing;
            try
            {
                TResult executeResult = OnExecute(context);
                _result = new ExecutionResult<TResult>(executeResult);
                _state = ExecutionState.CompletedSuccessfully;
            }
            catch (Exception ex)
            {
                _state = ExecutionState.CompletedWithError;
                _result = new ExecutionResult<TResult>(ex);
                throw;
            }
            return _result.Result;
        }


        /// <summary>
        /// Called to do the work of execution.
        /// </summary>
        /// <param name="context">The context currently being executed under.</param>
        /// <returns>The result of the execution.</returns>
        protected abstract TResult OnExecute(IContext context);

    }
}
