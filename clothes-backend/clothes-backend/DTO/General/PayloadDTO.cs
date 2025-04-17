using clothes_backend.Service;
using clothes_backend.Utils.Enum;

namespace clothes_backend.DTO.General
{
    public class PayloadDTO<T>
    {
        //public bool success { get; set; }
        
        //public string message { get; set; }
        public StatusCode statusCode { get; set; }
        public string message
        {
            get { return MessageResponse.getMessage(statusCode); }
        }
        public T data { get; set; }
       
        public static PayloadDTO<T> OK(T data)
        {
            return new PayloadDTO<T> { data = data, statusCode = StatusCode.Success};
        }
        public static PayloadDTO<T> Error(StatusCode statusCode)
        {
            return new PayloadDTO<T> { statusCode = statusCode };
        }
    }
}
