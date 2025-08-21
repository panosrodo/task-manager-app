namespace TaskManagerApp.Exceptions
{
    public class TaskDeleteNotAllowedException : AppException
    {
        private static readonly string DEFAULT_CODE = "TaskDeleteNotAllowed";
        public TaskDeleteNotAllowedException(string code, string message)
            : base(code + DEFAULT_CODE, message)
        {
        }
        public TaskDeleteNotAllowedException(string message)
            : base(DEFAULT_CODE, message)
        {
        }
    }
}
