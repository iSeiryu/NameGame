namespace NameGame.Service.Models
{
    public class NameToFacesChallenge
    {
        public NameToFacesChallenge(int challengeId, string description, Employee employee, Face[] faces)
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
