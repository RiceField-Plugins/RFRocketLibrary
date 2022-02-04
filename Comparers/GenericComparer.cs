using System;
using System.Collections.Generic;
using Rocket.Unturned.Player;

namespace RFRocketLibrary.Comparers
{
    public class GenericComparer<T> : IEqualityComparer<T> where T : class
    {
        private Func<T, object> Expr { get; }

        public GenericComparer(Func<T, object> expr)
        {
            Expr = expr;
        }

        public bool Equals(T x, T y)
        {
            var first = Expr.Invoke(x);
            var sec = Expr.Invoke(y);
            return first != null && first.Equals(sec);
        }

        public int GetHashCode(T obj)
        {
            return Expr.Invoke(obj).GetHashCode();
        }
    }
}