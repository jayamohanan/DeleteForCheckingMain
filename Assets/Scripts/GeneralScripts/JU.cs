using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class JU
{
    public static List<Vector3> SortList(List<Vector3> list)
    {
        List<Vector3> newList = new List<Vector3>();
        List<Vector3> outputList = new List<Vector3>();
        for (int i = 0; i < list.Count; i++)
        {
            newList.Add(list[i]);
        }
        int rand;
        while (newList.Count>0)
        {
            rand = Random.Range(0, newList.Count);
            outputList.Add(newList[rand]);
            newList.RemoveAt(rand);
        }
        return outputList;
    }
    public static void PrintList<T>(List<T> list)
    {
        string s = "";
        for (int i = 0; i < list.Count; i++)
        {
            s += list[i].ToString() + " "; 
        }
        Debug.Log(s);
    }
    public static void PrintArray<T>(T[] array)
    {
        string s = "";
        for (int i = 0; i < array.Length; i++)
        {
            s += array[i].ToString() + " "; 
        }
        Debug.Log(s);
    }
    public static GameObject DebugCube(Vector3 pos,  Color color, float scale = 0.2f)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = pos;
        obj.transform.localScale *= scale;
        obj.GetComponentInChildren<MeshRenderer>().material.color = color;
        return obj;
    }

    //LOok at direction of an object turned e angles from zero rotation
    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
    public static bool IsNull(Object obj)
    {
        if (obj == null)
        {
            Debug.Log("Obj null");
            return true;
        }
        else
        {
            Debug.Log("Obj not null");
            return false;
        }
    }
    public static void Pause(bool pause = true)
    {//
        #if UNITY_EDITOR
        EditorApplication.isPaused = pause;
#endif
    }
    public static Mesh CreateMesh(Vector3[] vertices, int[] triangles, Vector2[] uv)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        return mesh;
    }
    public static GameObject CreateObjectWithMesh(Vector3 position, Vector3[] vertices, int[] triangles, Vector2[] uv)
    {
        GameObject obj = new GameObject();
        obj.transform.position = position;
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        mf.mesh = CreateMesh(vertices, triangles, uv);
        return obj;
    }
    public  static List<T> ShuffleList<T>(List<T> list)
    {
        List<T> tempList = new List<T>();
        List<T> resultList = new List<T>();
        for (int i = 0; i < list.Count; i++)
        {
            tempList.Add(list[i]);
        }
        while (tempList.Count != 0)
        {
            int index = Random.Range(0, tempList.Count);
            resultList.Add(tempList[index]);
            tempList.RemoveAt(index);
        }
        return resultList;
    }
    public static T[] ShuffleArray<T>(T[] array)
    {
        List<T> tempList = new List<T>();
        List<T> resultList = new List<T>();
        for (int i = 0; i < array.Length; i++)
        {
            tempList.Add(array[i]);
        }
        while (tempList.Count != 0)
        {
            int index = Random.Range(0, tempList.Count);
            resultList.Add(tempList[index]);
            tempList.RemoveAt(index);
        }
        return resultList.ToArray();
    }
    public static T[] CopyArrayByValue<T>(T[] array)
    {
        T[] newArray = new T[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }
        return newArray;
    }
    public static List<T> CopyListByValue<T>(List<T> list)
    {
        List<T> newList = new List<T>();
        for (int i = 0; i < list.Count; i++)
        {
            newList.Add(list[i]);
        }
        return newList;
    }
    public static void Hi(int num = -1)
    {
        if(num==-1)
        Debug.Log("Hi");
        else
        {
            Debug.Log("Hi "+num);
        }
    }

}
