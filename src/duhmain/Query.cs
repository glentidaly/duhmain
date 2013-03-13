using System;

namespace duhmain
{
    

    /// <summary>
    /// Represents an executable query that returns a single result and 
    /// has no side effects on the system.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned</typeparam>
    public interface IQuery<TResult> : IExecutable<TResult>
    {
        
    }

    /// <summary>
    /// Represents an executable query that returns a single result and has no side effects on the system
    /// </summary>
    /// <typeparam name="TResult">The type of result returned</typeparam>
    public abstract class Query<TResult> : Executable<TResult>, IQuery<TResult>
    {

    }
}
