using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
  public class PortfolioService
  {
    #region Property
    private readonly ApplicationDbContext _appDBContext;
    #endregion

    #region Constructor
    public PortfolioService(ApplicationDbContext appDBContext)
    {
      _appDBContext = appDBContext;
    }
    #endregion

    #region Get List of Portfolios
    public async Task<List<Portfolio>> GetAllPortfoliosAsync()
    {
      return await _appDBContext.Portfolios.ToListAsync();
    }
    #endregion

    #region Get List of Portfolios by UserId
    public async Task<List<Portfolio>> GetAllPortfoliosByUserAsync(string UserId)
    {
      return await _appDBContext.Portfolios
      .Where(p => p.UserId.Equals(UserId))
      .Include(p => p.Cryptocoin)
      .Include(p => p.Exchange)
      .ToListAsync();
    }
    #endregion

    #region Insert Portfolio
    public async Task<bool> InsertPortfolioAsync(Portfolio portfolio)
    {
      await _appDBContext.Portfolios.AddAsync(portfolio);
      await _appDBContext.SaveChangesAsync();
      return true;
    }
    #endregion

    #region Get Portfolio by Id
    public async Task<Portfolio> GetPortfolioAsync(int Id)
    {
      Portfolio portfolio = await _appDBContext.Portfolios.FirstOrDefaultAsync(c => c.Id.Equals(Id));
      return portfolio;
    }
    #endregion

    #region Update Portfolio
    public async Task<bool> UpdatePortfolioAsync(Portfolio portfolio)
    {
      _appDBContext.Portfolios.Update(portfolio);
      await _appDBContext.SaveChangesAsync();
      return true;
    }
    #endregion

    #region DeletePortfolio
    public async Task<bool> DeletePortfolioAsync(Portfolio portfolio)
    {
      _appDBContext.Remove(portfolio);
      await _appDBContext.SaveChangesAsync();
      return true;
    }
    #endregion
  }
}