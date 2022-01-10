using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoSI2.mapper
{
    public interface IMapper<T,Tid>
    {
        T Create(T entity);
        T Read(Tid id);
        T Update(T entity);
        T Delete(T entity);
    }
}
