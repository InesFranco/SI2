using System.Data.SqlClient;

namespace TrabalhoSI2.dal
{
    internal interface IContext : IDisposable
    {
        void Open();
        SqlCommand createCommand();
    }
}
