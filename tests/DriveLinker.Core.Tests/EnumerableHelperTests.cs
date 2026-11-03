using DriveLinker.Core.Helpers;

namespace DriveLinker.Core.Tests;

public class EnumerableHelperTests
{
    [Fact]
    public async Task SelectAsync_preserves_order()
    {
        var source = Enumerable.Range(1, 50).ToList();

        var result = await source.SelectAsync(async i =>
        {
            await Task.Delay(Random.Shared.Next(0, 5));
            return i * 2;
        }, concurrency: 4);

        Assert.Equal(source.Select(i => i * 2), result);
    }

    [Fact]
    public async Task SelectAsync_respects_the_concurrency_limit()
    {
        int running = 0;
        int maxObserved = 0;

        await Enumerable.Range(0, 20).SelectAsync(async _ =>
        {
            int now = Interlocked.Increment(ref running);
            InterlockedMax(ref maxObserved, now);
            await Task.Delay(10);
            Interlocked.Decrement(ref running);
            return 0;
        }, concurrency: 3);

        Assert.InRange(maxObserved, 1, 3);
    }

    private static void InterlockedMax(ref int target, int value)
    {
        int current;
        while ((current = Volatile.Read(ref target)) < value &&
               Interlocked.CompareExchange(ref target, value, current) != current)
        {
        }
    }
}
