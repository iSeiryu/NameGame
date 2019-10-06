namespace NameGame.Domain.Models
{
    public class Employee
    {
        public Employee(string id, string firstName, string lastName)
        {
            Id = id;
            Name = firstName + " " + lastName;
        }

        public string Id { get; }
        public string Name { get; }
    }
}
