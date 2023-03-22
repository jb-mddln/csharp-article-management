namespace csharp_article_management
{
    public class Program
    {
        // Déclare notre liste d'articles et l'initialise lors du lancement du programme tant que liste vide
        private static List<Article> Articles { get; set; } = new List<Article>();

        public static void Main(string[] args)
        {
            Article articleOne = new Article();
            articleOne.Id = 1;
            articleOne.Name = "Rhum";
            articleOne.Price = 6.50;
            articleOne.CurrentStock = 43;
            articleOne.MaxStock = 100;

            Article articleTwo = new Article();
            articleTwo.Id = 2;
            articleTwo.Name = "Coca-Cola";
            articleTwo.Price = 2.10;
            articleTwo.CurrentStock = 16;
            articleTwo.MaxStock = 75;

            Article articleThree = new Article();
            articleThree.Id = 3;
            articleThree.Name = "Capri-sun";
            articleThree.Price = 1.80;
            articleThree.CurrentStock = 8;
            articleThree.MaxStock = 20;

            Articles.Add(articleOne);
            Articles.Add(articleTwo);
            Articles.Add(articleThree);

            DisplayMenu();
            while (true)
            {
                // Récupère le texte entré sur la console
                string? line = Console.ReadLine();

                // Si texte est vide ou null
                if (string.IsNullOrEmpty(line))
                {
                    Console.WriteLine("> Erreur ligne vide ...");
                    continue;
                }

                // Vérifie que le texte entré est bien un int valide
                if (!int.TryParse(line, out int optionId))
                {
                    Console.WriteLine($"> Erreur {line} n'est pas une option valide");
                    continue;
                }

                // Gestion de nos différentes options
                switch (optionId)
                {
                    // 1) Rechercher un article par référence.
                    case 1:
                        DisplayArticleById();
                        break;

                    // 2)  Ajouter un article au stock en vérifiant l’unicité de la référence.
                    case 2:
                        AddArticle();
                        break;

                    // 3) Supprimer un article par référence.
                    case 3:
                        DeleteArticleById();
                        break;

                    // 4) Modifier un article par référence.
                    case 4:
                        EditArticleById();
                        break;

                    // 5) Rechercher un article par nom.
                    case 5:
                        DisplayArticleByName();
                        break;

                    // 6) Rechercher un article par intervalle de prix de vente.
                    case 6:
                        DisplayAllArticlesByPriceRange();

                        // Alternative une ligne sans boucle avec Join et LINQ WHere + Condition et Select
                        // Console.WriteLine(string.Join("\n", articles.Where(article => article.Price >= 1.65 && article.Price <= 2.4).Select(article => article.ToString())));
                        break;

                    // 7) Afficher tous les articles.
                    case 7:
                        DisplayAllArticles();

                        // Alternative une ligne sans boucle avec Join et LINQ Select
                        // Console.WriteLine(string.Join("\n", articles.Select(article => article.ToString())));
                        break;

                    // Quitter
                    case 8:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("> Entrez d'abord une option valide, les options valides sont '1, 2, 3, 4, 5, 6, 6, 7, 8' ...");
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche le menu dans notre console
        /// </summary>
        private static void DisplayMenu()
        {
            // @ Pour un string multi ligne
            Console.WriteLine(@"----
----
> Entrez '1, 2, 3, 4, 5, 6, 7, 8' pour sélectionner rapidement une option ...
----
1) Rechercher un article par id
2) Ajouter un article
3) Supprimer un article
4) Modifier un article
5) Rechercher un article par nom
6) Rechercher un article par intervalle de prix de vente (prix min - prix max)
7) Afficher tous les articles
8) Quitter l'application
----
----");
        }

        /// <summary>
        /// Boucle foreach dans notre liste d'articles pour afficher l'article dont l'id est égal à l'id entré par l'utilisateur
        /// </summary>
        /// <param name="articles"></param>
        private static void DisplayArticleById()
        {
            string articleIdString = HandleUserInput("Id Article");

            // Vérifie que l'id entré est bien un int valide
            if (!int.TryParse(articleIdString, out int articleId))
            {
                Console.WriteLine($"> Erreur {articleId} n'est pas un id d'article valide");
                return;
            }

            // Vérifie que notre liste contient bien un article avec l'id entrer par l'utilisateur
            if (!Articles.Any(article => article.Id == articleId))
            {
                Console.WriteLine($"> Erreur aucun article avec l'id {articleId} n'a pu être trouvé ...");
                return;
            }

            // Alternative une ligne sans boucle avec LINQ FirstOrDefault + Condition (premier résultat trouvé avec notre condition ou null)
            /* Article? article = articles.FirstOrDefault(article => article.Id == articleId);
            if (article != null)
            {
                Console.WriteLine($"> Article trouvé avec l'id {articleId}: \n");
                Console.WriteLine(article.ToString());
            } */

            Console.WriteLine($"> Article trouvé avec l'id {articleId}: \n");
            foreach (Article article in Articles)
            {
                // Si l'id est égal à l'id entré par l'utilisateur
                if (article.Id == articleId)
                {
                    Console.WriteLine(article.ToString());
                    // Article trouvé on stop notre boucle
                    break;
                }
            }
        }


        /// <summary>
        /// Ajoute un article à notre liste d'articles
        /// </summary>
        /// <param name="articles"></param>
        private static void AddArticle()
        {
            string articleName = HandleUserInput("Nom de l'article");

            // On cherche si un article du même nom existe déjà, si c'est le cas on ajoute juste au stock de l'article
            Article? article = Articles.FirstOrDefault(article => article.Name.ToLower().Equals(articleName));
            if (article != null)
            {
                article.CurrentStock += 1;
                Console.WriteLine($"> Succès l'article {article.Name} a été correctement modifié (+ 1 au stock)");
            }
            else
            {
                // L'article n'existe pas ont le créer

                string articlePrice = HandleUserInput("Prix de l'article");

                // Vérifie que le prix entré est bien un double valide
                if (!double.TryParse(articlePrice, out double price))
                {
                    Console.WriteLine($"> Erreur {price} n'est pas un prix valide");
                    return;
                }

                string articleStock = HandleUserInput("Stock de l'article");

                // Vérifie que le prix entré est bien un int valide
                if (!int.TryParse(articleStock, out int stock))
                {
                    Console.WriteLine($"> Erreur {stock} n'est pas un stock valide");
                    return;
                }

                Article newArticle = new Article();
                // Retourne l'id max de notre liste et ajoute + 1 pour un id libre
                newArticle.Id = Articles.Max(article => article.Id) + 1;
                newArticle.Name = articleName;
                newArticle.Price = price;
                newArticle.CurrentStock = stock;
                newArticle.MaxStock = stock;

                Articles.Add(newArticle);

                Console.WriteLine($"> Succès l'article {newArticle.Name} a été correctement ajouté");
            }
        }

        /// <summary>
        /// Supprime l'article dont l'id est égal à celui entré par l'utilisateur de notre liste
        /// </summary>
        /// <param name="articles"></param>
        private static void DeleteArticleById()
        {
            string articleIdString = HandleUserInput("Id Article");

            // Vérifie que l'id entré est bien un int valide
            if (!int.TryParse(articleIdString, out int articleId))
            {
                Console.WriteLine($"> Erreur {articleId} n'est pas un id d'article valide");
                return;
            }

            if (!Articles.Any(article => article.Id == articleId))
            {
                Console.WriteLine($"> Erreur aucun article avec l'id {articleId} n'a pu être trouvé ...");
                return;
            }

            Articles.RemoveAll(article => article.Id == articleId);
            Console.WriteLine($"> Succès l'article avec l'id {articleId} a bien été supprimer des articles");
        }

        /// <summary>
        /// Modifie l'article dont l'id est égal à l'id entré par l'utilisateur
        /// </summary>
        /// <param name="articles"></param>
        private static void EditArticleById()
        {
            string articleIdString = HandleUserInput("Id Article");

            // Vérifie que l'id entré est bien un int valide
            if (!int.TryParse(articleIdString, out int articleId))
            {
                Console.WriteLine($"> Erreur {articleId} n'est pas un id d'article valide");
                return;
            }

            // Vérifie que notre liste contient bien un article avec l'id entrer par l'utilisateur
            if (!Articles.Any(article => article.Id == articleId))
            {
                Console.WriteLine($"> Erreur aucun article avec l'id {articleId} n'a pu être trouvé ...");
                return;
            }

            // Alternative une ligne sans boucle avec LINQ FirstOrDefault + Condition (premier résultat trouvé avec notre condition ou null)
            Article? article = Articles.FirstOrDefault(article => article.Id == articleId);
            if (article != null)
            {
                string articlePrice = HandleUserInput("Prix de l'article", true);
                // L'utilisateur a entré un prix on modifie donc le prix de notre article
                if (!string.IsNullOrEmpty(articlePrice))
                {
                    // Vérifie que le prix entré est bien un double valide
                    if (!double.TryParse(articlePrice, out double price))
                    {
                        Console.WriteLine($"> Erreur {price} n'est pas un prix valide");
                        return;
                    }

                    // Le prix actuel de l'article n'est pas le même que le prix entré par l'utilisateur, on le modifie
                    if (article.Price != price)
                        article.Price = price;
                }

                string articleStock = HandleUserInput("Stock disponible de l'article", true);
                // L'utilisateur a entré un stock on modifie donc le stock de notre article
                if (!string.IsNullOrEmpty(articleStock))
                {
                    // Vérifie que le prix entré est bien un int valide
                    if (!int.TryParse(articleStock, out int stock))
                    {
                        Console.WriteLine($"> Erreur {stock} n'est pas un stock valide");
                        return;
                    }

                    // Le stock actuel de l'article n'est pas le même que le stock entré par l'utilisateur, on le modifie
                    if (article.CurrentStock != stock)
                        article.CurrentStock = stock;
                }

                string articleMaxStock = HandleUserInput("Stock maximum de l'article", true);
                // L'utilisateur a entré un stock max on modifie donc le stock max de notre article
                if (!string.IsNullOrEmpty(articleMaxStock))
                {
                    // Vérifie que le stock max entré est bien un int valide
                    if (!int.TryParse(articleMaxStock, out int maxStock))
                    {
                        Console.WriteLine($"> Erreur {articleMaxStock} n'est pas un stock valide");
                        return;
                    }

                    // Le stock max actuel de l'article n'est pas le même que le stock max entré par l'utilisateur, on le modifie
                    if (article.MaxStock != maxStock)
                        article.MaxStock = maxStock;
                }
               
                Console.WriteLine($"> Succès l'article {article.Name} a été correctement modifié");
            }
        }

        /// <summary>
        /// Boucle foreach dans notre liste d'articles pour afficher l'article dont le nom est égal au nom entré par l'utilisateur
        /// </summary>
        /// <param name="articles"></param>
        private static void DisplayArticleByName()
        {
            string name = HandleUserInput("Nom").ToLower();
            if (name.Length >= 1 && name.Length < 3)
            {
                Console.WriteLine($"> Merci d'entrer un nom valide 3 caractères minimums ...");
                return;
            }

            // Alternative une ligne sans boucle avec LINQ FirstOrDefault + Condition (premier résultat trouvé avec notre condition ou null)
            /* Article? article = articles.FirstOrDefault(article => article.Name.ToLower().Equals(name));
            if (article != null)
            {
                Console.WriteLine($"> Article trouvé avec le nom {name}: \n");
                Console.WriteLine(article.ToString());
            } */

            Console.WriteLine($"> Article trouvé avec le nom {name}: \n");
            foreach (Article article in Articles)
            {
                // Si le nom en minuscule est égal au nom entré par l'utilisateur
                if (article.Name.ToLower().Equals(name))
                {
                    Console.WriteLine(article.ToString());
                    // Article trouvé on stop notre boucle
                    break;
                }
            }
        }

        /// <summary>
        /// Boucle foreach dans notre liste d'articles pour afficher leurs informations selon si le prix est compris entre prix min et prix max 
        /// </summary>
        /// <param name="articles"></param>
        private static void DisplayAllArticlesByPriceRange()
        {
            string priceMinString = HandleUserInput("Prix minimum");

            // Vérifie que le prix entré est bien un double valide
            if (!double.TryParse(priceMinString, out double priceMin))
            {
                Console.WriteLine($"> Erreur {priceMinString} n'est pas un prix valide");
                return;
            }

            string priceMaxString = HandleUserInput("Prix maximum");

            // Vérifie que le prix entré est bien un double valide
            if (!double.TryParse(priceMaxString, out double priceMax))
            {
                Console.WriteLine($"> Erreur {priceMaxString} n'est pas un prix valide");
                return;
            }

            Console.WriteLine($"> Liste des articles dont le prix est compris entre {priceMin} Euro et {priceMax} Euro: \n");

            // LINQ OrderBy pour sortir les résultats par prix croissant
            foreach (Article article in Articles.OrderBy(article => article.Price))
            {
                // Si le prix est égal ou supérieur à prix min et que prix est inférieur ou égale à prix max
                if (article.Price >= priceMin && article.Price <= priceMax)
                {
                    Console.WriteLine(article.ToString());
                }
            }
        }

        /// <summary>
        /// Boucle foreach dans notre liste d'articles pour afficher leurs informations
        /// </summary>
        /// <param name="articles"></param>
        private static void DisplayAllArticles()
        {
            Console.WriteLine("> Liste des articles: \n");

            foreach (Article article in Articles)
            {
                Console.WriteLine(article.ToString());
            }
        }

        /// <summary>
        /// Récupère le texte entré par l'utilisateur et vérifie son intégrité
        /// </summary>
        /// <param name="parameterName">Nom du paramètre que l'utilisateur doit entrer</param>
        /// <param name="allowEmpty">Autorise ou non l'entrée d'un paramètre vide</param>
        /// <returns>string vide ou le paramètre entré par l'utilisateur</returns>
        private static string HandleUserInput(string parameterName, bool allowEmpty = false)
        {
            Console.WriteLine($"> Entrez {parameterName}: ");

            // Récupère le texte entré par l'utilisateur
            string? parameter = Console.ReadLine();

            // Boucle qui si n'accapte pas une entrée vide et que le texte est vide ou null on boucle jusqu'à ce que l'utilisateur entre quelque chose, si accepte l'entrée vide on sort et retourne un string vide
            while (!allowEmpty && string.IsNullOrEmpty(parameter))
            {
                Console.WriteLine($"> Erreur {parameterName} ne peut pas être vide ...");
                Console.WriteLine($"> Entrez {parameterName}: ");
                parameter = Console.ReadLine();
            }

            // Condition ternaire si n'accepte pas un texte vide et que le texte n'est pas vide ou null on retourne le texte entré par l'utilisateur sinon un string vide
            return !allowEmpty && !string.IsNullOrEmpty(parameter) ? parameter : string.Empty;
        }
    }
}