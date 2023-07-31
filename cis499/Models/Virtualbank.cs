using System;
using System.Collections.Generic;

namespace cis499.Models;

public partial class Virtualbank
{
    public int Id { get; set; }

    public string? BankName { get; set; }

    public string? CardNumber { get; set; }

    public string? Balance { get; set; }

    public string? Cvv { get; set; }
}
