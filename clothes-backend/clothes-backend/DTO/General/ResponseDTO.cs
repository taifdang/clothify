namespace clothes_backend.DTO.General
{
    public static class ResponseDTO<T>
    {
        public static PayloadDTO<T> success(T data)
        {
            return new PayloadDTO<T> { success = true, data = data };
        }
        public static PayloadDTO<T> fail(string message)
        {
            return new PayloadDTO<T> { success = false, message = message };
        }
    }
}
