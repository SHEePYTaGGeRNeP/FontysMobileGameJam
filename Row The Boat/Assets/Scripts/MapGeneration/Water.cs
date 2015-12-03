using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    class Water : MonoBehaviour
    {
        public Mesh mesh;
        //public Water left;
        //public Water right;

        private Vector3 nextPosition = new Vector3();
        private Vector3 startPosition = new Vector3();
        private float lerpValue = -1;


        public void Update1()
        {
            Vector3[] vertices = this.mesh.vertices;

            if (this.lerpValue >= 1)
            {
                this.lerpValue = 0;
                this.startPosition = this.nextPosition;
                this.nextPosition = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                
            }
            
            vertices[4] = Vector3.Lerp(this.startPosition, this.nextPosition, this.lerpValue);

            this.mesh.vertices = vertices;
            this.lerpValue += 0.1f;
        }
    }
}
