FuncManager.Execute();

await Task.Delay(1000);


public class FuncManager
{
    public static void Execute()
    {
        try
        {
            var text = "Hello";
            Console.WriteLine("Start");
            Func(async () =>
            {
                Console.WriteLine(text);
                throw new NotImplementedException();
            });
            Console.WriteLine("End");
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception handled");
        }
    }
    
    public static void Func(Action action)
    {
        action();
    }
}