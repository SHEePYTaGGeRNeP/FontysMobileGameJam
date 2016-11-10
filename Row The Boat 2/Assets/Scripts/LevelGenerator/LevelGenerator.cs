using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.LevelGenerator
{
    class LevelGenerator : BaseClass<LevelGenerator>
    {
        #region "Fields"

        public int LevelSeed = 12345;

        private Level current;

        public  GameObject RiverCollider;

        #endregion

        #region "Constructors"



        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        public void GenerateLevel(int seed)
        {
            PRNG.ChangeSeed(seed);

            current = new Level();
            current.Generate();
            current.Draw();
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"

        public void Start()
        {
            GenerateLevel(LevelSeed);
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
