using System.Text.Json;
using API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(
            this HttpResponse response,
            int currentPage,
            int itemsPerPage,
            int totalItems,
            int totalPages
        )
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            const string exposedPaginationHeaderName = "X-Pagination";


            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add(exposedPaginationHeaderName, JsonSerializer.Serialize(paginationHeader, options));
            response.Headers.Add(HeaderNames.AccessControlExposeHeaders, exposedPaginationHeaderName);
        }
    }
}