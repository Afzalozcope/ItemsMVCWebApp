using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using ItemsMVCWebApp.Models;

public class ItemController : Controller
{
    private AppDbContext db = new AppDbContext();

    public async Task<ActionResult> Index()
    {
        var items = await db.Items
                            .AsNoTracking()
                            .ToListAsync();

        return View(items);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> AddItemAsync(Item item)
    {
        if (ModelState.IsValid)
        {
            db.Items.Add(item);
            await db.SaveChangesAsync();
        }

        return Json(new { success = true });
    }

    public async Task<ActionResult> Edit(int id)
    {
        var item = await db.Items.FindAsync(id);

        if (item == null)
        {
            return HttpNotFound();
        }

        return View(item);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Item item)
    {
        db.Entry(item).State = EntityState.Modified;
        await db.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> Delete(int id)
    {
        var item = await db.Items.FindAsync(id);

        if (item != null)
        {
            db.Items.Remove(item);
            await db.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            db.Dispose();
        }
        base.Dispose(disposing);
    }
}