using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using ItemsMVCWebApp.Models;

public class ItemsApiController : ApiController
{
    private AppDbContext db = new AppDbContext();

    [HttpGet]
    [Route("api/items")]
    public async Task<IHttpActionResult> GetItems()
    {
        var items = await db.Items
                            .AsNoTracking()
                            .ToListAsync();

        return Ok(items);
    }

    [HttpGet]
    [Route("api/items/{id}")]
    public async Task<IHttpActionResult> GetItem(int id)
    {
        var item = await db.Items.FindAsync(id);

        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    [Route("api/items")]
    public async Task<IHttpActionResult> PostItem(Item item)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        db.Items.Add(item);
        await db.SaveChangesAsync();

        return Ok(item);
    }

    [HttpPut]
    [Route("api/items/{id}")]
    public async Task<IHttpActionResult> PutItem(int id, Item item)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existing = await db.Items.FindAsync(id);

        if (existing == null)
        {
            return NotFound();
        }

        UpdateItem(item, existing);
        await db.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete]
    [Route("api/items/{id}")]
    public async Task<IHttpActionResult> DeleteItem(int id)
    {
        var item = await db.Items.FindAsync(id);

        if (item == null) 
        {
            return NotFound();
        }

        db.Items.Remove(item);
        await db.SaveChangesAsync();
        return Ok();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            db.Dispose();
        }
        base.Dispose(disposing);
    }
    private void UpdateItem(Item source, Item target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
    }
}