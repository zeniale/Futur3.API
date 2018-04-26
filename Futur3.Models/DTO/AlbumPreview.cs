﻿namespace Futur3.Models.DTO
{
    public class AlbumPreview
    {
        public int AlbumId { get; set; }

        public string Name { get; set; }
        public string WebSite { get; set; }
        public string City { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int PhotoCount { get; set; }
        public string RandomThumbnailUrl { get; set; }
    }
}
