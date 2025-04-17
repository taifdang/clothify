using clothes_backend.Utils.Enum;

namespace clothes_backend.DTO.General
{
    public static class ResponseDTO<T>
    {
        public static PayloadDTO<T> success(T data)
        {
            return new PayloadDTO<T> { success = true, data = data };
        }
        public static PayloadDTO<T> fail(ErrorType statusCode)
        {
            return new PayloadDTO<T> { success = false, statusCode = statusCode };
        }
    }
}
