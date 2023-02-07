public interface IRepository<T> {
    Task<bool> SaveAsync(T entity);
    Task<T?> GetAsync(int entityId);
}