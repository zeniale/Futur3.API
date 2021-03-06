﻿namespace Futur3.Models
{
    public class ApplicationSettings
    {
        public MongoDbSettings MongoDbSettings { get; set; }
        public RemoteDataServiceSettings RemoteDataServiceSettings { get; set; }
    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public string AlbumCollection { get; set; }
        public string UserCollection { get; set; }
        public string PhotoCollection { get; set; }

        public string AlbumPreviewCollection { get; set; }
    }

    public class RemoteDataServiceSettings
    {
        public string AlbumsUrl { get; set; }
        public string UsersUrl { get; set; }
        public string PhotosUrl { get; set; }
    }
}
