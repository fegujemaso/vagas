
using System;

namespace Vagas.Database.Base
{
    public enum DATABASE_TIPO_ID
    {
        MSSQL = 1,
        ORACLE = 2,
        MYSQL = 3
    }

    public class _BaseItem
    {
        public static DATABASE_TIPO_ID DatabaseTipoId
        {
            get
            {
                var databaseTipoId = (DATABASE_TIPO_ID)Enum.Parse(typeof(DATABASE_TIPO_ID), System.Configuration.ConfigurationManager.AppSettings["Database.Tipo.Id"]?.ToString());

                return databaseTipoId;
            }
        }

        public static string DatabaseUrl
        {
            get
            {
                var databaseUrl = System.Configuration.ConfigurationManager.AppSettings["Database.Url"]?.ToString();

                return databaseUrl;
            }
        }

        public static string DatabaseNome
        {
            get
            {
                var databaseNome = System.Configuration.ConfigurationManager.AppSettings["Database.Nome"]?.ToString();

                return databaseNome;
            }
        }

        public static string DatabaseUsuario
        {
            get
            {
                var databaseUsuario = System.Configuration.ConfigurationManager.AppSettings["Database.Usuario"]?.ToString();

                return databaseUsuario;
            }
        }

        public static string DatabaseSenha
        {
            get
            {
                var databaseSenha = System.Configuration.ConfigurationManager.AppSettings["Database.Senha"]?.ToString();

                return databaseSenha;
            }
        }

        protected string _connectionString { get; set; }

        public virtual string ObterQueryString(DATABASE_TIPO_ID databaseTipoId)
        {
            var connectionString = string.Empty;

            switch (databaseTipoId)
            {
                case DATABASE_TIPO_ID.MSSQL:
                    connectionString = string.Format("Provider=SQLNCLI11;Server={0};Database={1};Uid={2};Pwd={3};", _BaseItem.DatabaseUrl, _BaseItem.DatabaseNome, _BaseItem.DatabaseUsuario, _BaseItem.DatabaseSenha);
                    break;

                case DATABASE_TIPO_ID.ORACLE:
                    break;

                case DATABASE_TIPO_ID.MYSQL:
                    connectionString = string.Format("Server={0};Port=3306;Database={1};Uid={2};Pwd={3};SslMode=none;", _BaseItem.DatabaseUrl, _BaseItem.DatabaseNome, _BaseItem.DatabaseUsuario, _BaseItem.DatabaseSenha);
                    break;
            }

            return connectionString;
        }
    }
}
