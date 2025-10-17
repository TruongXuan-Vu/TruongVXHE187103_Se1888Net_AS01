using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Category
    {

        public short CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDesciption { get; set; }

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }

        public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

        public virtual Category? ParentCategory { get; set; }

    }
}
