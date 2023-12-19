using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace GBReaderBarthelemyQ.Repository.Sql
{
    /// <summary>
    /// Classe SqlStorage  LCOM = 1-(7/(2*8)) = 0.5625
    /// </summary>
    public class SqlStorage : IDisposable, IDataInterface
    {
        private const string CREATE_AUTHOR_COMMAND = @"CREATE TABLE AUTHOR(" +
            "code VARCHAR(6) NOT NULL PRIMARY KEY," +
            "surname VARCHAR(50) NOT NULL," +
            "name VARCHAR (50) NOT NULL)";

        private const string CREATE_BOOK_COMMAND = @"CREATE TABLE BOOK(" +
            "id INT NOT NULL AUTO_INCREMENT PRIMARY KEY," +
            "title VARCHAR(150) NOT NULL," +
            "summary VARCHAR (500) NOT NULL," +
            "isbn VARCHAR(13) NOT NULL," +
            "published BIT)";

        private const string CREATE_PAGE_COMMAND = @"CREATE TABLE PAGE(" +
            "id INT NOT NULL AUTO_INCREMENT PRIMARY KEY," +
            "text VARCHAR(3000) NOT NULL," +
            "num INT NOT NULL," +
            "book_id INT NOT NULL," +
            "FOREIGN KEY(book_id) REFERENCES BOOK(id)" +
            "ON DELETE CASCADE)";

        private const string CREATE_CHOICE_COMMAND = @"CREATE TABLE CHOICE(" +
            "id INT NOT NULL AUTO_INCREMENT PRIMARY KEY," +
            "text VARCHAR(250) NOT NULL," +
            "ref_page_id INT NOT NULL," +
            "page_id INT NOT NULL," +
            "FOREIGN KEY(page_id) REFERENCES PAGE(id)" +
            "ON DELETE CASCADE)";


        private const string SELECT_BOOKS_FROM_ISBNS_COMMAND = @"SELECT B.id AS id, B.title AS title, B.summary AS summary, B.isbn AS isbn, A.surname AS surname, A.name AS name " +
                                                             "FROM BOOK B " +
                                                             "JOIN AUTHOR A ON A.code = SUBSTR(B.isbn, 3, 6) " +
                                                             "WHERE published = true AND isbn IN ";

        private const string SELECT_RANDOM_BOOKS_COMMAND = @"SELECT B.id AS id, B.title AS title, B.summary AS summary, B.isbn AS isbn, A.surname AS surname, A.name AS name " +
                                                            "FROM BOOK B " +
                                                            "JOIN AUTHOR A ON A.code = SUBSTR(B.isbn, 3, 6) " +
                                                            "WHERE published = true " +
                                                            "ORDER BY RAND() " +
                                                            "LIMIT 25";

        private const string SELECT_FILTERED_BOOKS_COMMAND = @"SELECT B.id AS id, B.title AS title, B.summary AS summary, B.isbn AS isbn, A.surname AS surname, A.name AS name " +
                                                            "FROM BOOK B " +
                                                            "JOIN AUTHOR A ON A.code = SUBSTR(B.isbn, 3, 6) " +
                                                            "WHERE published = true AND title LIKE @title OR isbn = @isbn";


        private const string SELECT_BOOK_PAGES_COMMAND = @"SELECT * FROM PAGE WHERE book_id = @bookId ORDER BY num ASC";

        private const string SELECT_BOOK_CHOICES_COMMAND = @"SELECT C.id AS id, C.page_id AS ownerPage, C.ref_page_id AS refPage, C.text AS text " +
                                                            "FROM CHOICE C " +
                                                            "JOIN PAGE P ON P.id = C.page_id " +
                                                            "WHERE P.book_id = @bookId";

        private readonly IDbConnection _con;

        private readonly IdKeeper _idKeeper;

        /// <summary>
        /// Constructeur de la classe SqlStorage
        /// </summary>
        /// <param name="con"></param>
        /// <param name="idKeeper"></param>
        public SqlStorage(IDbConnection con, IdKeeper idKeeper)
        {
            _con = con;
            _idKeeper = idKeeper;
        }

        /*
        public int Setup() //TODO only for tests same for java
        {
            int rows = 0;
            try
            {
                using IDbCommand createCommand = _con.CreateCommand();
                createCommand.CommandText = CREATE_AUTHOR_COMMAND;
                rows += createCommand.ExecuteNonQuery();

                createCommand.CommandText = CREATE_BOOK_COMMAND;
                rows += createCommand.ExecuteNonQuery();

                createCommand.CommandText = CREATE_PAGE_COMMAND;
                rows += createCommand.ExecuteNonQuery();

                createCommand.CommandText = CREATE_CHOICE_COMMAND;
                rows += createCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                //Should be Handled
                Console.WriteLine(ex.Message);

                throw new Exception(ex.Message);
            }

            return rows;
        }

        public int Teardown() //todo only for tests same for java
        {
            int rows = 0;

            try
            {
                using (IDbCommand dropCommand = _con.CreateCommand())
                {
                    dropCommand.CommandText = "DROP TABLE CHOICE";
                    rows += dropCommand.ExecuteNonQuery();

                    dropCommand.CommandText = "DROP TABLE PAGE";
                    rows += dropCommand.ExecuteNonQuery();

                    dropCommand.CommandText = "DROP TABLE BOOK";
                    rows += dropCommand.ExecuteNonQuery();

                    dropCommand.CommandText = "DROP TABLE AUTHOR";
                    rows += dropCommand.ExecuteNonQuery();
                }

                return rows;
            }
            catch (SqlException ex)
            {
                //Should be Handled
                Console.WriteLine(ex.Message);

                throw new Exception(ex.Message);
            }
        }*/

        /// <summary>
        /// Récupère une liste de livre sur base d'une query
        /// </summary>
        /// <param name="searchString">(String) chaine de caractère de recherche</param>
        /// <returns>(IReadOnlyList<Book>) Liste de livres (retourne des livres aléatoires si searchString est vide ou null)</returns>
        public IReadOnlyList<Book> LoadBooks(string searchString)
        {
            List<Book> books = new List<Book>();

            try
            {
                using IDbCommand selectCommand = _con.CreateCommand();

                if (searchString == null || searchString.Length == 0)
                {
                    selectCommand.CommandText = SELECT_RANDOM_BOOKS_COMMAND;
                }
                else
                {
                    selectCommand.CommandText = SELECT_FILTERED_BOOKS_COMMAND;
                    AddParam(selectCommand, "@title", "%" + searchString + "%", DbType.String);
                    AddParam(selectCommand, "@isbn", searchString, DbType.String);
                }

                using IDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    string authorInfos = (string)reader["surname"] + " " + (string)reader["name"];
                    Book book = new Book(new Cover((string)reader["title"], (string)reader["summary"], authorInfos,
                        new Isbn((string)reader["isbn"])), null!);

                    books.Add(book);

                    _idKeeper.AddBook(book, (int)reader["id"]);
                }
            }
            catch (SqlException ex)
            {
                //Should be Handled todo
                Console.WriteLine(ex.Message);

                throw new Exception(ex.Message);
            }

            return books;
        }

        /// <summary>
        /// Récupère une liste de livre sur des isbns
        /// </summary>
        /// <param name="isbns">(string) isbns en string</param>
        /// <returns>(IReadOnlyList<Book>) Liste de livres</returns>
        public IReadOnlyList<Book> LoadBooksByIsbns(string[] isbns)
        {
            List<Book> books = new List<Book>();

            try
            {
                using IDbCommand selectCommand = _con.CreateCommand();

                selectCommand.CommandText = SELECT_BOOKS_FROM_ISBNS_COMMAND;
                LaDaronnade(selectCommand, isbns); //Normalement je l'aurai appelé BindIsbns

                using IDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    string authorInfos = (string)reader["surname"] + " " + (string)reader["name"];
                    Book book = new Book(new Cover((string)reader["title"], (string)reader["summary"], authorInfos,
                        new Isbn((string)reader["isbn"])), null!);

                    books.Add(book);

                    _idKeeper.AddBook(book, (int)reader["id"]);
                }
            }
            catch (SqlException ex)
            {
                //Should be Handled todo
                Console.WriteLine(ex.Message);

                throw new Exception(ex.Message);
            }

            return books;
        }

        /// <summary>
        /// La méthode Daron ou BindIsbns
        /// permet de binder les isbns à la commande
        /// </summary>
        /// <param name="selectCommand">(IDbCommand)</param>
        /// <param name="isbns">(string[]) tableau de isbns en string</param>
        private void LaDaronnade(IDbCommand selectCommand, string[] isbns)
        {
            selectCommand.CommandText += "(";
            for (int i = 0; i < isbns.Length; i++)
            {
                if (i > 0)
                {
                    selectCommand.CommandText += ',';
                }

                selectCommand.CommandText += $"@p{i}";
                AddParam(selectCommand, $"@p{i}", isbns[i], DbType.String);
            }
            selectCommand.CommandText += ")";
        }

        /// <summary>
        /// Charge les pages et les choix du livre
        /// </summary>
        /// <param name="book">(Book) livre</param>
        public void LoadPages(Book book)
        {
            try
            {
                LoadBookPages(book);
                LoadChoicesInPage(book);
            }
            catch (Exception e)
            {
                book.Pages = null!;
                Console.WriteLine(e);//TODO handle
                throw;
            }
        }

        /// <summary>
        /// Ajoute les pages avec leur choix au livre
        /// </summary>
        /// <param name="book">(Book) livre</param>
        private void LoadBookPages(Book book)
        {
            List<Page> pages = new List<Page>();

            using IDbCommand selectCommand = _con.CreateCommand();

            selectCommand.CommandText = SELECT_BOOK_PAGES_COMMAND;
            AddParam(selectCommand, "@bookId", _idKeeper.GetBookId(book), DbType.Int32);
            Console.WriteLine(_idKeeper.GetBookId(book));

            using (IDataReader reader = selectCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Page page = new Page((string)reader["text"], null!);
                    pages.Add(page);

                    _idKeeper.AddPage(page, (int)reader["id"]);
                }
            }

            book.Pages = pages;
        }

        /// <summary>
        /// Assigne les choix à leur pages
        /// </summary>
        /// <param name="book">(Book) livre</param>
        private void LoadChoicesInPage(Book book)
        {
            using IDbCommand selectCommand = _con.CreateCommand();

            selectCommand.CommandText = SELECT_BOOK_CHOICES_COMMAND;
            AddParam(selectCommand, "@bookId", _idKeeper.GetBookId(book), DbType.Int32);

            using IDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                Page page = _idKeeper.GetPageById((int)reader["ownerPage"]);
                //Page page = book.Pages[(int)reader["ownerPage"]];
                page.Choices.Add(new Choice((string)reader["text"], (int)reader["refPage"])); //TODO demeter?
            }
        }

        /// <summary>
        /// Ajoute un paramètre à une commande
        /// </summary>
        /// <param name="command">(IDbCommand) commande</param>
        /// <param name="paramName">(String) nom du paramètre</param>
        /// <param name="val">(object) valeur</param>
        /// <param name="type">(DbType) type de la valeur</param>
        private void AddParam(IDbCommand command, string paramName, object val, DbType type)
        {
            var nameParam = command.CreateParameter();
            nameParam.ParameterName = paramName;
            nameParam.Value = val;
            nameParam.DbType = type;
            command.Parameters.Add(nameParam);
        }

        public void Dispose() => _con.Dispose();

    }

    public class NotFoundException : Exception
    {
    }
}