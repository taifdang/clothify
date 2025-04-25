using System.Collections.Concurrent;

namespace clothes_backend.Service.EMAIL
{
    public class HandleQueue<T>
    {
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly SemaphoreSlim _semophore = new SemaphoreSlim(0);

        public void Write(T item)
        {
            _queue.Enqueue(item);
            _semophore.Release();
        }
        public async Task<T> ReadAsync()
        {
            await _semophore.WaitAsync();
            _queue.TryDequeue(out T item);
            return item;
        }
        
    }
}
        