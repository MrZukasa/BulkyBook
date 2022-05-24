using System.ComponentModel.DataAnnotations;
namespace BulkyBookWeb.Models;

public class Category {
    [Key]           /*Assegno la chiave primaria alla props Id, questa è chiamata Data Annota*/
    public int Id { get; set; }
    [Required]      /* Aggiungo l'attributo che fa si che il nome sia validato come richiesto */    
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
}