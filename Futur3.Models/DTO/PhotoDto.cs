using System;
using System.Collections.Generic;
using System.Text;

namespace Futur3.Models.DTO
{
    public class PhotoDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public int AlbumId { get; set; }
        public int Likes { get; set; }
    }
}
