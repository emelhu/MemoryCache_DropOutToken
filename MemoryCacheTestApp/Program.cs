using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Install-Package Microsoft.Extensions.Options
// Install-Package Microsoft.Extensions.Caching.Abstractions

namespace MemoryCacheTestApp
{
  using System.Diagnostics;
  using System.Threading;
  using Microsoft.Extensions.Caching.Memory;

  class Program
  {
    private static IMemoryCache memoryCache;

    private static DropOutToken token1 = new DropOutToken();
    private static DropOutToken token2 = new DropOutToken();

    private static Stopwatch    stopwatch;

    static void Main(string[] args)
    {
      Console.WriteLine();
      Console.BackgroundColor = ConsoleColor.DarkBlue;
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("Microsoft.Extensions.Caching MemoryCache & DropOutToken test. * by eMeL (www.emel.hu)");
      Console.WriteLine();
      Console.WriteLine();
      Console.ResetColor();

      var testItems = GetTestList();

      MemoryCacheOptions.DefaultExpirationScanFrequency = TimeSpan.FromMilliseconds(500);

      memoryCache   = new MemoryCache(new MemoryCacheOptions());

      var cts = new CancellationTokenSource(10 * 1000);

      var task = Task.Run(() =>
            {
              while (!cts.IsCancellationRequested)
              {
                memoryCache.Set<string>(Guid.NewGuid(), String.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 100))); 
                Thread.Sleep(10);
              }
            });

      stopwatch = Stopwatch.StartNew();

      StoreCacheItems(testItems);
      //Thread.Sleep(500);

      for (int times = 0; times < 10; times++)
      {
        if ((stopwatch.ElapsedMilliseconds > 5000) && !token1.DropOutRequest)
        {
          token1.DropOut();
          Console.WriteLine("token1.DropOut()");
          Console.WriteLine();
        }

        if ((stopwatch.ElapsedMilliseconds > 7000) && !token2.DropOutRequest)
        {
          token2.DropOut();
          Console.WriteLine("token2.DropOut()");
          Console.WriteLine();
        }

        PrintCacheItems(testItems);

        Thread.Sleep(1000);
      }

      cts.Cancel();

      task.Wait();

      Console.WriteLine();
      Console.Write("Press Enter to exit...");
      Console.ReadLine();
    }

    
    public static List<CacheTestItem> GetTestList()
    {
      var list = new List<CacheTestItem>();

      list.Add(new CacheTestItem("TEST_D2000",         2000));
      list.Add(new CacheTestItem("TEST_D4000",         4000));
      list.Add(new CacheTestItem("TEST_D4000_Token1",  4000, token1));
      list.Add(new CacheTestItem("TEST_D6000_Token1",  6000, token1));
      list.Add(new CacheTestItem("TEST_D6000_Token2",  6000, token2));
      list.Add(new CacheTestItem("TEST_D8000_Token2",  8000, token2));

      return list;
    }

    public static void StoreCacheItems(List<CacheTestItem> testItems)
    {
      foreach (var item in testItems)
      {
        memoryCache.Set<string>(item.key, item.data, item.delay, item.token);
      }      
    }

    private static void PrintCacheItems(List<CacheTestItem> testItems)
    {
      Console.WriteLine("Elapsed milliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

      foreach (var item in testItems)
      {
        Console.WriteLine("{0,-17}: {1}", item.key, memoryCache.Get<string>(item.key));
      }   
      
      Console.WriteLine(); 
    }

  }

  public struct CacheTestItem
  {
    public string       key;
    public string       data;
    public TimeSpan     delay;
    public DropOutToken token;

    public CacheTestItem(string key, int delayMilliSec, DropOutToken token = null)
    {
      this.key    = key;
      this.data   = "Test data:" + Guid.NewGuid().ToString() + " [" + key + "]";
      this.delay  = TimeSpan.FromMilliseconds(delayMilliSec);
      this.token  = token;
    } 
  }
}
