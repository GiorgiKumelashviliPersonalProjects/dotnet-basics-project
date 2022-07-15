// ReSharper disable ClassNeverInstantiated.Global

namespace API.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public int PublicId { get; set; }

        // fully defining relationship
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}