using BulkyBookWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Data;
public class ApplicationDbContext :DbContext
{
    /* definisco il costruttore all'interno della mia classe */
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
    {        
    }
    /* definisco il nome dalla tabella che andrò a creare */
    public DbSet<Category> Categories { get; set; }   
}