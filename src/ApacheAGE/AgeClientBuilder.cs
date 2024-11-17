using System;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ApacheAGE
{
    public class AgeClientBuilder
    {
        private ILoggerFactory? _logger;
        private readonly string _connectionString;
        private readonly NpgsqlConnection? _npgsqlConnection;

        /// <summary>
        /// Create a client builder.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string to the database.
        /// </param>
        /// <exception cref="ArgumentException">
        /// A required argument is null or empty or default.
        /// </exception>
        public AgeClientBuilder(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            _connectionString = connectionString;
        }

        /// <summary>
        /// Create a client builder with an existing NpgsqlConnection.
        /// </summary>
        /// <param name="npgsqlConnection">
        /// Existing NpgsqlConnection to the database.
        /// </param>
        /// <exception cref="ArgumentException">
        /// A required argument is null or empty or default.
        /// </exception>
        public AgeClientBuilder(NpgsqlConnection npgsqlConnection)
        {
            _npgsqlConnection = npgsqlConnection ?? throw new ArgumentException("NpgsqlConnection cannot be null.", nameof(npgsqlConnection));
        }

        /// <summary>
        /// Make use of a given configured logger.
        /// </summary>
        /// <param name="logger">
        /// Logger factory that will be used for logging.
        /// </param>
        /// <returns>
        /// The same builder instance so multiple calls could be chained.
        /// </returns>
        public AgeClientBuilder UseLogger(ILoggerFactory logger)
        {
            _logger = logger;
            return this;
        }

        /// <summary>
        /// Generate a new <see cref="AgeClient"/> using the configurations
        /// set in the builder.
        /// </summary>
        /// <returns></returns>
        public AgeClient Build()
        {
            if (_npgsqlConnection != null)
            {
                return new AgeClient(_npgsqlConnection, BuildConfigurations());
            }
            return new AgeClient(_connectionString, BuildConfigurations());
        }

        private AgeConfiguration BuildConfigurations() => 
            new(_logger is null
                    ? AgeLoggerConfiguration.NullLoggerConfiguration
                    : new AgeLoggerConfiguration(_logger));
    }
}
