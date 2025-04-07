namespace clothes_backend.Inteface
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();

    }
}
