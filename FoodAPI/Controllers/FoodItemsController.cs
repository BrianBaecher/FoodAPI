using FoodAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodAPI.Controllers
{
	[Route("api/FoodItems")]
	[ApiController]
	public class FoodItemsController : ControllerBase
	{
		private readonly FoodItemContext _context;

		public FoodItemsController(FoodItemContext context)
		{
			_context = context;
		}

		// GET: api/FoodItems
		[HttpGet]
		public async Task<ActionResult<IEnumerable<FoodItem>>> GetFoodItems()
		{
			return await _context.FoodItems.ToListAsync();
		}

		// GET: api/FoodItems/byId/5
		[HttpGet("byId/{id}")]
		public async Task<ActionResult<FoodItem>> GetFoodItemById(long id)
		{
			var foodItem = await _context.FoodItems.FindAsync(id);

			if (foodItem == null)
			{
				return NotFound();
			}

			return foodItem;
		}

		// GET: api/FoodItems/byName/Kohlrabi
		[HttpGet("byName/{name}")]
		public async Task<ActionResult<FoodItem>> GetFoodItemByName(string name)
		{
			FoodItem? foodItem = await _context.FoodItems.FirstOrDefaultAsync(x => (x.Name ?? string.Empty).ToLower() == name.ToLower()); // ignore case

			if (foodItem == null)
			{
				return NotFound();
			}


			return foodItem;
		}

		// GET: api/FoodItems/inGroup/Herbs%20and%20Spices
		[HttpGet("inGroup/{groupName}")]
		public async Task<ActionResult<IEnumerable<FoodItem>>> GetFoodGroup(string groupName)
		{
			var foodGroup = await _context.FoodItems.Where(x => (x.Group ?? string.Empty).ToLower() == groupName.ToLower()).ToListAsync();

			if (foodGroup == null || foodGroup.Count == 0)
			{
				return NotFound();
			}

			return foodGroup;
		}

		// GET: api/FoodItems/inSubGroup/Spices
		[HttpGet("inSubGroup/{subGroupName}")]
		public async Task<ActionResult<IEnumerable<FoodItem>>> GetSubGroup(string subGroupName)
		{
			var subGroup = await _context.FoodItems.Where(x => (x.SubGroup ?? string.Empty).ToLower() == subGroupName.ToLower()).ToListAsync();

			if (subGroup == null || subGroup.Count == 0) { return NotFound(); }

			return subGroup;
		}

		// GET: api/FoodItems/groups
		[HttpGet("groups")]
		public async Task<ActionResult<IEnumerable<string?>>> GetValidGroups()
		{
			var groups = await _context.FoodItems.Select(x => x.Group).Distinct().OrderBy(g => g).ToListAsync();

			if (!groups.Any()) { return NotFound(); }

			return groups;
		}

		// GET: api/FoodItems/subGroups
		[HttpGet("subGroups")]
		public async Task<ActionResult<IEnumerable<string?>>> GetValidSubGroups()
		{
			var groups = await _context.FoodItems.Select(x => x.SubGroup).Distinct().OrderBy(sg => sg).ToListAsync();

			if (!groups.Any()) { return NotFound(); }

			return groups;
		}


		// PUT: api/FoodItems/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutFoodItem(long id, FoodItem foodItem)
		{
			if (id != foodItem.Id)
			{
				return BadRequest();
			}

			_context.Entry(foodItem).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!FoodItemExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/FoodItems
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<FoodItem>> PostFoodItem(FoodItem foodItem)
		{
			_context.FoodItems.Add(foodItem);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetFoodItemById), new { id = foodItem.Id }, foodItem);
		}

		// DELETE: api/FoodItems/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFoodItem(long id)
		{
			var foodItem = await _context.FoodItems.FindAsync(id);
			if (foodItem == null)
			{
				return NotFound();
			}

			_context.FoodItems.Remove(foodItem);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool FoodItemExists(long id)
		{
			return _context.FoodItems.Any(e => e.Id == id);
		}
	}
}
