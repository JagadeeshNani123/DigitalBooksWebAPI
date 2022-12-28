using System;
using System.Collections.Generic;

namespace DigitalBooksWebAPI.Models;

public partial class Book
{
    public Guid BookId { get; set; }

    public string BookName { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public decimal Price { get; set; }

    public Guid PublisherId { get; set; }

    public string PublishedDate { get; set; } = null!;

    public string BookContent { get; set; } = null!;

    public bool Active { get; set; }

    public Guid UserId { get; set; }



    public virtual ICollection<Subscription> Subscriptions { get; } = new List<Subscription>();

}
