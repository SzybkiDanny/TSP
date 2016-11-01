using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP.Rcl
{
    public class RestrictedCandidateList
    {
        private readonly int _limit;

        private readonly SortedList<int, Candidate> _list =
            new SortedList<int, Candidate>(new DuplicateKeyComparer<int>());

        private readonly Random _random = new Random();

        public RestrictedCandidateList(int limit)
        {
            _limit = limit;
        }

        public void TryAdd(Candidate candidate)
        {
            if (_list.Count < _limit)
            {
                _list.Add(candidate.Delta, candidate);
                return;
            }

            if (candidate.Delta >= _list.Last().Key)
                return;

            _list.RemoveAt(_limit - 1);
            _list.Add(candidate.Delta, candidate);
        }

        public Candidate GetRandomCandidate()
        {
            return _list.ElementAt(_random.Next(_limit)).Value;
        }

        public void Empty()
        {
            _list.Clear();
        }

        private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                var result = x.CompareTo(y);

                return result == 0 ? 1 : result;
            }
        }
    }
}