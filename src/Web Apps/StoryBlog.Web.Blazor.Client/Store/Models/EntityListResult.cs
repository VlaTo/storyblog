using System;
using System.Collections;
using System.Collections.Generic;

namespace StoryBlog.Web.Client.Store.Models
{
    internal class EntityListResult<TEntity> : IEnumerable<TEntity>
    {
        public IEnumerable<TEntity> Entities
        {
            get;
        }

        public Uri BackwardUri
        {
            get;
        }

        public Uri ForwardUri
        {
            get;
        }

        public EntityListResult(IEnumerable<TEntity> entities, Uri backwardUri = null, Uri forwardUri = null)
        {
            Entities = entities;
            BackwardUri = backwardUri;
            ForwardUri = forwardUri;
        }

        public IEnumerator<TEntity> GetEnumerator() => Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}