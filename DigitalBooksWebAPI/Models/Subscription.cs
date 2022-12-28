using System;
using System.Collections.Generic;

namespace DigitalBooksWebAPI.Models;

public partial class Subscription
{
    public Guid SubscriptionId { get; set; }

    public Guid BookId { get; set; }

    public Guid UserId { get; set; }

    public string SubscriptionDate { get; set; } = null!;
}
