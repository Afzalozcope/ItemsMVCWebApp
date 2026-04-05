using System.Threading.Tasks;
using System.Web.Http;
using ItemsMVCWebApp.Models;

public class ItemApiController : ApiController
{
    private readonly IItemRepository _repo;

    public ItemApiController(IItemRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [Route("api/items")]
    public async Task<IHttpActionResult> GetItems()
    {
        var items = await _repo.GetAllAsync();
        return Ok(items);
    }

    [HttpGet]
    [Route("api/items/{id}")]
    public async Task<IHttpActionResult> GetItem(int id)
    {
        var item = await _repo.GetByIdAsync(id);

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

        await _repo.AddAsync(item);
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
        var existing = await _repo.GetByIdAsync(id);

        if (existing == null)
        {
            return NotFound();
        }
        UpdateItem(item, existing);
        await _repo.UpdateAsync(existing);
        return Ok(existing);
    }

    [HttpDelete]
    [Route("api/items/{id}")]
    public async Task<IHttpActionResult> DeleteItem(int id)
    {
        var existing = await _repo.GetByIdAsync(id);

        if (existing == null)
        {
            return NotFound();
        }
        await _repo.DeleteAsync(id);
        return Ok();
    }

    private void UpdateItem(Item source, Item target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
    }
}