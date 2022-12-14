using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser: IdentityUser<int>
    {
        public string KnownAs { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        
        public ICollection<Photo> Photos { get; set; }
        
        public ICollection<UserLikes> LikedByUsers { get; set; }
        public ICollection<UserLikes> LikedUsers { get; set; }
        
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}