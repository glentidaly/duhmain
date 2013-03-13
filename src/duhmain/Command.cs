using System;

namespace duhmain
{
  
    /// <summary>
    /// Defines a command to be executed.
    /// </summary>
    public interface ICommand<TResult> : IExecutable<TResult>
    {
        

    }

    /// <summary>
    /// Implements an abstract pattern for encapsulating tasks to be executed.
    /// </summary>
    /// <typeparam name="TResult">The type of the result to expect.</typeparam>
    public abstract class Command<TResult> : Executable<TResult>, ICommand<TResult>
    {

    }

}
