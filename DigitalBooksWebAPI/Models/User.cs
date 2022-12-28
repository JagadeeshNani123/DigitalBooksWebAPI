using System;
using System.Collections.Generic;

namespace DigitalBooksWebAPI.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid RoleId { get; set; }

    public virtual ICollection<Book> Books { get; } = new List<Book>();


    public virtual ICollection<Subscription> Subscriptions { get; } = new List<Subscription>();
}
