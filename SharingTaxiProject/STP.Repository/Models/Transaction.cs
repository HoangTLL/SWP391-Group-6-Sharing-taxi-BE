using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int? WalletId { get; set; }

    public decimal? Amount { get; set; }

    public string? TransactionType { get; set; }

    public string? ReferenceId { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual Wallet? Wallet { get; set; }
}
