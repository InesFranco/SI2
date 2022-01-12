using System.Data.SqlClient;

namespace TrabalhoSI2.dal
{
    public interface IContext : IDisposable
    {
        void Open();
        SqlCommand createCommand();
    }
}
