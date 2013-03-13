using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duhmain
{
    /// <summary>
    /// Represents a query that returns a paged sequence of results
    /// </summary>
    /// <typeparam name="TResult">The type of result being retuned.</typeparam>
    public interface IPagedQuery<TResult> : IQuery<IPagedQueryResult<TResult>>
    {
        /// <summary>
        /// Returns the specification of the page used to get the results.
        /// </summary>
        PageSpecification PageSpecification { get; }

    }

    /// <summary>
    /// Instructions on the current page size.
    /// </summary>
    /// <remarks>This type is immutable</remarks>
    public struct PageSpecification
    {

        #region constructors

        /// <summary>
        /// Creates a new instance of a page specification.
        /// </summary>
        /// <param name="pageSize">The number of items in a page</param>
        /// <param name="itemIndex">The index of the first item of the page.</param>
        public PageSpecification(uint pageSize, uint itemIndex)
        {
            _pageSize = pageSize;
            _itemIndex = itemIndex;
        }


        #endregion

        #region fields and properties

        private readonly uint _pageSize;
        private readonly uint _itemIndex;

        /// <summary>
        /// The number of items in each page.
        /// </summary>
        public uint PageSize
        {
            get
            {
                return _pageSize;
            }
        }

        /// <summary>
        /// The index of the first item in the list to return.
        /// </summary>
        public uint ItemIndex
        {
            get
            {
                return _itemIndex;
            }
        }

        #endregion

    }

    /// <summary>
    /// Represents a result of a multiple result query that supports paging.
    /// </summary>
    public interface IPagedQueryResult<TResult>
    {

        /// <summary>
        /// The total number of results in all pages (if available)
        /// </summary>
        long? TotalResultCount { get; }

        /// <summary>
        /// Returns the specification of the page used to get these results.
        /// </summary>
        PageSpecification PageSpecification { get; }

        /// <summary>
        /// Gets an enumerator for the current page of results.
        /// </summary>
        IEnumerable<TResult> Results { get; }

        /// <summary>
        /// Returns a query that may be executed to get the next page of results with the same criteria that was used to
        /// get this set of results.
        /// </summary>
        /// <returns>The query that can be executed to get the next page of results.</returns>
        IPagedQuery<TResult> GetNextPageQuery();

    }

    /// <summary>
    /// A base class for paged query results.
    /// </summary>
    /// <typeparam name="TResult">The type of items returned</typeparam>
    public abstract class PagedQuery<TResult> :  Query<IPagedQueryResult<TResult>>, IPagedQuery<TResult>
    {

        #region constructors

        /// <summary>
        /// Creates an instance with the given paged specification.
        /// </summary>
        /// <param name="pageSpec">The page specification to use as a starting point.</param>
        public PagedQuery(PageSpecification pageSpec)
        {
            _pageSpecification = pageSpec;
        }

        #endregion

        #region fields and properties

        private readonly PageSpecification _pageSpecification;

        /// <summary>
        /// The current specification in place for controlling the paged results returned.
        /// </summary>
        public PageSpecification PageSpecification
        {
            get
            {
                return _pageSpecification;
            }
        }

        #endregion


        //TODO: Implement the rest of this to override the execute and pass in the page specification etc.

    }
}
