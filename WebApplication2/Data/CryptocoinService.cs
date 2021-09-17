using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
  public class CryptocoinOption
  {
    public int Id { get; set; }
    public string Symbol { get; set; }
  }

  public class CryptocoinService
  {
    #region Property
    private readonly ApplicationDbContext _appDBContext;
    #endregion

    #region Constructor
    public CryptocoinService(ApplicationDbContext appDBContext)
    {
      _appDBContext = appDBContext;
    }
    #endregion

    #region Get List of Cryptocoins
    public async Task<List<Cryptocoin>> GetAllCryptocoinsAsync()
    {
      return await _appDBContext.Cryptocoins.ToListAsync();
    }
    #endregion

    #region Get List of Cryptocoins Symbol
    public async Task<List<CryptocoinOption>> GetAllCryptocoinSymbolsAsync()
    {
      return await _appDBContext.Cryptocoins.Select(c => new CryptocoinOption()
      {
        Id = c.Id,
        Symbol = c.Symbol
      }).ToListAsync();
    }
    #endregion

    #region Insert Cryptocoin
    public async Task<bool> InsertCryptocoinAsync(Cryptocoin cryptocoin)
    {
      await _appDBContext.Cryptocoins.AddAsync(cryptocoin);
      await _appDBContext.SaveChangesAsync();
      return true;
    }
    #endregion

    #region Get Cryptocoin by Id
    public async Task<Cryptocoin> GetCryptocoinAsync(int Id)
    {
      Cryptocoin cryptocoin = await _appDBContext.Cryptocoins.FirstOrDefaultAsync(c => c.Id.Equals(Id));
      return cryptocoin;
    }
    #endregion

    #region Update Cryptocoin
    public async Task<bool> UpdateCryptocoinAsync(Cryptocoin cryptocoin)
    {
      _appDBContext.Cryptocoins.Update(cryptocoin);
      await _appDBContext.SaveChangesAsync();
      return true;
    }
    #endregion

    #region DeleteCryptocoin
    public async Task<bool> DeleteCryptocoinAsync(Cryptocoin cryptocoin)
    {
      _appDBContext.Remove(cryptocoin);
      await _appDBContext.SaveChangesAsync();
      return true;
    }
    #endregion
  }
}