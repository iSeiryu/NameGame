namespace NameGame.Service.Models
{
    public class Challenge
    {
        public Challenge(string description, Employee employee, Face[] faces)
        {
            Description = description;
            Name = employee;
            Faces = faces;
        }

        public string Description { get; }
        public Employee Name { get; }
        public Face[] Faces { get; }
    }
}
