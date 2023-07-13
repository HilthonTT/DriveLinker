namespace DriveLinker.Core.Helpers;
public static class EnumerableHelper
{
    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(
        this IEnumerable<TSource> source, Func<TSource, Task<TResult>> method,
        int concurrency = int.MaxValue)
    {
        var semaphore = new SemaphoreSlim(concurrency);
        try
        {
            return await Task.WhenAll(source.Select(async s =>
            {
                try
                {
                    await semaphore.WaitAsync();
                    return await method(s);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        finally
        {
            semaphore.Dispose();
        }
    }
}
