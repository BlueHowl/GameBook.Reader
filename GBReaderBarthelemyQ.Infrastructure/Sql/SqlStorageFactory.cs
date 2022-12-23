using GBReaderBarthelemyQ.Repositories;
using GBReaderBarthelemyQ.Repositories.Exceptions;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace GBReaderBarthelemyQ.Repository.Sql
{
    /// <summary>
    /// Classe SqlStorageFactory
    /// </summary>
    public class SqlStorageFactory : IDataFactoryInterface
    {
        private readonly DbProviderFactory _factory;
        private readonly string _connectionString;

        private readonly IdKeeper _idKeeper;

        /// <summary>
        /// Constructeur de SqlStorageFactory
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        /// <exception cref="ProviderNotFoundException"></exception>
        public SqlStorageFactory(string providerName, string connectionString)
        {
            _idKeeper = new IdKeeper();

            var fullName = typeof(MySqlClientFactory).AssemblyQualifiedName;
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", fullName!);

            try
            {
                _factory = DbProviderFactories.GetFactory(providerName);
                _connectionString = connectionString;
            }
            catch (ArgumentException e)
            {
                throw new ProviderNotFoundException($"Unable to load prodiver {providerName}", e);
            }
        }

        /// <summary>
        /// Crée et renvoi un nouveau Storage
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnableToConnectException"></exception>
        /// <exception cref="InvalidConnectionStringException"></exception>
        public IDataInterface NewStorage()
        {
            try
            {
                IDbConnection con = _factory.CreateConnection()!;

                if (con == null)
                {
                    throw new UnableToConnectException("Impossible de se connecter", new Exception());
                }

                con.ConnectionString = _connectionString;
                con.Open();

                return new SqlStorage(con, _idKeeper);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidConnectionStringException(ex);
            }
            catch (MySqlException ex)
            {
                throw new UnableToConnectException("Impossible d'établir une connexion à la bd", ex);
            }
        }
    }

    public class ProviderNotFoundException : Exception
    {
        public ProviderNotFoundException(string s, Exception argumentException)
            : base(s, argumentException)
        {
        }
    }

    public class InvalidConnectionStringException : Exception
    {
        public InvalidConnectionStringException(Exception argumentException)
            : base("Unable to use this connection string", argumentException)
        {
        }
    }
}
