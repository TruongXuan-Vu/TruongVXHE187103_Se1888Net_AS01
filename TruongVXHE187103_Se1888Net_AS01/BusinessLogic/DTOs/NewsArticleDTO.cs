using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class NewsArticleDTO
    {
        public string NewsArticleId { get; set; } = null!;

        public string? NewsTitle { get; set; }

        public string? Headline { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public short? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }
        //thêm trường CreatedByName và UpdatedByName
        public string? CreatedByName { get; set; }
        public string UpdatedByName { get; set; } = null!;

        public short? UpdatedById { get; set; }


    }
}
