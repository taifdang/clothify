namespace clothes_backend.Interfaces
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();

    }
}
