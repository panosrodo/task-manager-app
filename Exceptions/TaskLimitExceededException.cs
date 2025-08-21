namespace TaskManagerApp.Exceptions
{
    public class TaskLimitExceededException : AppException
    {
        private static readonly string DEFAULT_CODE = "TaskLimitExceeded";
        public TaskLimitExceededException(string code, string message)
            : base(code + DEFAULT_CODE, message)
        {
        }
        public TaskLimitExceededException(string message)
            : base(DEFAULT_CODE, message)
        {
        }
    }
}
