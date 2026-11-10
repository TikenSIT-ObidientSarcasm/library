using library.data;
using library.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace library.Controllers
{
    public class MembersController : Controller
    {
        private readonly MemberRepository _members;

        public MembersController(MemberRepository members)
        {
            _members = members;
        }

        // GET: /Members
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var members = await _members.GetAllAsync();
            return View(members);
        }

        // GET: /Members/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Member());
        }

        // POST: /Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View(member);
            }

            await _members.AddAsync(member);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Members/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var member = await _members.GetByIdAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: /Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Member member)
        {
            if (id != member.Member_id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(member);
            }

            var existingMember = await _members.GetByIdAsync(id);

            if (existingMember == null)
            {
                return NotFound();
            }

            await _members.UpdateAsync(member);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Members/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var member = await _members.GetByIdAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST /Members/Delete/5 : the delete was confirmed.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                await _members.DeleteAsync(id);
            }
            catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                var member = await _members.GetByIdAsync(id);
                if (member == null)
                {
                    return NotFound();
                }
                ViewBag.Error = "This member has loans on record, so the database will not delete them.";
                return View("Delete", member);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
