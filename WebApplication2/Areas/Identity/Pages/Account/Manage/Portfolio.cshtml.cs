using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Areas.Identity.Pages.Account.Manage
{
  public class PortfolioModel : PageModel
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly PortfolioService _portfolioService;
    private readonly CryptocoinService _cryptocoinService;
    private readonly ExchangeService _exchangeService;

    public IList<Portfolio> Portfolios { get; set; }
    public IList<CryptocoinOption> CryptocoinOptions { get; set; }
    public IList<ExchangeOption> ExchangeOptions { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    public PortfolioModel(
        UserManager<ApplicationUser> userManager,
        PortfolioService portfolioService,
        CryptocoinService cryptocoinService,
        ExchangeService exchangeService)
    {
      _userManager = userManager;
      _portfolioService = portfolioService;
      _cryptocoinService = cryptocoinService;
      _exchangeService = exchangeService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID 'user.Id'.");
      }

      Portfolios = await _portfolioService.GetAllPortfoliosByUserAsync(user.Id);
      CryptocoinOptions = await _cryptocoinService.GetAllCryptocoinSymbolsAsync();
      ExchangeOptions = await _exchangeService.GetAllExchangeSymbolsAsync();

      return Page();
    }

    public async Task<IActionResult> OnPostSavePortfolioAsync()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      int id = Convert.ToInt32(Request.Form["portfolio-id"]);
      if (id == -1) await CreatePortfolioAsync(user.Id);
      else await UpdatePortfolioAsync(id);

      return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeletePortfolioAsync()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }
      int id = Convert.ToInt32(Request.Form["delete-id"]);
      Portfolio portfolio = await _portfolioService.GetPortfolioAsync(id);
      var ret = await _portfolioService.DeletePortfolioAsync(portfolio);
      if (!ret)
      {
        StatusMessage = "Delete portfolio error";
      }

      return RedirectToPage();
    }

    private async Task CreatePortfolioAsync(string userId)
    {
      Portfolio portfolio = new Portfolio();
      portfolio.UserId = userId;
      portfolio.CryptocoinId = Convert.ToInt32(Request.Form["cryptocoin"]);
      portfolio.Amount = Convert.ToDouble(Request.Form["amount"]);
      portfolio.ExchangeId = Convert.ToInt32(Request.Form["exchange"]);
      portfolio.Active = Request.Form["active"].Equals("on");
      var ret = await _portfolioService.InsertPortfolioAsync(portfolio);
      if (!ret)
      {
        StatusMessage = "Create portfolio error";
      }
    }

    private async Task UpdatePortfolioAsync(int id)
    {
      Portfolio portfolio = await _portfolioService.GetPortfolioAsync(id);
      portfolio.CryptocoinId = Convert.ToInt32(Request.Form["cryptocoin"]);
      portfolio.Amount = Convert.ToDouble(Request.Form["amount"]);
      portfolio.ExchangeId = Convert.ToInt32(Request.Form["exchange"]);
      portfolio.Active = Request.Form["active"].Equals("on");
      var ret = await _portfolioService.UpdatePortfolioAsync(portfolio);
      if (!ret)
      {
        StatusMessage = "Update portfolio error";
      }
    }
  }
}