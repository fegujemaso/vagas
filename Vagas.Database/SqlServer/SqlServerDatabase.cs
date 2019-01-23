namespace Vagas.Database.SqlServer
{
    public class SqlServerDatabase : Base._BaseItem, Interface.IDatabase
    {
        public SqlServerDatabase()
            : this(Base.DATABASE_TIPO_ID.MSSQL)
        { }

        public SqlServerDatabase(Base.DATABASE_TIPO_ID databaseTipoId)
        {
            this._connectionString = this.ObterQueryString(databaseTipoId);
        }

        public SqlServerDatabase(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public virtual System.Data.DataSet ExecutarRetornandoDataSet(string sql)
        {
            var dataSet = new System.Data.DataSet();

            using (var oleDbConnection = new System.Data.SqlClient.SqlConnection(this._connectionString))

            using (var oleDbCommand = new System.Data.SqlClient.SqlCommand(sql, oleDbConnection))

            using (var oleDbDataAdapter = new System.Data.SqlClient.SqlDataAdapter(oleDbCommand))
            {
                oleDbDataAdapter.Fill(dataSet);
            }

            return dataSet;
        }

        public virtual System.Data.DataTable ExecutarRetornandoDataTable(string sql)
        {
            var dataSet = this.ExecutarRetornandoDataSet(sql);

            if (dataSet.Tables.Count > 0)
                return dataSet.Tables[0].Copy();

            return null;
        }

        public virtual void ExecutarSemRetorno(string sql)
        {
            var dataSet = new System.Data.DataSet();

            using (var oleDbConnection = new System.Data.SqlClient.SqlConnection(this._connectionString))
            {
                oleDbConnection.Open();

                using (var oleDbCommand = new System.Data.SqlClient.SqlCommand(sql, oleDbConnection))
                    oleDbCommand.ExecuteNonQuery();
            }
        }
    }
}
