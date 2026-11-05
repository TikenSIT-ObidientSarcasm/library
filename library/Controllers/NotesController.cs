
using library.data;
using library.Models;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers;

public class NotesController : Controller
{
    private readonly NotesRepository _notes;

    public NotesController(NotesRepository notes)
    {
        _notes = notes;
    }

    // GET: /Notes
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var notes = await _notes.GetAllAsync();
        return View(notes);
    }

    // GET: /Notes/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Note());
    }

    // POST: /Notes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Note note)
    {
        if (!ModelState.IsValid)
        {
            return View(note);
        }

        await _notes.AddAsync(note);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Notes/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var note = await _notes.GetByIdAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return View(note);
    }

    // POST: /Notes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, Note note)
    {
        if (id != note.note_id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(note);
        }

        var existingNote = await _notes.GetByIdAsync(id);

        if (existingNote == null)
        {
            return NotFound();
        }

        await _notes.UpdateAsync(note);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Notes/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(long id)
    {
        var note = await _notes.GetByIdAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return View(note);
    }

    // POST: /Notes/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var note = await _notes.GetByIdAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        await _notes.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}
