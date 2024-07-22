Person.Execute();

internal record Person
{
    public string Name { get; set; }

    public static void Execute()
    {
        Person person = new Stuff("ST", 334);

        Type type = person.GetType();

        type.Assembly.GetType();

        Console.WriteLine(type.BaseType.Name);
    }
}

internal record Stuff : Person
{
    public Stuff(string name, int salary)
    {
        Name = name;
        Salary = salary;
    }
    
    public int Salary { get; set; }
}

internal class Car
{
    public string Model { get; set; }
}