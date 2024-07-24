var person = new Person();

var type = person.GetType();
var typeOfType = type.GetType();
var typeOfTypeOfType = typeOfType.GetType();

Console.WriteLine(type);
Console.WriteLine(typeOfType);
Console.WriteLine(typeOfTypeOfType);



struct Person
{
    public string Name { get; set; }
}