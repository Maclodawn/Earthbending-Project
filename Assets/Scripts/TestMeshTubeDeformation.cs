using UnityEngine;
using System.Collections;

public class TestMeshTubeDeformation : MonoBehaviour
{
    public static void deform(GameObject gameObject)
    {
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        mesh.Clear();

        float height = 0.1f;
        float bottomRadius = .5f;
        float topRadius = .5f;
        int nbSides = 18;

        int nbVerticesCap = nbSides + 1;
        #region Vertices

        // bottom + top + sides
        Vector3[] vertices = new Vector3[nbVerticesCap + nbVerticesCap + nbSides * 2 + 2];
        int vert = 0;
        float _2pi = Mathf.PI * 2f;

        // Bottom cap
        vertices[vert++] = new Vector3(0f, 0f, 0f);
        while (vert <= nbSides)
        {
            float rad = (float)vert / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0f, Mathf.Sin(rad) * bottomRadius);
            vert++;
        }

        // Top cap
        vertices[vert++] = new Vector3(0f, height, 0f);
        while (vert <= nbSides * 2 + 1)
        {
            float rad = (float)(vert - nbSides - 1) / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
            vert++;
        }

        // Sides
        int v = 0;
        while (vert <= vertices.Length - 4)
        {
            float rad = (float)v / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
            vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0, Mathf.Sin(rad) * bottomRadius);
            vert += 2;
            v++;
        }
        vertices[vert] = vertices[nbSides * 2 + 2];
        vertices[vert + 1] = vertices[nbSides * 2 + 3];
        #endregion

        #region Normales

        // bottom + top + sides
        Vector3[] normales = new Vector3[vertices.Length];
        vert = 0;

        // Bottom cap
        while (vert <= nbSides)
        {
            normales[vert++] = Vector3.down;
        }

        // Top cap
        while (vert <= nbSides * 2 + 1)
        {
            normales[vert++] = Vector3.up;
        }

        // Sides
        v = 0;
        while (vert <= vertices.Length - 4)
        {
            float rad = (float)v / nbSides * _2pi;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            normales[vert] = new Vector3(cos, 0f, sin);
            normales[vert + 1] = normales[vert];

            vert += 2;
            v++;
        }
        normales[vert] = normales[nbSides * 2 + 2];
        normales[vert + 1] = normales[nbSides * 2 + 3];
        #endregion

        #region UVs
        Vector2[] uvs = new Vector2[vertices.Length];

        // Bottom cap
        int u = 0;
        uvs[u++] = new Vector2(0.5f, 0.5f);
        while (u <= nbSides)
        {
            float rad = (float)u / nbSides * _2pi;
            uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
            u++;
        }

        // Top cap
        uvs[u++] = new Vector2(0.5f, 0.5f);
        while (u <= nbSides * 2 + 1)
        {
            float rad = (float)u / nbSides * _2pi;
            uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
            u++;
        }

        // Sides
        int u_sides = 0;
        while (u <= uvs.Length - 4)
        {
            float t = (float)u_sides / nbSides;
            uvs[u] = new Vector3(t, 1f);
            uvs[u + 1] = new Vector3(t, 0f);
            u += 2;
            u_sides++;
        }
        uvs[u] = new Vector2(1f, 1f);
        uvs[u + 1] = new Vector2(1f, 0f);
        #endregion

        #region Triangles
        int nbTriangles = nbSides + nbSides + nbSides * 2;
        int[] triangles = new int[nbTriangles * 3];

        // Bottom cap
        int tri = 0;
        int i = 0;
        while (tri < nbSides - 1)
        {
            triangles[i] = 0;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = tri + 2;
            tri++;
            i += 3;
        }
        triangles[i] = 0;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = 1;
        tri++;
        i += 3;

        // Top cap

        // We need to go for the next triangle as for the Top cap
        // We will be defining reversed triangles (210 instead of 012 so we want to avoid 100)
        tri++;
        while (tri < nbSides * 2)
        {
            triangles[i] = tri + 2;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = nbVerticesCap;
            tri++;
            i += 3;
        }

        triangles[i] = nbVerticesCap + 1;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = nbVerticesCap;
        tri++;
        i += 3;
        tri++;

        // Sides
        while (tri <= nbTriangles)
        {
            triangles[i] = tri + 2;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = tri + 0;
            tri++;
            i += 3;

            triangles[i] = tri + 1;
            triangles[i + 1] = tri + 2;
            triangles[i + 2] = tri + 0;
            tri++;
            i += 3;
        }
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();
    }

    public static void expand(Mesh mesh, float amount)
    {
        // Here are 2 possibilities, depending on what we want :
        // - To save memory and order it :
        //      delete the old TOP caps and replace it with the new then follow with the old and the new SIDES
        // - To save memory and keep it as it has been added :
        //      delete the old TOP caps and shift the old SIDES then follow with the new TOP and SIDES
        // - To save time, do not bother to delete the old TOP caps as they will not be accessible anymore

        mesh.MarkDynamic();

        float height = amount;
        float bottomRadius = mesh.bounds.extents.x;
        float topRadius = mesh.bounds.extents.x;
        int nbSides = 18/*(mesh.vertexCount / 4) - 1*/;

        // BottomLength = TopLength = SidesLength
        int nbVerticesCap = nbSides + 1;
        int sidesLength = nbSides * 2 + 1;

        #region Vertices

        // old_vertices(=bottom + top + sides) - old_top + top + sides
        Vector3[] vertices = new Vector3[mesh.vertexCount + nbSides * 2 + 1];

        int sidesStart = mesh.vertices.Length - sidesLength;
        int topStart = sidesStart - nbVerticesCap;        

        for (int j = 0; j < mesh.vertexCount; ++j)
        {
            if (j < topStart)
                vertices[j] = mesh.vertices[j];
            else if (j >= sidesStart)
                vertices[j - nbVerticesCap] = mesh.vertices[j];
        }

        int vert = mesh.vertices.Length - nbVerticesCap;
        float _2pi = Mathf.PI * 2f;

//         // Bottom cap
//         vertices[vert++] = new Vector3(0f, 0f, 0f);
//         while (vert <= mesh.vertexCount + nbSides)
//         {
//             float rad = (float)vert / nbSides * _2pi;
//             vertices[vert] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0f, Mathf.Sin(rad) * bottomRadius);
//             vert++;
//         }

        // Top cap
        vertices[vert++] = new Vector3(0f, height, 0f);
        while (vert <= mesh.vertexCount - nbVerticesCap + nbSides * 2 + 1)
        {
            float rad = (float)(vert - nbSides - 1) / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
            vert++;
        }

        // Sides
        int v = 0;
        while (vert <= vertices.Length - 4)
        {
            float rad = (float)v / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
            vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0, Mathf.Sin(rad) * bottomRadius);
            vert += 2;
            v++;
        }
        vertices[vert] = vertices[mesh.vertexCount];
        vertices[vert + 1] = vertices[mesh.vertexCount + 1];
        #endregion

        #region Normales

        // old_vertices(=bottom + top + sides) - old_top + top + sides
        Vector3[] normales = new Vector3[vertices.Length];
        for (int j = 0; j < mesh.vertexCount; ++j)
        {
            if (j < topStart)
                normales[j] = mesh.normals[j];
            else if (j >= sidesStart)
                normales[j - nbVerticesCap] = mesh.normals[j];
        }
        vert = mesh.vertices.Length - nbVerticesCap;

