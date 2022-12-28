using System;
using System.Collections.Generic;

namespace DigitalBooksWebAPI.Models;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
