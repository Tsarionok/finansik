var _lock = new object();

var common = 1;

var iterator = new ConcurrentIterator();

var threadOne = new Thread(Print);
threadOne.Name = "ONE";
var threadTwo = new Thread(Print);
threadTwo.Name = "TWO";
Thread.Sleep(300);

threadOne.Start();
threadTwo.Start();

while (threadOne.IsAlive && threadTwo.IsAlive) { }

Console.WriteLine($"Total executions: {iterator.Value}");

void Print()
{
    while (common <= 200)
    {
        var isLocked = false;
        try
        {
            Monitor.Enter(_lock, ref isLocked);
            for (var i = 0; i <= 5; i++)
            {
                if (common > 200)
                    break;

                if (common == 42)
                    throw new Exception();
        
                Console.WriteLine($"[{Thread.CurrentThread.Name}] {common++}");
                iterator.Increment();
            }
        }
        finally
        {
            if (isLocked)
            {
                Monitor.Exit(_lock);
                Console.WriteLine($"[{Thread.CurrentThread.Name}] if finally");
            }
            else
            {
                Console.WriteLine($"[{Thread.CurrentThread.Name}] else finally");
            }
        }
            
        Thread.Sleep(10);
        Console.WriteLine($"[{Thread.CurrentThread.Name}] continue");
    }
}


public class ConcurrentIterator
{
    private readonly object _lock = new();

    public int Value { get; private set; }

    public void Increment()
    {
        lock (_lock)
        {
            ++Value;
        }
    }
}
