using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Xml.Serialization;

namespace RagWebApi.Models
{
    public class DataDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength]
        public string Text { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int NumberOfPages { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; } = string.Empty;

        [MaxLength]
        public string? DescriptionEmbedding { get; set; } = string.Empty;

        [NotMapped]
        public float[] Embedding
        {
            get => JsonSerializer.Deserialize<float[]>(DescriptionEmbedding ?? "") ?? [];
            set => DescriptionEmbedding = JsonSerializer.Serialize(value);
        }
    }

    public class DataDocumentForBulk
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int NumberOfPages { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; } = string.Empty;

        public override string ToString()
        {
            return
                $"Text: {Text} " +
                $"Author: {Author} " +
                $"NumberOfPages: {NumberOfPages} " +
                $"CreationDate: {CreationDate} " +
                $"Title: {Title} ";
         }

    }


    public class DataDocumentViewModel
    {
        public string Text { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int NumberOfPages { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Similarity { get; set; }

        public override string ToString()
        {
            return
                $"Text: {Text} " +
                $"Author: {Author} " +
                $"NumberOfPages: {NumberOfPages} " +
                $"CreationDate: {CreationDate} " +
                $"Title: {Title} "  +
                $"Similarity: {Similarity}";
        }

    }

    [XmlRoot("ArrayOfDataDocuments", Namespace = "")]
    public class ArrayOfDataDocuments
    {
        [XmlElement("DataDocument")]
        public List<DataDocument> Items { get; set; } = [];
    }

}

