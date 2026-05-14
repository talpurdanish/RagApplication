using Microsoft.AspNetCore.Mvc;

namespace RagWebApi.Models
{
    public class OutputResults
    {
        public static JsonResult Success(object? results, int statusCode = 200)
        {
            return new JsonResult(new
            {
                error = false,
                message = "",
                code = statusCode,
                results
            })
            {
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }

        public static JsonResult Success(string message, object? results, int statusCode = 200)
        {
            return new JsonResult(new
            {
                error = false,
                message,
                code = statusCode,
                results
            })
            {
                StatusCode = statusCode,
                ContentType = "application/json"
            };

        }

        public static JsonResult Success(string message, int statusCode)
        {
            return new JsonResult(new
            {
                error = false,
                message,
                code = statusCode,
                results = ""
            })
            {
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }

        public static JsonResult Error(string message, int statusCode = 400)
        {
            IList<int> codes = [200, 201, 400, 401, 404, 403, 422, 500];
            var findCode = codes.Any((code) => code == statusCode);
            if (!findCode) statusCode = 500;

            return new JsonResult(new
            {
                message,
                error = true,
                statusCode,
            })
            {
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }
    }
}
