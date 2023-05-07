using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegetation
{
    public class Bridge : MonoBehaviour
    {
        private Controller controller;
        [SerializeField] private float width = 1f;
        [SerializeField] private float heigth = 0.5f;

        private Vector3 startPos;
        private Vector3 finalPos;

        public void Generate(Controller controller, Vector3 startPos, Vector3 finalPos)
        {
            this.controller = controller;
            this.startPos = startPos;
            this.finalPos = finalPos;

            GenerateMesh();
            StartCoroutine(CreateFoliage());
        }

        private void GenerateMesh()
        {
            //
            List<Vector3> vertex = new List<Vector3>();
            List<int> tris = new List<int>();
            Vector3 x = Vector3.Cross(Vector3.up, finalPos - startPos).normalized;
            Vector3 z = Vector3.Cross(Vector3.up, x).normalized;
            //
            vertex.Add(startPos - finalPos + x * width);
            vertex.Add(startPos - finalPos + -x * width);
            vertex.Add(x * width);
            vertex.Add(-x * width);
            int frst = vertex.Count - 4;
            CreatePlane(frst + 0, frst + 1, frst + 2, frst + 3, false, ref tris);
            //
            vertex.Add(startPos - finalPos + x * width + Vector3.down * heigth);
            vertex.Add(startPos - finalPos + -x * width + Vector3.down * heigth);
            vertex.Add(x * width + Vector3.down * heigth);
            vertex.Add(-x * width + Vector3.down * heigth);
            frst = vertex.Count - 4;
            CreatePlane(frst + 0, frst + 1, frst + 2, frst + 3, true, ref tris);
            //
            frst = vertex.Count - 8;
            CreatePlane(frst + 4, frst + 0, frst + 5, frst + 1, false, ref tris);
            CreatePlane(frst + 5, frst + 1, frst + 7, frst + 3, false, ref tris);
            CreatePlane(frst + 7, frst + 3, frst + 6, frst + 2, false, ref tris);
            CreatePlane(frst + 6, frst + 2, frst + 4, frst + 0, false, ref tris);
            //
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.Clear();
            mesh.SetVertices(vertex);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            GetComponent<MeshCollider>().sharedMesh = mesh;

        }

        private void CreatePlane(int a, int b, int c, int d, bool inverse, ref List<int> tris)
        {
            if (!inverse)
            {
                tris.Add(a);
                tris.Add(b);
                tris.Add(c);

                tris.Add(d);
                tris.Add(c);
                tris.Add(b);
            }
            else
            {
                tris.Add(a);
                tris.Add(c);
                tris.Add(b);

                tris.Add(d);
                tris.Add(b);
                tris.Add(c);
            }
        }

        private IEnumerator CreateFoliage()
        {
            float dist = Vector3.Distance(startPos, finalPos);
            Vector3 dir = (startPos - finalPos).normalized;
            float step = 5f;
            int max = (int)(dist / step);
            float timeStep = .17f;
            for (int i = 0; i < max + 1; i++)
            {
                yield return new WaitForSeconds(timeStep);
                Vector3 pos = finalPos + (dir * (step * i));
                controller.CreatePlant(0, 1f, false, pos, Vector3.zero, true);
                
            }
        }

        
    }
}
