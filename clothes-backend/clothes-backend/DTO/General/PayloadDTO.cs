namespace clothes_backend.DTO.General
{
    public class PayloadDTO<T>
    {
        public bool success { get; set; }
        public T data { get; set; }
        public string message { get; set; }
        //public static PayloadDTO<T> OK(T data)
        //{
        //    return new PayloadDTO<T> { success = true, data = data };
        //}
        //public static PayloadDTO<T> Error(string message)
        //{
        //    return new PayloadDTO<T> { success = false, message = message };
        //}
    }
}
