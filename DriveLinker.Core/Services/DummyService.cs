using DriveLinker.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace DriveLinker.Core.Services;
public class DummyService : IDummyService
{
    private const string CacheName = nameof(DummyService);
    private readonly IMemoryCache _cache;

    public DummyService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public List<Drive> GetDummyDrives()
    {
        var output = _cache.Get<List<Drive>>(CacheName);
        if (output is null)
        {
            output = GetDrives();
            _cache.Set(CacheName, output);
        }

        return output;
    }

    private static List<Drive> GetDrives()
    {
        return new List<Drive>()
        {
            new()
            {
                Id = 1,
                Letter = "A",
                IpAddress = "123.123.123",
                DriveName = "Anonymous Drive",
                Password = "abc-123",
                Key = "NULL",
                Iv = "NULL",
                DateCreated = GetRandomDate(),
            },

            new()
            {
                Id = 2,
                Letter = "B",
                IpAddress = "234.234.234",
                DriveName = "Bricked Drive",
                Password = "abc-123",
                Key = "NULL",
                Iv = "NULL",
                DateCreated = GetRandomDate(),
            },

            new()
            {
                Id = 3,
                Letter = "C",
                IpAddress = "433.122.111",
                DriveName = "Comically Bad Drive",
                Password = "abc-123",
                Key = "NULL",
                Iv = "NULL",
                DateCreated = GetRandomDate(),
            },
            new()
            {
                Id = 4,
                Letter = "D",
                IpAddress = "999.999.999",
                DriveName = "Dirty Drive",
                Password = "abc-123",
                Key = "NULL",
                Iv = "NULL",
                DateCreated = GetRandomDate(),
            },
            new()
            {
                Id = 5,
                Letter = "E",
                IpAddress = "5555.5555.9959",
                DriveName = "Energy Drive",
                Password = "abc-123",
                Key = "NULL",
                Iv = "NULL",
                DateCreated = GetRandomDate(),
            },
            new()
            {
                Id = 6,
                Letter = "F",
                IpAddress = "222.666.0000",
                DriveName = "False Drive",
                Password = "abc-123",
                Key = "NULL",
                Iv = "NULL",
                DateCreated = GetRandomDate(),
            },
            new()
            {
                Id = 7,
                Letter = "G",
                IpAddress = "444.1313.865",
                DriveName = "Energy Drive",
                Password = "abc-123",
                Key = "NULL",
                Iv = "NULL",
                DateCreated = GetRandomDate(),
            },
            new()
            {
                Id = 8,
                Letter = "H",
                IpAddress = "940.1563.6942",
                DriveName = "Energy Drive",
                Password = "abc-123",
                Key = "NULL",
                Iv = "NULL",
                DateCreated = GetRandomDate(),
            },
        };
    }

    private static DateTime GetRandomDate()
    {
        var random = new Random();

        var start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;
        return start.AddDays(random.Next(range));
    }
}
