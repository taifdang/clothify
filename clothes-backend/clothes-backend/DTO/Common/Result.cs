using clothes_backend.Heplers.General;
using clothes_backend.Utils.Enum;
namespace clothes_backend.DTO.General
{
    public class Result<T>
    {      
        public StatusCode statusCode { get; set; }
        private string error;
        public string message
        {
            //get { return ErrorMsg.getMessage(statusCode); }
            get => string.IsNullOrEmpty(error) ? ErrorMsg.getMessage(statusCode) : error;
            set => error = value;
        }
        public T data { get; set; }       
        public static Result<T> Success(T data = default!)
        {
            return new Result<T> { data = data, statusCode = StatusCode.Success};
        }
        public static Result<T> Failure(StatusCode statusCode = default)
        {
            return new Result<T> { statusCode = statusCode };
        }
        public static Result<T> IsValid(string? errmsg = null)
        {
            return new Result<T>
            {
                statusCode = StatusCode.Isvalid,
                data = default,
                message = errmsg
            };
        }
    }
}
