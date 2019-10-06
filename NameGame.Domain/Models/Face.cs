namespace NameGame.Domain.Models
{
    public class Face
    {
        public Face(string id, string imageUrl)
        {
            Id = id;
            ImageUrl = imageUrl;
        }

        public string Id { get; }
        public string ImageUrl { get; }
    }
}
