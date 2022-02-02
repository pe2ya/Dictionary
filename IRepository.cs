using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    public interface IRepository<T> where T : IBaseClass
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Save(T element);
        void Remove(T element);
    }
}
