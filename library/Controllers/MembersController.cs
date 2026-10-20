using library.data;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers;

public class MembersController : Controller
{
    private readonly MemberRepository _members;

    // ASP.NET Core hands in the repository, because Program.cs registered it.
    public MembersController(MemberRepository members)
    {
        _members = members;
    }

    // GET /Members
    public async Task<IActionResult> Index()
    {
        var members = await _members.GetAllAsync();
        return View(members);
    }

}