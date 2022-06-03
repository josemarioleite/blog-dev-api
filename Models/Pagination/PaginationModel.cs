using System.Collections.Generic;

namespace blog_api_dev.Models.Pagination
{
    public class PaginationModel
    {
        public int rowCount;
        public IEnumerable<Models.Article.Article> article;
    }
}