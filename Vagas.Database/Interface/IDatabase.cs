namespace Vagas.Database.Interface
{
    public interface IDatabase
    {
        System.Data.DataSet ExecutarRetornandoDataSet(string sql);

        System.Data.DataTable ExecutarRetornandoDataTable(string sql);

        void ExecutarSemRetorno(string sql);
    }
}
