using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;

namespace BaseDeDonnees
{
    class Program
    {
        public static string ConnectionString;
        static void Main(string[] args)
        {
            // Récupération de la connection string depuis le fichier de configuration
            // Pour accéder à ConfigurationManager, il faut ajouter la référence à System.Configuration puis faire le using approprié
            ConnectionString = ConfigurationManager.ConnectionStrings["MainDatabaseConnectionString"].ConnectionString;

            IEnumerable<Student> studentsFromDatabase = ReadDatas();
            // J'ai maintenant accès à une liste d'objets de type Student pour les afficher

            Console.ReadLine();

        }

        /// <summary>
        /// Récupère depuis la base la liste des étudiants qui sont sauvegardés
        /// </summary>
        /// <returns>Une liste d'étudiant (objets C#)</returns>
        static IEnumerable<Student> ReadDatas()
        {
            // Création de la liste dans laquelle les étudiants seront insérés
            List<Student> students = new List<Student>();

            // Création de la connexion à la base de données et ouverture de la connexion
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            // Si on est arrivé ici sans erreur, c'est que la connection string était correcte et que la base de données est accessible
            Console.WriteLine("Connexion établie !");

            // Création de la requête Sql pour récupérer les Students
            SqlCommand allStudentsCommand = new SqlCommand("select * from Students", connection);

            //Execution de la requête et récupération du reader pour lire ligne à ligne les résultats
            SqlDataReader reader = allStudentsCommand.ExecuteReader();

            // Boucle de lecture ligne à ligne du DbDataReader
            while (reader.Read())
            {
                // A partir des informations de chaque colonne de la base, on créé un étudiant en castant les données dans le bon type
                Student studentToCreate = new Student(reader["FirstName"].ToString(), reader["Name"].ToString(), (int)reader["Age"]);
                students.Add(studentToCreate);
            }

            // Ne pas oubliez de fermer les connexions !!
            reader.Close();
            connection.Close();

            return students;
        }
    }
}
