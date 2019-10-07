namespace NameGame.Service.Models
{
    public class Challenge
    {
        public Challenge(int challengeId, string description, Employee employee, Face[] faces)
        {
            ChallengeId = challengeId;
            Description = description;
            Name = employee;
            Faces = faces;
        }

        public int ChallengeId { get; }
        public string Description { get; }
        public Employee Name { get; }
        public Face[] Faces { get; }
    }
}
