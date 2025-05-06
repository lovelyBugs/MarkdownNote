namespace NoteService
{
    public class APIResponse<T>
    {
        public bool Succeed { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public static APIResponse<T> Success(T data)
        {
            return new APIResponse<T>()
            {
                Succeed = true,
                Data = data,
                ErrorMessage = string.Empty
            };
        }
        public static APIResponse <T> Failure(string errorMsg)
        {
            return new APIResponse<T>()
            {
                Succeed = false,
                Data = default(T),
                ErrorMessage = errorMsg
            };
        }
    }
}
