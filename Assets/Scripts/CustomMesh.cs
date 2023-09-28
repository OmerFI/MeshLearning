using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class CustomMesh : MonoBehaviour
{
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private Transform moveD;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    private void Update()
    {
        Vector2 direction = transform.rotation * Vector2.up;

        Vector2 leftTopPoint = Vector2.zero;
        Vector2 rightTopPoint = Vector2.zero;

        RaycastHit2D leftHit = Physics2D.Raycast(leftPivot.position, direction);
        if (leftHit.collider != null)
        {
            leftTopPoint = leftHit.point;
        }

        RaycastHit2D rightHit = Physics2D.Raycast(rightPivot.position, direction);
        if (rightHit.collider != null)
        {
            rightTopPoint = rightHit.point;
        }

        Mesh mesh = new Mesh();

        if (leftHit.collider != rightHit.collider)
        {
            // Intersection of two colliders
            float crossPointX;
            float crossPointY;
            if (rightTopPoint.y >= leftTopPoint.y)
            {
                if (rightTopPoint.x >= leftTopPoint.x)
                {
                    crossPointX = leftTopPoint.x;
                    crossPointY = rightTopPoint.y;
                }
                else
                {
                    crossPointX = rightTopPoint.x;
                    crossPointY = leftTopPoint.y;
                }
            }
            else
            {
                if (rightTopPoint.x >= leftTopPoint.x)
                {
                    crossPointX = rightTopPoint.x;
                    crossPointY = leftTopPoint.y;
                }
                else
                {
                    crossPointX = leftTopPoint.x;
                    crossPointY = rightTopPoint.y;
                }
            }

            Vector2 crossPoint = new Vector2(crossPointX, crossPointY);
            
            Vector3[] vertices = new Vector3[5];
            Vector2[] uv = new Vector2[5];
            int[] triangles = new int[9];
            
            Vector3 AB = rightPivot.position - leftPivot.position;
            Vector3 AC = (Vector3)crossPoint - leftPivot.position;
            Vector3 DPoint = leftPivot.position + Vector3.Project(AC, AB);
            moveD.transform.position = DPoint;

            float leftX = leftPivot.localPosition.x;
            float rightX = rightPivot.localPosition.x;
            float middleX = leftPivot.localPosition.x + Vector3.Distance(leftPivot.position, DPoint);
            float middleHeight = Vector3.Distance(DPoint, (Vector3)crossPoint);
            vertices[0] = new Vector3(leftX, 0);
            vertices[1] = new Vector3(leftX, Vector3.Distance(leftPivot.position, leftTopPoint));
            vertices[2] = new Vector3(middleX, middleHeight);
            vertices[3] = new Vector3(rightX, Vector3.Distance(rightPivot.position, rightTopPoint));
            vertices[4] = new Vector3(rightX, 0);
            
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(0, 1);
            uv[2] = new Vector2(0.5f, 1);
            uv[3] = new Vector2(1, 1);
            uv[4] = new Vector2(1, 0);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
            
            triangles[6] = 0;
            triangles[7] = 3;
            triangles[8] = 4;
            
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }
        else
        {
            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            float leftX = leftPivot.localPosition.x;
            float rightX = rightPivot.localPosition.x;
            vertices[0] = new Vector3(leftX, 0);
            vertices[1] = new Vector3(leftX, Vector3.Distance(leftPivot.position, leftTopPoint));
            vertices[2] = new Vector3(rightX, Vector3.Distance(rightPivot.position, rightTopPoint));
            vertices[3] = new Vector3(rightX, 0);

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(0, 1);
            uv[2] = new Vector2(1, 1);
            uv[3] = new Vector2(1, 0);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
            
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
}