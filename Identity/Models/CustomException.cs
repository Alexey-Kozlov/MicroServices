namespace Common
{
    public class CustomException : Exception
    {
        private string _message;
        public CustomException(string message) : base()
        {
            _message = message;
        }
        public override string StackTrace
        {
            get { return _message; }
        }

    }
}
