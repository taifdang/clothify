namespace clothes_backend.Repository
{
    public class GenericResponse<T>
    {
        public string status { get; set; }     
        public T? data { get; set; }

        public static GenericResponse<T> OK(T data)
        {
            return new GenericResponse<T> { status = "success", data = data };
        }
        public static GenericResponse<T> Fail(string? message = null)
        {
            return new GenericResponse<T> { status = "error "+ message };
        }
    }
}
