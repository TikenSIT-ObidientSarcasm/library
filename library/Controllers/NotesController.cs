using library.Data;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers;

public class NotesController : Controller
{
    private readonly NotesRepository _notes;

    // ASP.NET Core hands in the repository, because Program.cs registered it.
    public NotesController(NotesRepository notes)
    {
        _notes = notes;
    }

    // GET /Notes
    public async Task<IActionResult> Index()
    {
        var notes = await _notes.GetAllAsync();
        return View(notes);
    }

}