public interface IObjectPool<A>
{
    /// <summary>
    /// Gets an object from the pool.
    /// </summary>
    /// <returns></returns>
    public abstract A Get();

     /// <summary>
    /// Returns the object to the pool.
    /// </summary>
    /// <param name="obj"></param>
    public abstract void Return(A obj);

    public abstract void Clear();
}
