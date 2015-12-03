using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    class Water : MonoBehaviour
    {
        public Mesh Mesh;
        //public Water left;
        //public Water right;

        private Vector3 _nextPosition = new Vector3();
        private Vector3 _startPosition = new Vector3();
        private float _lerpValue = -1;


        public void Update1()
        {
            Vector3[] vertices = this.Mesh.vertices;

            if (this._lerpValue >= 1)
            {
                this._lerpValue = 0;
                this._startPosition = this._nextPosition;
                this._nextPosition = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                
            }
            
            vertices[4] = Vector3.Lerp(this._startPosition, this._nextPosition, this._lerpValue);

            this.Mesh.vertices = vertices;
            this._lerpValue += 0.1f;
        }
    }
}
