using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RagWebApi.DataContext;
using RagWebApi.Models;
using System.Xml.Serialization;

namespace RagWebApi.Service.Documents
{

    public interface IDocumentService
    {
        Task<List<DataDocumentViewModel>> GetAll();

        Task<bool> BulkInsert(IFormFile file);
        Task<string> SearchAsync(string query, List<string> previousUserPrompts);
        Task<bool> UpdateEmbeddings(List<DocumentEmbeddingsResult> results);
        void StartAnalysis();
    }
    public record DocumentEmbeddingsResult(int DocumentId, float[] Embeddings);


    public class DocumentService(RagContext context, IDocumentAiService aiService, WorkflowNotifier notifier) : IDocumentService
    {

        public void StartAnalysis()
        {
            notifier.TriggerImageAiAnalysis();
        }

        public async Task<bool> BulkInsert(IFormFile file)
        {

            if (file == null || file.Length == 0)
                return false;

            // Inside your UploadCsv method:
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var list = ExtractXml(reader).Items;

            await context.DataDocuments.AddRangeAsync(list);
            var rowsSaved = await context.SaveChangesAsync();
            return rowsSaved > 0;

        }

        private static ArrayOfDataDocuments ExtractXml(StreamReader reader)
        {
            try
            {
                XmlSerializer serializer = new(typeof(ArrayOfDataDocuments));
                var records = serializer.Deserialize(reader);
                return records is null ? new ArrayOfDataDocuments() : (ArrayOfDataDocuments)records;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> SearchAsync(string query, List<string> previousUserPrompts)
        {
            var documents = await context.DataDocuments.ToListAsync();
            return await aiService.SearchAsync(query, documents, previousUserPrompts);
        }

        public async Task<bool> UpdateEmbeddings(List<DocumentEmbeddingsResult> results)
        {
            var ids = results.Select(r => r.DocumentId).ToList();

            var documents = await context.DataDocuments.Where(m => ids.Contains(m.Id)).ToListAsync();

            foreach (var document in documents)
            {
                var result = results.FirstOrDefault(r => r.DocumentId == document.Id);
                if (result != null)
                {

                    document.DescriptionEmbedding = JsonConvert.SerializeObject(result.Embeddings);
                    context.Entry<DataDocument>(document).State = EntityState.Modified;
                }
            }

            context.DataDocuments.UpdateRange(documents);
            var rows = await context.SaveChangesAsync();
            return rows == documents.Count;
        }

        public async Task<List<DataDocumentViewModel>> GetAll()
        {
            var documents = await context.DataDocuments.Select(d => new DataDocumentViewModel()
            {
                Title = d.Title,
                Author = d.Author,
                NumberOfPages = d.NumberOfPages,
                CreationDate = d.CreationDate,
                Text = d.Text
            }).ToListAsync();

            return documents;
        }
    }

}
