using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    class SRandom
    {
        private int seed;
        private static SRandom instance;
        private int id;

        public SRandom(int seed)
        {
            this.seed = seed;
            instance = this;
            this.id = 0;


        }

        public static SRandom Get(int seed)
        {
            if (instance == null)
            {
                return new SRandom(seed);
            }
            else if (seed == instance.seed)
            {
                return instance;
            }
            else
                return new SRandom(seed);
        }

        public float Random(float min, float max)
        {
            id++;
            float diff = max * 1000 - min * 1000;

            string num = "";
            for (int i = 0; i < 32; i++)
            {
                num += (id * seed + seed + id + i) % 2;
            }



            float number = Convert.ToInt32(num, 2);


            number = min + (number % diff);

            Debug.Log(number);
            return number / 1000;
        }

        public int Random(int min, int max)
        {
            return Mathf.RoundToInt(Random(min, max));
        }
    }
}
