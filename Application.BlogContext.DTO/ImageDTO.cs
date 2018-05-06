namespace NLayerApp.Application.BlogBoundedContext.DTO
{
    using NLayerApp.Domain.Seedwork;
    using Newtonsoft.Json;
    public class ImageDTO : Entity
    {
        public int ImageId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        [JsonIgnore]
        public int PostId { get; set; }
        [JsonIgnore]
        public PostDTO Post { get; set; }
    }
}
