using Microsoft.AspNetCore.Mvc;
using BulkyBookWeb.Data;
using BulkyBookWeb.Models;

namespace BulkyBookWeb.Controllers;
// definisco il controller che richiamerò la classe che ho creato per leggere dal DB
public class CategoryController : Controller
{
    // leggo il parametro passato dalla classe creata in precedenza, il parametro passato è l'intero DB
    private readonly ApplicationDbContext _db;
    // associo al controller il db che leggo come parametro
    public CategoryController(ApplicationDbContext db)
    {
        _db = db;
    }
    public IActionResult Index()
    {
        // leggo il database e lo interpolo con un interfaccia
        IEnumerable<Category> objCategoryList = _db.Categories;
        return View(objCategoryList);
    }
    // GET
    public IActionResult Create()
    {
        return View();
    }
    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if(obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name","The DisplayOrder cannot exactly match the Name.");
        }
        if (ModelState.IsValid){
            // aggiungo il parametro al DB
            _db.Categories.Add(obj);
            // pusho i dati al DB
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(obj);
    }
}