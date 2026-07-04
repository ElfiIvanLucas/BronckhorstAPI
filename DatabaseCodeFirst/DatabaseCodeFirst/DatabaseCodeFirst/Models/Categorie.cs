using System.Numerics;

namespace DatabaseCodeFirst.Models
{
    public class Categorie
    {
        public int CategorieId { get; set; }
        public string? Name { get; set; }

        public  ICollection<Subcategorie> Subcategories { get; set; }
    }
}
