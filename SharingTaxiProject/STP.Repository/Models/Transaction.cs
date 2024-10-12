using System;
using System.Collections.Generic;

namespace STP.Repository.Models;

public partial class Transaction
{
    public int Id { get; set; }
    // Khóa ngoại liên kết với bảng Deposit
    public int? DepositId { get; set; }
    public int? WalletId { get; set; }

    public decimal? Amount { get; set; }

    public string? TransactionType { get; set; }

    public string? ReferenceId { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    // Thuộc tính điều hướng đến Deposit
    public virtual Deposit? Deposit { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
