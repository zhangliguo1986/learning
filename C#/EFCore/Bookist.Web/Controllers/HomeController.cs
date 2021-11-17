using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookist.Web.Models;


namespace Bookist.Web.Controllers;

public class HomeController : Controller
{
    private readonly BookistDbContext _context;

    public HomeController(BookistDbContext context)
    {
        _context = context;
    }

    public async Task<ViewResult> Index()
    {
        var book = await _context.Set<Book>().OrderBy(x => x.Id).LastOrDefaultAsync();
        return View(book);
    }
}