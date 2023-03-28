using System.Runtime.Serialization;

namespace EventCommunity.Core.Exceptions
{
    [Serializable]
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string entity, string property, string value)
            : base(string.Format("{0} with {1} with the value of {2}, already exists!", entity, property, value))
        {
        }

        protected DuplicateEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
