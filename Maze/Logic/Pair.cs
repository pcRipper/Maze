using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Logic
{
    [Serializable]
    public class Pair<First, Second>
        where First : notnull
        where Second : notnull
    {
        public First first;
        public Second second;

        public Pair()
        {

        }

        public Pair(First first = default(First), Second second = default(Second))
        {
            this.first = first;
            this.second = second;
        }

        public Pair(Pair<First, Second> pair)
        {
            this.first = typeof(First).IsClass ? (First)Activator.CreateInstance(typeof(First), pair.first) : pair.first;
            this.second = typeof(Second).IsClass ? (Second)Activator.CreateInstance(typeof(Second), pair.second) : pair.second;
        }
        public static bool operator == (Pair<First,Second> l, Pair<First,Second> r)
        {
            return l.first.Equals(r.first) && l.second.Equals(r.second);
        }

        public static bool operator !=(Pair<First, Second> l, Pair<First, Second> r)
        {
            return !(l == r);
        }

        public override int GetHashCode()
        {
            int l = first.GetHashCode();
            int r = second.GetHashCode();
            return (l ^ r);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Pair<First, Second>) return false;
            Pair<First,Second> asPair = obj as Pair<First,Second>;
            return asPair.first.Equals(this.first) && asPair.second.Equals(this.second);
        }

        public override string ToString()
        {
            return $"({first.ToString()},{second.ToString()})";
        }
    }
}
