using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Vegetation
{
    public class Generator : MonoBehaviour
    {
        [System.Serializable]
        private class ControlPoint
        {
            public string id;
            [HideInInspector] public Vector3 pos;
            [HideInInspector] public Vector3 normal;
            public List<ControlPoint> next;
            public List<int> vertex;
            public bool isBranchEnd;

            public ControlPoint(int id, Vector3 pos, Vector3 normal)
            {
                this.id = "point" + id;
                this.pos = pos;
                this.normal = normal.normalized;
                next = new List<ControlPoint>();
                vertex = new List<int>();
            }
            public void addQuad(int vertex)
            {
                this.vertex.Add(vertex + 0);
                this.vertex.Add(vertex + 1);
                this.vertex.Add(vertex + 2);
                this.vertex.Add(vertex + 3);
            }
        }
        private List<ControlPoint> controlPoints;
        private Plant plant;
        private float size;
        private bool explode;
        private bool isGrowing;
        private bool climbable;
        [SerializeField] private LayerMask layer;

        public void Generate(Plant plant, float size, bool climbable)
        {
            this.plant = plant;
            this.size = size;
            this.climbable = climbable;
            controlPoints = new List<ControlPoint>();
            
            StartGenerate(transform.position, 
                transform.up);
        }

        private void StartGenerate(Vector3 startPos, Vector3 startNormal)
        {
            RaycastHit hit;
            Debug.DrawRay(startPos + startNormal, -startNormal * (3f + 0.5f), Color.green, 10f);
            if (Physics.Raycast(startPos + startNormal, -startNormal, out hit, 
                3f + 0.5f, layer))
            {
                transform.position = hit.point;
                startPos = hit.point;
                startNormal = hit.normal.normalized;
                //transform.rotation =
                    //Quaternion.FromToRotation(Vector3.up, hit.normal.normalized);
                transform.localEulerAngles = Vector3.zero;
            }
            else
            {
                Debug.Log("no start ray");
                Destroy(this.gameObject);
                
                return;
            }
            controlPoints.Add(new ControlPoint(0, startPos, startNormal));

            Vector3 startDir = RandomDirection(startNormal);
            float angle = 4f / Mathf.RoundToInt(plant.trunkBranchNumber);

            for (int i = 0; i < Mathf.RoundToInt(plant.trunkBranchNumber); i++)
            {
                Vector3 dir = RotateDirection(startNormal, startDir, angle * i);
                ControlPoint startPoint = controlPoints[0];

                for (int j = 0; j < Mathf.RoundToInt(plant.trunkNumber * size); j++)
                {
                    Debug.DrawRay(startPoint.pos, dir, Color.blue, 10f);
                    GrowStep(ref startPoint, ref dir);
                }

            }

            CreateTrunk(startPos);
            CreateFoliage(startPos);
        }

        private void GrowStep(ref ControlPoint curPoint, ref Vector3 curDir)
        {

            // inizialize
            RaycastHit hit;
            float portionLength = 0.5f;
            Vector3 p0 = curPoint.pos;
            Vector3 p1 = p0 + (curPoint.normal * portionLength / 2f);
            Vector3 dir = RandomAngleDirection(curPoint.normal, curDir, 2f / Mathf.RoundToInt(plant.trunkBranchNumber), 1f);
            Debug.DrawRay(p0, dir * portionLength, Color.cyan, 10f);

            if (Physics.Raycast(p1, dir, out hit, portionLength, layer))
            {
                //Debug.Log("hit: p1 - p2");
                Debug.DrawRay(p1, dir * portionLength, Color.green, 10f);

                // there's an obstacle -> create new one
                curPoint.next.Add(new ControlPoint(controlPoints.Count, hit.point, hit.normal));
                controlPoints.Add(curPoint.next[curPoint.next.Count - 1]);
                curDir = curPoint.normal;
            }
            else
            {

                //Debug.Log("no hit: p1 - p2");
                Debug.DrawRay(p1, dir * portionLength, Color.red, 10f);
                // no obstacle, check below
                Vector3 p2 = p1 + (dir * portionLength);
                if (Physics.Raycast(p2, -curPoint.normal, out hit, portionLength, layer))
                {
                    float dist = Vector3.Distance(hit.point, p2);
                    // below there's something
                    if (dist < portionLength / 2f)
                    {
                        //Debug.Log("hit short: p2 - p3");
                        Debug.DrawRay(p2, -curPoint.normal * portionLength, Color.green, 10f);
                        // above start point
                        curPoint.next.Add(new ControlPoint(controlPoints.Count,
                            (hit.point + p1 + p0) / 3f, p1 - (hit.point + p0) / 2f));
                        controlPoints.Add(curPoint.next[curPoint.next.Count - 1]);
                        //
                        curPoint.next[curPoint.next.Count - 1].next.Add(new ControlPoint(controlPoints.Count, hit.point, hit.normal));
                        controlPoints.Add(curPoint.next[curPoint.next.Count - 1].next[curPoint.next[curPoint.next.Count - 1].next.Count - 1]);
                        curDir = dir;

                    }
                    else if (dist > portionLength / 2f)
                    {
                        //Debug.Log("hit long: p2 - p3");
                        Debug.DrawRay(p2, -curPoint.normal * portionLength, Color.green, 10f);
                        // below start point
                        curPoint.next.Add(new ControlPoint(controlPoints.Count,
                            (p0 + hit.point + p2) / 3f,
                            p2 - ((p0 + hit.point) / 2f)));
                        controlPoints.Add(curPoint.next[curPoint.next.Count - 1]);
                        //
                        curPoint.next[curPoint.next.Count - 1].next.Add(new ControlPoint(controlPoints.Count, hit.point, hit.normal));
                        controlPoints.Add(curPoint.next[curPoint.next.Count - 1].next[curPoint.next[curPoint.next.Count - 1].next.Count - 1]);
                        curDir = dir;
                    }
                    else
                    {
                        //Debug.Log("hit: p2 - p3");
                        Debug.DrawRay(p2, -curPoint.normal * portionLength, Color.green, 10f);
                        curPoint.next.Add(new ControlPoint(controlPoints.Count, hit.point, hit.normal));
                        controlPoints.Add(curPoint.next[curPoint.next.Count - 1]);
                        curDir = dir;
                    }
                }
                else
                {
                    //Debug.Log("no hit: p2 - p3");
                    Debug.DrawRay(p2, -curPoint.normal * portionLength, Color.red, 10f);
                    // no  obstacle below, check back
                    Vector3 p3 = p2 + (-curPoint.normal * portionLength);
                    if (Physics.Raycast(p3, -dir, out hit, portionLength, layer))
                    {
                        //Debug.Log("hit: p3 - p4");
                        Debug.DrawRay(p3, -dir * portionLength, Color.green, 10f);
                        // back there's something
                        // TO DO: not perfect with edges
                        float x = portionLength - Vector3.Distance(p3, hit.point);
                        Vector3 pi = p0 + dir * x;
                        curPoint.next.Add(new ControlPoint(controlPoints.Count,
                            (pi + (pi + (p2 - pi) * (Vector3.Distance(pi, p2) * portionLength))) / 2f,
                            p2 - ((p0 + hit.point) / 2f)));
                        //curPoint.next.Add(new ControlPoint(controlPoints.Count, 
                        //(p0 + hit.point + p2) / 3f, p2 - ((p0 + hit.point) / 2f)));
                        controlPoints.Add(curPoint.next[curPoint.next.Count - 1]);
                        //
                        curPoint.next[curPoint.next.Count - 1].next.Add(new ControlPoint(controlPoints.Count, hit.point, hit.normal));
                        controlPoints.Add(curPoint.next[curPoint.next.Count - 1].next[curPoint.next[curPoint.next.Count - 1].next.Count - 1]);
                        curDir = -curPoint.normal;
                    }
                    else
                    {
                        //Debug.Log("no hit: p3 - p4");
                        Debug.DrawRay(p3, -dir * portionLength, Color.red, 10f);
                        return;
                        // no obstacle back, check up
                        Vector3 p4 = p3 + (-dir * portionLength);
                        if (Physics.Raycast(p4, curPoint.normal, out hit, portionLength, layer))
                        {
                            //Debug.Log("hit: p4 - p5");
                            // up there's something
                            curPoint.next.Add(new ControlPoint(controlPoints.Count, (p0 + p2) / 2f, curPoint.normal));
                            controlPoints.Add(curPoint.next[curPoint.next.Count - 1]);
                            curPoint.next.Add(new ControlPoint(controlPoints.Count, ((p2 + hit.point) / 2f + p3) / 2f, p3 - (p1 + hit.point) / 2f));
                            controlPoints.Add(curPoint.next[curPoint.next.Count - 1]);
                            Debug.DrawRay(p3, curPoint.normal * portionLength, Color.green, 1f);
                            curPoint.next.Add(new ControlPoint(controlPoints.Count, hit.point, hit.normal));
                            controlPoints.Add(curPoint.next[curPoint.next.Count - 1]);
                            curDir = -dir;
                        }
                        else
                        {
                            // no obstacle up
                            Debug.Log("no hit: p4 - p5");
                        }
                    }
                }
            }

            curPoint = controlPoints[controlPoints.Count - 1];
        }

        private void CreateTrunk(Vector3 startPos)
        {
            // init
            List<Vector3> vertex = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uv = new List<Vector2>();
            int vertexCount = 0;
            List<Vector3> collisionPoints = new List<Vector3>();
            collisionPoints.Add(controlPoints[0].pos
                + (controlPoints[0].normal * 0.2f) - startPos);
            // get max ivy length
            int realBranchMax = 0;
            for (int i = 0, count = 0; i < controlPoints.Count; i++)
            {

                if (controlPoints[i].next.Count > 0)
                {
                    count++;
                }
                else
                {
                    if (count > realBranchMax) realBranchMax = count;
                    count = 0;
                }
            }
            int branchCount = 0;
            float ivySize = 0.05f;
            // loop
            for (int i = 0; i < controlPoints.Count; i++)
            {
                for (int j = 0; j < controlPoints[i].next.Count; j++)
                {
                    // init
                    Vector3 _dir = controlPoints[i].next[j].pos - controlPoints[i].pos;
                    Vector3 _wideDir = Vector3.Cross(controlPoints[i].normal, _dir).normalized;

                    // add triangles
                    float joinOffset = 0f;
                    float dist = Vector3.Distance(controlPoints[i].pos, controlPoints[i].next[j].pos);
                    if (dist > 2 * ivySize)
                    {
                        joinOffset = ivySize;
                        // add join quad to next.pos
                    }
                    vertex.Add(controlPoints[i].pos + (_wideDir * ivySize)
                        + controlPoints[i].normal * ivySize * 0.1f
                        + (_dir * joinOffset)
                        - startPos);
                    vertex.Add(controlPoints[i].pos + (-_wideDir * ivySize)
                        + controlPoints[i].normal * ivySize * 0.1f
                        + (_dir * joinOffset)
                        - startPos);
                    if (controlPoints[i].next[j].next.Count == 0) _wideDir = Vector3.zero;
                    vertex.Add(controlPoints[i].next[j].pos + (_wideDir * ivySize)
                        + controlPoints[i].normal * ivySize * 0.1f
                        + (-_dir * joinOffset)
                        - startPos);
                    vertex.Add(controlPoints[i].next[j].pos + (-_wideDir * ivySize)
                        + controlPoints[i].normal * ivySize * 0.1f
                        + (-_dir * joinOffset)
                        - startPos);
                    // add clockwise 0 1 2
                    tris.Add(vertexCount + 0);
                    tris.Add(vertexCount + 1);
                    tris.Add(vertexCount + 2);
                    // add clockwise 3 2 1
                    tris.Add(vertexCount + 3);
                    tris.Add(vertexCount + 2);
                    tris.Add(vertexCount + 1);
                    // uv map same number of vertex
                    uv.Add(new Vector2((float)(branchCount) / (float)(realBranchMax), 0f));
                    uv.Add(new Vector2((float)(branchCount) / (float)(realBranchMax), 1f));
                    uv.Add(new Vector2((float)(branchCount + 1) / (float)(realBranchMax), 0f));
                    uv.Add(new Vector2((float)(branchCount + 1) / (float)(realBranchMax), 1f));
                    //
                    controlPoints[i].addQuad(vertexCount);
                    vertexCount += 4;
                    // foliage
                }
                if (controlPoints[i].next.Count > 0)
                {
                    //CheckBound(ref bounds, controlPoints[i].pos);
                    branchCount++;
                }
                else
                {
                    controlPoints[i].isBranchEnd = true;
                    if (Vector3.Dot(controlPoints[i].normal, Vector3.up) < 0.5f || controlPoints[i].pos.y >= controlPoints[0].pos.y)
                        collisionPoints.Add(controlPoints[i].pos
                        + (controlPoints[i].normal * 0.2f) - startPos);
                    branchCount = 0;
                }
            }
            // create join triangles
            for (int i = 0; i < controlPoints.Count; i++)
            {
                for (int j = 0; j < controlPoints[i].next.Count; j++)
                {
                    if (controlPoints[i].next[j].vertex.Count > 0)
                    {
                        // add 2 3 4
                        tris.Add(controlPoints[i].vertex[2 + (j * 4)]);
                        tris.Add(controlPoints[i].vertex[3 + (j * 4)]);
                        tris.Add(controlPoints[i].next[j].vertex[0]);
                        // add 5 4 3
                        tris.Add(controlPoints[i].next[j].vertex[1]);
                        tris.Add(controlPoints[i].next[j].vertex[0]);
                        tris.Add(controlPoints[i].vertex[3 + (j * 4)]);
                    }
                }
            }
            if (climbable && Vector3.Dot(controlPoints[0].normal, Vector3.up) < 0.5f)
            {
                CreateCollisions(collisionPoints.ToArray());
            } else
            {
                GetComponentInChildren<Collider>().enabled = false;
            }
            // assign
            GetComponent<Renderer>().enabled = false;
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.Clear();
            mesh.SetVertices(vertex);
            mesh.SetUVs(0, uv);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            GetComponent<Renderer>().material.SetColor("_Color", plant.trunkColor);
            GetComponent<Renderer>().material.SetFloat("_Grow", 0f);
            GetComponent<Renderer>().enabled = true;
            StartCoroutine(GrowVine(GetComponent<Renderer>().material, 0f));
        }
        private void CheckBound(ref float[] bound, Vector3 point)
        {
            if (bound == null)
            {
                bound = new float[6];
                bound[0] = point.x;
                bound[1] = point.x;
                bound[2] = point.y;
                bound[3] = point.y;
                bound[4] = point.z;
                bound[5] = point.z;
            }
            if (point.x > bound[0]) bound[0] = point.x;
            else if (point.x < bound[1]) bound[1] = point.x;
            if (point.y > bound[2]) bound[2] = point.y;
            else if (point.y < bound[3]) bound[3] = point.y;
            if (point.z > bound[4]) bound[4] = point.z;
            else if (point.z < bound[5]) bound[5] = point.z;
        }

        private void CreateCollisions(Vector3[] vertex)
        {
            /*transform.GetChild(1).rotation =
                Quaternion.FromToRotation(Vector3.up, controlPoints[0].normal);
            transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(
                Mathf.Abs(bounds[0] - bounds[1]),
                .5f,
                Mathf.Abs(bounds[4] - bounds[5])
            );
            transform.GetChild(1).GetComponent<BoxCollider>().center = new Vector3(
                ((bounds[0] + bounds[1]) / 2),
                0f,
                ((bounds[4] + bounds[5]) / 2)
            );*/
            List<int> tris = new List<int>();
            for (int i = 1; i < vertex.Length; i++)
            {
                int next = i + 1;
                if (next >= vertex.Length) next = 1;
                tris.Add(0);
                tris.Add(i);
                tris.Add(next);
            }
            Mesh mesh = transform.GetChild(1).GetComponent<MeshFilter>().mesh;
            mesh.Clear();
            mesh.SetVertices(vertex);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            transform.GetChild(1).GetComponent<MeshCollider>().sharedMesh = mesh;
            transform.GetChild(1).GetComponent<MeshCollider>().enabled = true;

        }

        private void CreateFoliage(Vector3 startPos)
        {
            float[] maxDistance = new float[plant.foliages.Count];
            List<GameObject>[] instances = new List<GameObject>[plant.foliages.Count];

            for (int i = 0; i < controlPoints.Count; i++)
            {
                for (int j = 0; j < controlPoints[i].next.Count; j++)
                {
                    // axis
                    Vector3 pos0 = controlPoints[i].pos;
                    Vector3 pos1 = controlPoints[i].next[j].pos;
                    Vector3 z = (pos1 - pos0).normalized;
                    Vector3 y = controlPoints[i].normal;
                    Vector3 x = Vector3.Cross(y, z).normalized;
                    int foliagePerTrunk = Random.Range(1,3);
                    if (0 == i && j % 2 == 1) foliagePerTrunk = 0;
                    for (int k = 0; k < foliagePerTrunk; k++)
                    {
                        // choose with probabilty
                        float randomValue = Random.value;
                        int f = Random.Range(0, plant.foliages.Count);
                        while (randomValue >
                            plant.foliages[f].p)
                        {
                            randomValue = Random.value;
                            f = Random.Range(0, plant.foliages.Count);
                        }
                        // init

                        // create
                        if (instances[f] == null) instances[f] = new List<GameObject>();
                        
                        instances[f].Add(
                            Instantiate(plant.foliages[f].prefab,
                            transform));

                        Vector3 pos = pos0 + z * (Vector3.Distance(pos0, pos1) / foliagePerTrunk * k);
                        if (Vector3.Distance(controlPoints[i].pos, startPos) > maxDistance[f])
                            maxDistance[f] = Vector3.Distance(controlPoints[i].pos, startPos);

                        instances[f][instances[f].Count - 1]
                            .transform.localPosition = pos - startPos + y * plant.foliages[f].size * size * 0.08f;
                        instances[f][instances[f].Count - 1]
                            .transform.rotation = Quaternion.LookRotation(z, y);
                        instances[f][instances[f].Count - 1]
                            .transform.localScale = Vector3.one * plant.foliages[f].size * 0.6f * Random.Range(0.8f, 1.4f);
                    }
                }
            }
            // create meshes for each foliage
            for (int i = 0; i < instances.Length; i++)
            {
                //SetQuad();
                // init
                List<MeshFilter> meshFilters = new List<MeshFilter>();
                for (int k = 0; k < instances[i].Count; k++)
                {
                    meshFilters.Add(instances[i][k]
                        .GetComponentInChildren<MeshFilter>());
                }
                CombineInstance[] combine = new CombineInstance[meshFilters.Count];
                // combine
                int j = 0;
                while (j < meshFilters.Count)
                {
                    combine[j].mesh = meshFilters[j].sharedMesh;
                    combine[j].transform = meshFilters[j].transform.localToWorldMatrix;
                    j++;
                }
                Transform parent = null;
                if (i != 0) parent = Instantiate(transform.GetChild(0).gameObject, transform).transform;
                else parent = transform.GetChild(0);

                parent.GetComponent<Renderer>().material =
                    meshFilters[0].gameObject.GetComponent<Renderer>().material;

                parent.GetComponent<Renderer>().enabled = false;
                parent.GetComponent<MeshFilter>().mesh = new Mesh();
                parent.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
                parent.localPosition = -startPos;
                for (int k = 0; k < instances[i].Count; k++)
                {
                    Destroy(instances[i][k]);
                }
                Material mat = parent.GetComponent<Renderer>().material;
                mat.SetFloat("_Distance", maxDistance[i]*1.1f);
                mat.SetVector("_Origin", startPos);
                mat.SetFloat("_Grow", 0f);
                mat.SetColor("_Color", plant.foliages[i].color);
                mat.SetColor("_OverlayA", plant.foliages[i].overlayA);
                mat.SetColor("_OverlayB", plant.foliages[i].overlayB);
                parent.GetComponent<Renderer>().enabled = true;
                StartCoroutine(GrowVine(mat, .5f));
            }
        }


        private Vector3 RandomAngleDirection(Vector3 normal, Vector3 startDir, float angleMin, float angleMax)
        {
            Vector3 _v3 = RandomDirection(normal);
            int max = 0;
            while (!(Vector3.Dot(startDir.normalized, _v3.normalized) > angleMin && Vector3.Dot(startDir.normalized, _v3.normalized) < angleMax))
            {
                max++;
                _v3 = RandomDirection(normal);
                if (max > 10)
                {
                    Debug.Log("max random angle");
                    return _v3;
                }
            }
            return _v3;
        }
        private Vector3 RandomDirection(Vector3 normal)
        {
            Vector3 _v3 = Vector3.zero;
            int max = 0;
            while (_v3 == Vector3.zero)
            {
                _v3 = Vector3.Cross(normal, Random.insideUnitSphere);
                if (max > 10)
                {
                    Debug.Log("max random");
                    return _v3;
                }
            }
            return _v3.normalized;
        }

        private Vector3 RotateDirection(Vector3 normal, Vector3 dir, float angle)
        {
            return Quaternion.AngleAxis(angle * 90f, normal) * dir;
        }

        private IEnumerator GrowVine(Material mat, float delay)
        {
            isGrowing = true;
            yield return new WaitForSeconds(delay);
            float grow = .0f;
            float growTime = 2f * 10f;
            while (grow < 1f)
            {
                grow += 1f / growTime;
                mat.SetFloat("_Grow", grow);
                yield return new WaitForSeconds(1f / growTime);
            }
            isGrowing = false;
            if (explode)
            {
                Explode();
            }
            
        }

        public static string MeshToString(MeshFilter mf)
        {
            Mesh m = mf.mesh;
            Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

            StringBuilder sb = new StringBuilder();

            sb.Append("g ").Append(mf.name).Append("\n");
            foreach (Vector3 v in m.vertices)
            {
                sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
            }
            sb.Append("\n");
            foreach (Vector3 v in m.normals)
            {
                sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
            }
            sb.Append("\n");
            foreach (Vector3 v in m.uv)
            {
                sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
            }
            for (int material = 0; material < m.subMeshCount; material++)
            {
                sb.Append("\n");
                sb.Append("usemtl ").Append(mats[material].name).Append("\n");
                sb.Append("usemap ").Append(mats[material].name).Append("\n");

                int[] triangles = m.GetTriangles(material);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                        triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
                }
            }
            return sb.ToString();
        }

        public static void MeshToFile(MeshFilter mf, string filename)
        {
            //File.WriteAllText("D:\\Unity\\UntitleNatureGame\\UntitleNatureGame\\Assets\\Scripts\\vegetation\\" + filename + ".obj", MeshToString(mf));
        }
    
        public void Explode()
        {
            explode = true;
            if (!isGrowing)
            {
                //GameObject go = Instantiate(plant.explosion, transform.position, Quaternion.identity);
                //Destroy(go, 5f);
                Destroy(this.gameObject);
            }
        }

    }
}