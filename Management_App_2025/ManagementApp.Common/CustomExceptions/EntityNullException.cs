namespace ManagementApp.Common.CustomExceptions
{
    public class EntityNullException : Exception
    {
        public EntityNullException()
        {            
        }

        public EntityNullException(string message) : base(message)
        { 
        }
    }
}
