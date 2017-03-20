using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
  public class Exchange
  {
    public int Id { get; set; }

    [MaxLength(255)]
    public string Symbol { get; set; }

    [MaxLength(255)]
    public string Name { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Portfolio> Portfolios { get; set; }

    public Exchange()
    {
      Active = true;
      CreatedAt = DateTime.UtcNow;
      UpdatedAt = DateTime.UtcNow;
      Portfolios = new HashSet<Portfolio>();
    }
  }
}