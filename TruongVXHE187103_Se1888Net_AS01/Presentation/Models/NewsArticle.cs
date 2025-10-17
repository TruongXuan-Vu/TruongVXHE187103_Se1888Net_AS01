using System;
using System.Collections.Generic;

namespace Presentation.Models;

public partial class NewsArticle
{
    public string Id { get; set; } = null!;

    public string? Title { get; set; }

    public string Headline { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }
    
    public string? Content { get; set; }

    public string? Source { get; set; }

    public short? CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public bool? Status { get; set; }

    public short? CreatedById { get; set; }
    //thêm trường CreatedByName và UpdatedByName
    public string? CreatedByName { get; set; }
    public string UpdatedByName { get; set; } = null!;

    public short? UpdatedById { get; set; }
}
