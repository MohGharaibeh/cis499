using System;
using System.Collections.Generic;

namespace cis499.Models;

public partial class Testimonial
{
    public string? State { get; set; }

    public string? TestMessage { get; set; }

    public int Id { get; set; }

    public int? UserAccountId { get; set; }

    public DateTime? AddedDate { get; set; }

    public virtual Useraccount? UserAccount { get; set; }
}
