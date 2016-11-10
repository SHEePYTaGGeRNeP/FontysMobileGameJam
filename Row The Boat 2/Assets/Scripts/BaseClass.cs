using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The bassclass for a MonoBehaviour script
    /// </summary>
    /// <typeparam name="T">The type of the script that use this baseclass</typeparam>
    abstract class BaseClass<T> : MonoBehaviour where T : BaseClass<T>
    {
        #region "Fields"

        protected static T _instance;

        #endregion

        #region "Constructors"



        #endregion

        #region "Properties"

        /// <summary>
        /// Gets the current instance of the object
        /// </summary>
        public static T INSTANCE
        {
            get { return _instance; }
        }

        #endregion

        #region "Methods"



        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"

        /// <summary>
        /// The Unity Engine awake method.
        /// This method can be overwritten BUT THE base.Awake() MUST ALWAYS BE CALLED BECAUSE OF THE SINGLETON INSTANCE
        /// </summary>
        public virtual void Awake()
        {
            _instance = (T)Convert.ChangeType(this, typeof(T));
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
