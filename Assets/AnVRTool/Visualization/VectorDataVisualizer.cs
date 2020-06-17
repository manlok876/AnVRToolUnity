using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.Rendering;

namespace AnVRTool
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class VectorDataVisualizer : MonoBehaviour
    {
        private MeshFilter meshFilter = null;
        new private MeshRenderer renderer = null;

        private AnVRSessionData session = null;
        private Vector3[] data = null;

        public float cellRadius = 0.05f;
        // AABB
        public Vector3 areaStart = new Vector3();
        public Vector3 areaEnd = new Vector3();
        private int[,,] bins = null;
        private Vector3Int cellCount = new Vector3Int();

        public float redThreshold = 2f;

        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            renderer = GetComponent<MeshRenderer>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                VisualizeData("Gaze Pointer");
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                VisualizeData("Left_DefaultControllerPointer(Clone)"); // PokePointer, GrabPointer
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                VisualizeData("Right_DefaultControllerPointer(Clone)");
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                meshFilter.mesh = null;
            }
        }

        void VisualizeData(string trackName)
        {
            if (session == null)
            {
                GetSessionFromCollector();
            }
            if (session == null)
            {
                Debug.Log("Could not get session from collector");
                return;
            }

            GetDataFromSession(trackName);
            Debug.Log("Time before clustering: " + Time.realtimeSinceStartup);
            ClusterizeData();
            Debug.Log("Time after clustering: " + Time.realtimeSinceStartup);
            meshFilter.mesh = GetBinsMesh();
            Debug.Log("Time after mesh: " + Time.realtimeSinceStartup);
        }

        void GetSessionFromCollector()
        {
            AnVRDataCollector collector = FindObjectOfType<AnVRDataCollector>();
            if (collector == null)
            {
                Debug.Log("AnVR Data Collector not found");
                return;
            }

            session = collector.sessionData;
            if (session == null)
            {
                Debug.Log("Session object is null");
            }
        }

        Vector3[] AggregateTrackData(List<Tuple<float, Vector3>> trackData)
        {
            Vector3[] result = new Vector3[trackData.Count];

            for (int i = 0; i < trackData.Count; i++)
            {
                result[i] = trackData[i].Item2;
            }

            return result;
        }

        void GetDataFromSession(string trackName)
        {
            data = AggregateTrackData(session.GetVectorTrackData(trackName));
        }

        void ClusterizeData()
        {
            Vector3 cells = (areaEnd - areaStart) / (cellRadius * 2);
            cellCount = new Vector3Int((int)cells.x, (int)cells.y, (int)cells.z);
            bins = new int[cellCount.x, cellCount.y, cellCount.z];

            foreach (Vector3 point in data)
            {
                if (!IsInbound(point))
                {
                    continue;
                }
                Vector3 relativePoint = (point - areaStart) / (cellRadius * 2);
                bins[(int)relativePoint.x, (int)relativePoint.y, (int)relativePoint.z]++;
            }
        }

        Mesh GetBinsMesh()
        {
            int nextVertex = 0;
            List<Vector3> vertices = new List<Vector3>();
            List<Color> vertexColors = new List<Color>();
            List<int> triangles = new List<int>();

            for (int idx_x = 0; idx_x < bins.GetLength(0); idx_x++)
            {
                for (int idx_y = 0; idx_y < bins.GetLength(1); idx_y++)
                {
                    for (int idx_z = 0; idx_z < bins.GetLength(2); idx_z++)
                    {
                        if (bins[idx_x, idx_y, idx_z] == 0)
                        {
                            continue;
                        }
                        Vector3[] cellVertices;
                        Color[] cellVertexColors;
                        GetCellVertices(
                            GetCellCenter(idx_x, idx_y, idx_z),
                            bins[idx_x, idx_y, idx_z], 
                            out cellVertices, out cellVertexColors);

                        if (cellVertices.Length > 0)
                        {
                            int[] cellTriangles;
                            GetCubeTriangles(nextVertex, out cellTriangles);

                            vertices.AddRange(cellVertices);
                            vertexColors.AddRange(cellVertexColors);
                            triangles.AddRange(cellTriangles);

                            nextVertex += cellVertices.Length;
                        }
                    }
                }
            }

            Mesh result = new Mesh();
            result.vertices = vertices.ToArray();
            result.triangles = triangles.ToArray();
            result.colors = vertexColors.ToArray();

            return result;
        }

        Vector3 GetCellCenter(int x, int y, int z)
        {
            return areaStart + 2 * cellRadius * new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);
        }

        void GetCellVertices(Vector3 center, int binSize, out Vector3[] vertices, out Color[] vertexColors)
        {
            if (binSize == 0)
            {
                vertices = new Vector3[0];
                vertexColors = new Color[0];
                return;
            }

            vertices = new Vector3[8];
            vertices[0] = center + cellRadius * new Vector3(-1, -1, -1);
            vertices[1] = center + cellRadius * new Vector3(-1, -1,  1);
            vertices[2] = center + cellRadius * new Vector3(-1,  1, -1);
            vertices[3] = center + cellRadius * new Vector3(-1,  1,  1);

            vertices[4] = center + cellRadius * new Vector3( 1, -1, -1);
            vertices[5] = center + cellRadius * new Vector3( 1, -1,  1);
            vertices[6] = center + cellRadius * new Vector3( 1,  1, -1);
            vertices[7] = center + cellRadius * new Vector3( 1,  1,  1);

            Color cellColor = GetCellColor(binSize);
            vertexColors = new Color[8];
            for (int i = 0; i < 8; i++)
            {
                vertexColors[i] = cellColor;
            }
        }

        void GetCubeTriangles(int first, out int[] triangles)
        {
            triangles = new int[36];
            triangles[ 0] = first    ; triangles[ 1] = first + 1; triangles[ 2] = first + 3;
            triangles[ 3] = first    ; triangles[ 4] = first + 3; triangles[ 5] = first + 2;

            triangles[ 6] = first + 1; triangles[ 7] = first + 5; triangles[ 8] = first + 7;
            triangles[ 9] = first + 1; triangles[10] = first + 7; triangles[11] = first + 3;

            triangles[12] = first + 5; triangles[13] = first + 4; triangles[14] = first + 6;
            triangles[15] = first + 5; triangles[16] = first + 6; triangles[17] = first + 7;

            triangles[18] = first    ; triangles[19] = first + 4; triangles[20] = first + 5;
            triangles[21] = first    ; triangles[22] = first + 5; triangles[23] = first + 1;

            triangles[24] = first    ; triangles[25] = first + 2; triangles[26] = first + 6;
            triangles[27] = first    ; triangles[28] = first + 6; triangles[29] = first + 4;

            triangles[30] = first + 2; triangles[31] = first + 3; triangles[32] = first + 7;
            triangles[33] = first + 2; triangles[34] = first + 7; triangles[35] = first + 6;
        }

        Color GetCellColor(int binSize)
        {
            Color result = Color.Lerp(Color.green, Color.red, binSize / redThreshold);
            return result;
        }

        bool IsInbound(Vector3 point)
        {
            bool result = true;
            result = result && point.x <= areaEnd.x && point.x >= areaStart.x;
            result = result && point.y <= areaEnd.y && point.y >= areaStart.y;
            result = result && point.z <= areaEnd.z && point.z >= areaStart.z;
            return result;
        }
    }
}
