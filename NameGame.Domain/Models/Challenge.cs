namespace NameGame.Domain.Models
{
    public class Challenge
    {
        public Challenge(string description, string name, string[] faces)
        {
            Description = description;
            Name = name;
            Faces = faces;
        }

        public string Description { get; }
        public string Name { get; }
        public string[] Faces { get; }
    }
}
