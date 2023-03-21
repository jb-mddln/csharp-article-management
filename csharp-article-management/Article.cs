namespace csharp_article_management
{
    /// <summary>
    /// Représente notre objet article
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Identifieur unique
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de l'article
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Prix de l'article
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Le stock disponible de l'article
        /// </summary>
        public int CurrentStock { get; set; }

        /// <summary>
        /// Le stock maximum de l'article
        /// </summary>
        public int MaxStock { get; set; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Article() 
        { 
        }

        /// <summary>
        /// Les informations de notre article sous forme de chaine de caractères
        /// </summary>
        /// <returns>Retourne les informations de notre article sous forme de chaine de caractères</returns>
        public override string ToString()
        {
            return "Id: " + this.Id + "\n"
                + "Name: " + this.Name + "\n"
                + "Prix: " + this.Price + " Euro \n"
                + "Stock: " + this.CurrentStock + " / " + this.MaxStock + "\n";
        }
    }
}