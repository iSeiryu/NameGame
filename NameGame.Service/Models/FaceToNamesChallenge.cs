namespace NameGame.Service.Models
{
    public class FaceToNamesChallenge
    {
        public FaceToNamesChallenge(int challengeId, string description, Employee[] employees, Face face)
        {
            ChallengeId = challengeId;
            Description = description;
            Employees = employees;
            Face = face;
        }

        public int ChallengeId { get; }
        public string Description { get; }
        public Employee[] Employees { get; }
        public Face Face { get; }
    }
}
