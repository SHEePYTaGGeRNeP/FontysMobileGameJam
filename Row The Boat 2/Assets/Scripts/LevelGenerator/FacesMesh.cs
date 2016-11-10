using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.LevelGenerator
{
    class FacesMesh
    {
        #region "Fields"

        private List<Vector3> vertices;
        private List<int> faces;
        private int pointsPerLine;

        #endregion

        #region "Constructors"

        public FacesMesh(int pointsPerLine)
        {
            vertices = new List<Vector3>();
            faces = new List<int>();
            this.pointsPerLine = pointsPerLine;
        }

        #endregion

        #region "Properties"

        public Vector3[] Vertices
        {
            get { return vertices.ToArray(); }
        }

        public int[] Faces
        {
            get { return faces.ToArray(); }
        }

        #endregion

        #region "Methods"

        public void AddLine(Vector3[] points)
        {
            if (points.Length < pointsPerLine)
                return;

            vertices.AddRange(points);
            int startpointNew = vertices.Count - pointsPerLine;
            int startpointOld = vertices.Count - (pointsPerLine * 2);

            if (startpointOld < 0)
                return;

            for (int i = 0; i < pointsPerLine - 1; i++)
            {
                faces.AddRange(new int[] {
                    startpointNew + i,
                    startpointOld + 1 + i,
                    startpointOld + i,

                    startpointNew + i,
                    startpointNew + 1 + i,
                    startpointOld + 1 + i
                });
            }

        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"



        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
