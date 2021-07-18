using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
    public class ScrabbleBag<T>
    {
        private T[] _all;
        private int _idx;

        public ScrabbleBag(T[] data)
        {
            _all = data;
            Shuffle();
        }

        private void Shuffle()
        {
            // knuth shuffle
            for (int i = _all.Length - 1; i > 0; --i)
            {
                int r = Random.Range(0, i + 1);
                _all.Swap(r, i);
            }
        }

        public T GetRandomElement()
        {
            if (_idx == _all.Length)
            {
                Shuffle();
                _idx = 0;
            }

            return _all[_idx++];
        }
    }
}