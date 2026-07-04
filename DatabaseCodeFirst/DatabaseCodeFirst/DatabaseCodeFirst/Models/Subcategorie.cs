namespace DatabaseCodeFirst.Models
{
    public class Subcategorie
    {
        public int SubcategorieId { get; set; }

        public string? Name { get; set; }

        public int CategorieId { get; set; }

        public Categorie Categorie { get; set; }
    }
}
