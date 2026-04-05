using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using ItemsMVCWebApp.Models;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _db;

    public ItemRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Item>> GetAllAsync()
    {
        return await _db.Items
                        .AsNoTracking()
                        .ToListAsync();
    }

    public async Task<Item> GetByIdAsync(int id)
    {
        return await _db.Items.FindAsync(id);
    }

    public async Task AddAsync(Item item)
    {
        _db.Items.Add(item);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Item item)
    {
        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _db.Items.FindAsync(id);
        if (item != null)
        {
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
        }
    }
}