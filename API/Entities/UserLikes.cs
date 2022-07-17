namespace API.Entities
{
    public class UserLikes
    {
        public AppUser SourceUser { get; set; }
        public AppUser LikedUser { get; set; }
        
        public int SourceUserId { set; get; }
        public int LikedUserId { get; set; }
    }
}