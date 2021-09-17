using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class ApplicationUser : IdentityUser
{
  [MaxLength(255)]
  public string Name { get; set; }
}