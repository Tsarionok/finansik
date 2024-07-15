var ok = ThreadPool.SetMinThreads(40, 20);

var task1 = Task.Run(async () =>
{
    await Task.Delay(8000);
    Console.WriteLine("FIRST RUN");
});

var task2 = Task.Run(async () =>
{
    await Task.Delay(5000);
    Console.WriteLine("SECOND RUN");
});


Thread.CurrentThread.Name = "FinansikThread";

Console.WriteLine(Thread.CurrentThread.Name);

Task.WaitAll(task1, task2);




