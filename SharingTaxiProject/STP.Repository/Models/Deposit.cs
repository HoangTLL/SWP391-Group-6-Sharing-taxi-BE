using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

public partial class Deposit
{
    public int Id { get; set; }

    public int? WalletId { get; set; }
    public int UserId { get; set; }

    public int? TransactionId { get; set; }

    public decimal? Amount { get; set; }

    public string? DepositMethod { get; set; }

    public DateTime? DepositDate { get; set; }

    public int? Status { get; set; }

    public virtual Transaction? Transaction { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
