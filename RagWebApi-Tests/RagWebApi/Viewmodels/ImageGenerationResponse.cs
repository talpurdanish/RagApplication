namespace RagWebApi.Viewmodels
{ 
    public class GeneratedImage
    {
        public Choice[]? Choices { get; set; }
    }

    public class Image
    {
        public string Type { get; set; } = string.Empty;
        public ImageUrl? ImageUrl { get; set; }
    }

    public class ImageUrl
    {
        public string Url { get; set; } = string.Empty;

    }

}
