﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

public partial class Transaction
{
    public int Id { get; set; }

<<<<<<< Updated upstream
=======
    public int? DepositId { get; set; }

>>>>>>> Stashed changes
    public int? WalletId { get; set; }

    public decimal? Amount { get; set; }

    public string TransactionType { get; set; }

    public string ReferenceId { get; set; }

    public DateTime? CreatedAt { get; set; }

<<<<<<< Updated upstream
    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
=======
    public int? Status { get; set; }
>>>>>>> Stashed changes

    public virtual Deposit Deposit { get; set; }

    public virtual Wallet Wallet { get; set; }
}