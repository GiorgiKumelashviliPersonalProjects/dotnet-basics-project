using System;
using System.Collections.Generic;

namespace API.DTO
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string KnownAs { get; set; }
        public int Age { get; set; } // new
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public string PhotoUrl { get; set; }
        
        public ICollection<PhotoDto> Photos { get; set; }
    }
}