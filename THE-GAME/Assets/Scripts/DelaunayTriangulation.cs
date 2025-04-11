using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Nokta yapısı
/// </summary>
[System.Serializable]
public class Point
{
    public float x, z;
    public Point(float x, float z)
    {
        this.x = x;
        this.z = z;
    }
}
/// <summary>
/// Üçgen yapısı
/// </summary>
[System.Serializable]
public class Triangle
{
    public Point p1, p2, p3;

    public Triangle(Point p1, Point p2, Point p3)
    {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
    }

    // Üçgenin çevresini hesapla
    public float GetCircumradius()
    {
        // Basit bir yöntemle üçgenin çevresel yarıçapını hesapla
        float ax = p1.x, ay = p1.z;
        float bx = p2.x, by = p2.z;
        float cx = p3.x, cy = p3.z;

        float d = 2 * (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by));
        float ux = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
        float uy = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;

        float radius = Mathf.Sqrt((ux - ax) * (ux - ax) + (uy - ay) * (uy - ay));
        return radius;
    }

    // Üçgenin içinde olup olmadığını kontrol et
    public bool IsPointInside(Point p)
    {
        // Bu basit bir içerik kontrolü değil, sadece üçgenin çevresel sınırını kullanarak
        float radius = GetCircumradius();
        float dist = Mathf.Sqrt((p.x - p1.x) * (p.x - p1.x) + (p.z - p1.z) * (p.z - p1.z));
        return dist <= radius;
    }
}
/// <summary>
/// Delaunay Triangulation sınıfı
/// </summary>
public class DelaunayTriangulation : MonoBehaviour
{
    [SerializeField] Point[] superTrianglePoints;
    // Delaunay Triangulation hesaplama
    public List<Triangle> GenerateTriangulation(List<Point> points)
    {
        List<Triangle> triangles = new List<Triangle>();

        if (superTrianglePoints == null || superTrianglePoints.Length != 3)
        {
            Debug.LogError("SuperTrianglePoints dizisi hatalı! Lütfen üç nokta sağlayın.");
        }
        // 1. Başlangıçta büyük bir dış üçgen oluştur
        // Bu dış üçgen harita sınırlarını kapsayacak şekilde seçilir
        Point p1 = superTrianglePoints[0];
        Point p2 = superTrianglePoints[1];
        Point p3 = superTrianglePoints[2];

        Triangle initialTriangle = new Triangle(p1, p2, p3);
        triangles.Add(initialTriangle);

        // 2. Noktaları sırayla ekleyin
        foreach (Point point in points)
        {
            List<Triangle> badTriangles = new List<Triangle>();

            // 3. Tüm üçgenleri kontrol et ve bu noktayı içerenleri al
            foreach (Triangle triangle in triangles)
            {
                if (triangle.IsPointInside(point))
                {
                    badTriangles.Add(triangle);
                }
            }
            // 4. Kötü üçgenlerden yeni üçgenler oluştur
            foreach (Triangle badTriangle in badTriangles)
            {
                // Yeni üçgenler oluşturulacak
                Triangle newTriangle1 = new Triangle(badTriangle.p1, badTriangle.p2, point);
                Triangle newTriangle2 = new Triangle(badTriangle.p2, badTriangle.p3, point);
                Triangle newTriangle3 = new Triangle(badTriangle.p3, badTriangle.p1, point);
                triangles.Add(newTriangle1);
                triangles.Add(newTriangle2);
                triangles.Add(newTriangle3);
            }
            // 5. Eski üçgenleri sil    
            triangles.RemoveAll(t => badTriangles.Contains(t));
        }
        // Delaunay Triangulation tamamlandıktan sonra büyük üçgene bağlı üçgenleri temizle
        triangles = triangles.Where(t => !(t.p1 == p1 || t.p1 == p2 || t.p1 == p3 ||
                                   t.p2 == p1 || t.p2 == p2 || t.p2 == p3 ||
                                   t.p3 == p1 || t.p3 == p2 || t.p3 == p3)).ToList();
        return triangles;
    }
}