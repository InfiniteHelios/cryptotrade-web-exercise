using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
  public class Portfolio
  {
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(ApplicationUser))]
    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    [Required]
    [ForeignKey(nameof(Cryptocoin))]
    public int CryptocoinId { get; set; }
    public virtual Cryptocoin Cryptocoin { get; set; }

    [Required]
    public double Amount { get; set; }

    [Required]
    [ForeignKey(nameof(Exchange))]
    public int ExchangeId { get; set; }
    public virtual Exchange Exchange { get; set; }

    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Portfolio()
    {
      Active = true;
      CreatedAt = DateTime.UtcNow;
      UpdatedAt = DateTime.UtcNow;
    }
  }
}