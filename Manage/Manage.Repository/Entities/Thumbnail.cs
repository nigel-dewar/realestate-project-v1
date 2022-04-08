namespace Manage.Repository.Entities
{
    public class Thumbnail
    {
        public string Id { get; set; }

        public string ThumbnailImageUrl { get; set; }

        public bool MainThumbnail { get; set; }

        public string PropertyId { get; set; }
    }
}
