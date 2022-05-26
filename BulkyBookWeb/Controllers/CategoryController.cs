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
            TempData["Success"] = "Creation Successful";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    // GET
    public IActionResult Edit(int? Id)
    {
        if (Id == null || Id == 0)
        {
            return NotFound();
        }
        var categoryFromDb = _db.Categories.Find(Id);
        // var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id==Id);
        // var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id==Id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }
    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if(obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name","The DisplayOrder cannot exactly match the Name.");
        }
        if (ModelState.IsValid){            
            _db.Categories.Update(obj);        
            _db.SaveChanges();
            TempData["Success"] = "Edit Successful";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    // GET
    public IActionResult Delete(int? Id)
    {
        if (Id == null || Id == 0)
        {
            return NotFound();
        }
        var categoryFromDb = _db.Categories.Find(Id);
        // var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id==Id);
        // var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id==Id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }
    // POST
    [HttpPost,ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? Id)
    {
        var obj = _db.Categories.Find(Id);
        if(obj == null)
        {
            return NotFound();
        }        
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["Success"] = "Delete Successful";
            return RedirectToAction("Index");
    }
}