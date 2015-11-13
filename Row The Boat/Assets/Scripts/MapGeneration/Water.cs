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
            Vector3[] vertices = mesh.vertices;

            if (lerpValue >= 1)
            {
                lerpValue = 0;
                startPosition = nextPosition;
                nextPosition = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                
            }
            
            vertices[4] = Vector3.Lerp(startPosition, nextPosition, lerpValue);

            mesh.vertices = vertices;
            lerpValue += 0.1f;
        }
    }
}
