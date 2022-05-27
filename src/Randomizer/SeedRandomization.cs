using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TunicRandomizer.Stores;

namespace TunicRandomizer.Randomizer
{
    public class SeedRandomizer
    {
        private Random random;
        private int m_seed;

        public int Seed { 
            get { return m_seed; }
            set 
            {
                random = new Random(m_seed);
                m_seed = value; 
            }
        }

        public SeedRandomizer() : this(Environment.TickCount)
        {
        }

        public SeedRandomizer(int seed) 
        {
            random = new Random(seed);
            m_seed = seed;
        }

        public int[] RandomizeList(int listSize)
        {
            List<int> resultRandomMatching = new List<int>();
            List<int> itemsToPickFrom = Enumerable.Range(0, listSize).ToList();
            for (int i = 0; i < listSize; i++)
            {
                int randomIndex = random.Next(0, itemsToPickFrom.Count);
                int randomItem = itemsToPickFrom[randomIndex];
                itemsToPickFrom.Remove(randomItem);

                resultRandomMatching.Add(randomItem);
            }

            return resultRandomMatching.ToArray();
        }
    }
}
