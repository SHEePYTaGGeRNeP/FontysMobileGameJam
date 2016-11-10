using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    class SRandom
    {
        private int _seed;
        private static SRandom instance;
        private int _id;

        public SRandom(int seed)
        {
            this._seed = seed;
            instance = this;
            this._id = 0;


        }

        public static SRandom Get(int seed)
        {
            if (instance == null)
            {
                return new SRandom(seed);
            }
            else if (seed == instance._seed)
            {
                return instance;
            }
            else
                return new SRandom(seed);
        }

        public float Random(float min, float max)
        {
            this._id++;
            float diff = max * 1000 - min * 1000;

            string num = "";
            for (int i = 0; i < 32; i++)
            {
                num += (this._id * this._seed + this._seed + this._id + i) % 2;
            }



            float number = Convert.ToInt32(num, 2);


            number = min + (number % diff);

            //Debug.Log(number);
            return number / 1000;
        }

        public int Random(int min, int max)
        {
            return Mathf.RoundToInt(Random(min, max));
        }
    }
}
