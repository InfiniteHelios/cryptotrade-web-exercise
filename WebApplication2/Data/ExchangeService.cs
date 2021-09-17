using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
  public class ExchangeOption
  {
    public int Id { get; set; }
    public string Symbol { get; set; }
  }

  public class ExchangeService
  {
    #region Property
    private readonly ApplicationDbContext _appDBContext;
    #endregion

    #region Constructor
    public ExchangeService(ApplicationDbContext appDBContext)
    {
      _appDBContext = appDBContext;
    }
    #endregion

    #region Get List of Exchanges
    public async Task<List<Exchange>> GetAllExchangesAsync()
    {
      return await _appDBContext.Exchanges.ToListAsync();
    }
    #endregion

    #region Get List of Exchanges Symbol
    public async Task<List<ExchangeOption>> GetAllExchangeSymbolsAsync()
    {
      return await _appDBContext.Exchanges.Select(c => new ExchangeOption()
      {
        Id = c.Id,
        Symbol = c.Symbol
      }).ToListAsync();
    }
    #endregion

    #region Insert Exchange
    public async Task<bool> InsertExchangeAsync(Exchange exchange)
    {
      await _appDBContext.Exchanges.AddAsync(exchange);
      await _appDBContext.SaveChangesAsync();
      return true;
    }
    #endregion

    #region Get Exchange by Id
    public async Task<Exchange> GetExchangeAsync(int Id)
    {
      Exchange exchange = await _appDBContext.Exchanges.FirstOrDefaultAsync(c => c.Id.Equals(Id));
      return exchange;
    }
    #endregion

    #region Update Exchange
    public async Task<bool> UpdateExchangeAsync(Exchange exchange)
    {
      _appDBContext.Exchanges.Update(exchange);
      await _appDBContext.SaveChangesAsync();
      return true;
    }
    #endregion

    #region DeleteExchange
    public async Task<bool> DeleteExchangeAsync(Exchange exchange)
    {
      _appDBContext.Remove(exchange);
      await _appDBContext.SaveChangesAsync();
      return true;
    }
    #endregion
  }
}