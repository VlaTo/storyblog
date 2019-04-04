using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    public enum ModelState
    {
        Error = -1,
        None,
        Loading,
        Success
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ModelStatus : IEquatable<ModelStatus>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly ModelStatus Loading;

        /// <summary>
        /// 
        /// </summary>
        public static readonly ModelStatus Success;
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly ModelStatus None;

        /// <summary>
        /// 
        /// </summary>
        public ModelState State
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Error
        {
            get;
        }

        private ModelStatus(ModelState state, string error)
        {
            State = state;
        }

        static ModelStatus()
        {
            None = new ModelStatus(ModelState.None, null);
            Loading = new ModelStatus(ModelState.Loading, null);
            Success = new ModelStatus(ModelState.Success, null);
        }

        public bool Equals(ModelStatus other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return State == other.State && String.Equals(Error, other.Error);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is ModelStatus other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) State * 397) ^ (Error != null ? Error.GetHashCode() : 0);
            }
        }

        public static ModelStatus Failed(string error) => new ModelStatus(ModelState.Error, error);

        private static bool Equals(ModelStatus left, ModelStatus right) => false == ReferenceEquals(null, left) && left.Equals(right);

        public static bool operator ==(ModelStatus left, ModelStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ModelStatus left, ModelStatus right)
        {
            return false == Equals(left, right);
        }
    }
}