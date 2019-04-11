using System;

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

        private ModelStatus(ModelState state, string error = null)
        {
            State = state;
            Error = error;
        }

        static ModelStatus()
        {
            None = new ModelStatus(ModelState.None);
            Loading = new ModelStatus(ModelState.Loading);
            Success = new ModelStatus(ModelState.Success);
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
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

        /// <inheritdoc cref="object.Equals(object)" />
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

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) State * 397) ^ (Error != null ? Error.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ModelStatus Failed(string error) => new ModelStatus(ModelState.Error, error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ModelStatus left, ModelStatus right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ModelStatus left, ModelStatus right)
        {
            return false == Equals(left, right);
        }

        private static bool Equals(ModelStatus left, ModelStatus right) => false == ReferenceEquals(null, left) && left.Equals(right);
    }
}