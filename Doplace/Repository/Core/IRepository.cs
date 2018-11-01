using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doplace.Repository.Core
{
    interface IRepository
    {
        void Execute(string sql, object param);

        IEnumerable<TReturn> Query<TReturn>(string sql, object param);

        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, object param, Func<TFirst, TSecond, TReturn> func);

        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, object param, Func<TFirst, TSecond, TThird, TReturn> func);

        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, object param, Func<TFirst, TSecond, TThird, TFourth, TReturn> func);

        IEnumerable<TReturn> Query<TReturn>(string sql, object param, Type[] types, Func<object[], TReturn> func);
    }
}