//         // Bottom cap
//         while (vert <= mesh.vertexCount + nbSides)
//         {
//             normales[vert++] = Vector3.down;
//         }

        // Top cap
        while (vert <= mesh.vertexCount - nbVerticesCap + nbSides * 2 + 1)
        {
            normales[vert++] = Vector3.up;
        }
        
        // Sides
        v = 0;
        while (vert <= vertices.Length - 4)
        {
            float rad = (float)v / nbSides * _2pi;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            normales[vert] = new Vector3(cos, 0f, sin);
            normales[vert + 1] = normales[vert];

            vert += 2;
            v++;
        }
        normales[vert] = normales[mesh.vertexCount];
        normales[vert + 1] = normales[mesh.vertexCount];
        #endregion

        #region UVs
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int j = 0; j < mesh.vertexCount; ++j)
        {
            if (j < topStart)
                uvs[j] = mesh.uv[j];
            else if (j >= sidesStart)
                uvs[j - nbVerticesCap] = mesh.uv[j];
        }
        int u = mesh.vertices.Length - nbVerticesCap;

        // Bottom cap
//         uvs[u++] = new Vector2(0.5f, 0.5f);
//         while (u <= nbSides)
//         {
//             float rad = (float)u / nbSides * _2pi;
//             uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
//             u++;
//         }

        // Top cap
        uvs[u++] = new Vector2(0.5f, 0.5f);
        while (u <= mesh.vertexCount - nbVerticesCap + nbSides * 2 + 1)
        {
            float rad = (float)u / nbSides * _2pi;
            uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
            u++;
        }

        // Sides
        int u_sides = 0;
        while (u <= uvs.Length - 4)
        {
            float t = (float)u_sides / nbSides;
            uvs[u] = new Vector3(t, 1f);
            uvs[u + 1] = new Vector3(t, 0f);
            u += 2;
            u_sides++;
        }
        uvs[u] = new Vector2(1f, 1f);
        uvs[u + 1] = new Vector2(1f, 0f);
        #endregion

        #region Triangles
        int nbTriangles = nbSides + nbSides + nbSides * 2;
        int[] triangles = new int[(nbTriangles + nbSides * 2) * 3];

        int tri = 0;
        int i = 0;

        while (tri < nbTriangles)
        {
            if (tri < nbSides)
            {
                triangles[i] = mesh.triangles[i];
                triangles[i + 1] = mesh.triangles[i + 1];
                triangles[i + 2] = mesh.triangles[i + 2];
            }
            else if (tri >= nbSides * 2 + 1)
            {
                triangles[i - nbSides] = mesh.triangles[i];
                triangles[i - nbSides + 1] = mesh.triangles[i + 1];
                triangles[i - nbSides + 2] = mesh.triangles[i + 2];
            }
            
            tri++;
            i += 3;
        }

//         // Bottom cap
//         while (tri < nbSides - 1)
//         {
//             triangles[i] = 0;
//             triangles[i + 1] = tri + 1;
//             triangles[i + 2] = tri + 2;
//             tri++;
//             i += 3;
//         }
//         triangles[i] = 0;
//         triangles[i + 1] = tri + 1;
//         triangles[i + 2] = 1;
//         tri++;
//         i += 3;

        // Top cap
        tri++;
        while (tri < mesh.vertices.Length - sidesLength)
        {
            triangles[i] = tri + 2;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = nbVerticesCap;
            tri++;
            i += 3;
        }

        triangles[i] = nbVerticesCap + 1;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = nbVerticesCap;
        tri++;
        i += 3;
        tri++;

        // Sides
        while (tri <= nbTriangles + nbSides * 2)
        {
            triangles[i] = tri + 2;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = tri + 0;
            tri++;
            i += 3;

            triangles[i] = tri + 1;
            triangles[i + 1] = tri + 2;
            triangles[i + 2] = tri + 0;
            tri++;
            i += 3;
        }
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();
    }
}
