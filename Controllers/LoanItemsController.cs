using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CashflowApi.Models;
using HttpUtility = System.Web.HttpUtility;

namespace CashflowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanItemsController : ControllerBase
    {
        private readonly LoanContext _context;

        public LoanItemsController(LoanContext context)
        {
            _context = context;
        }

        // GET: api/LoanItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanItem>>> GetLoanItems()
        {
            return await _context.LoanItems.ToListAsync();
        }

        // GET: api/LoanItems/CashFlows?_ids=1,2,3,4
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanItem>>> GetCashFlows()
        {
            string _ids = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("_ids");
            List<int> TagIds = _ids.Split(',').Select(int.Parse).ToList();
            List<LoanItem> TagLoans = await _context.LoanItems.Where(loan => TagIds.contains(loan.Id))ToList();
            return CalculateCashFlow(TagLoans);
        }

        // GET: api/LoanItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanItem>> GetLoanItem(int id)
        {
            var loanItem = await _context.LoanItems.FindAsync(id);

            if (loanItem == null)
            {
                return NotFound();
            }

            return loanItem;
        }

        // PUT: api/LoanItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoanItem(int id, LoanItem loanItem)
        {
            if (id != loanItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(loanItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanItemExists(id))
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

        // POST: api/LoanItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoanItem>> PostLoanItem(LoanItem loanItem)
        {
            _context.LoanItems.Add(loanItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetLoanItem", new { id = loanItem.Id }, loanItem);
            return CreatedAtAction(nameof(GetLoanItem), new { id = loanItem.Id }, loanItem);
        }

        // DELETE: api/LoanItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoanItem(int id)
        {
            var loanItem = await _context.LoanItems.FindAsync(id);
            if (loanItem == null)
            {
                return NotFound();
            }

            _context.LoanItems.Remove(loanItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanItemExists(int id)
        {
            return _context.LoanItems.Any(e => e.Id == id);
        }
    }
}
