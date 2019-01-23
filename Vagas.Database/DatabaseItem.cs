using System.Data;

namespace Vagas.Database
{
    public class DatabaseItem : Base._BaseItem, Interface.IDatabase
    {
        private Interface.IDatabase _databaseItem = null;

        public DatabaseItem()
        {
            switch (DatabaseTipoId)
            {
                case Base.DATABASE_TIPO_ID.MSSQL:
                    this._databaseItem = new SqlServer.SqlServerDatabase();
                    break;
            }

        }

        public DataSet ExecutarRetornandoDataSet(string sql)
        {
            return _databaseItem.ExecutarRetornandoDataSet(sql);
        }

        public DataTable ExecutarRetornandoDataTable(string sql)
        {
            return _databaseItem.ExecutarRetornandoDataTable(sql);
        }

        public void ExecutarSemRetorno(string sql)
        {
            _databaseItem.ExecutarSemRetorno(sql);
        }
    }
}
