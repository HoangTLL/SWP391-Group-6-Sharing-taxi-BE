using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

public partial class Wallet
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public decimal? Balance { get; set; }

    public string? CurrencyCode { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
