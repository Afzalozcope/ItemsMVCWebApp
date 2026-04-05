using System.Threading.Tasks;
using System.Web.Mvc;
using ItemsMVCWebApp.Models;

public class ItemController : Controller
{
    private readonly IItemRepository _repo;

    public ItemController(IItemRepository repo)
    {
        _repo = repo;
    }

    public async Task<ActionResult> Index()
    {
        var items = await _repo.GetAllAsync();
        return View(items);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> AddItemAsync(Item item)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false });
        }

        await _repo.AddAsync(item);

        return Json(new { success = true });
    }

    public async Task<ActionResult> Edit(int id)
    {
        var item = await _repo.GetByIdAsync(id);

        if (item == null)
        {
            return HttpNotFound();
        }

        return View(item);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Item item)
    {
        if (!ModelState.IsValid)
        {
            return View(item);
        }
        await _repo.UpdateAsync(item);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}