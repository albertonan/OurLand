using UnityEngine;
using System.Collections.Generic;
public class BasicBlock: System.Object
{
    public Texture2D[] blockFaceTextures; //front, back, left, right, top, bottom
    public int blockID;
    public string blockName;

    public string blockType = "grass";	
    public int blockHardness = 2;
    public int blockResistance;
    public Vector3 pos;
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();
    public Texture2D textureAtlas;
    
    public int lastVertex = 0;
    public int textureWidth;

    public BasicBlock(Vector3 pos, Texture2D textureAtlas, int textureWidth)
    {
    
        this.pos = pos;
        this.textureAtlas = textureAtlas;
        this.textureWidth = textureWidth;
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
    }

    public void DrawCube(bool isFrontAdjacent, bool isBackAdjacent, bool isLeftAdjacent, bool isRightAdjacent, bool isTopAdjacent, bool isBottomAdjacent)
    {  
        if(!isFrontAdjacent) Front_GenerateFace();
        if(!isBackAdjacent) Back_GenerateFace();
        if(!isLeftAdjacent) Left_GenerateFace();
        if(!isRightAdjacent) Right_GenerateFace();
        if(!isTopAdjacent) Top_GenerateFace();
        if(!isBottomAdjacent) Bottom_GenerateFace();
    }

    //return vertices
    public List<Vector3> GetVertices()
    {
        return vertices;
    }

    //return triangles
    public List<int> GetTriangles()
    {
        return triangles;
    }

    //return uvs
    public List<Vector2> GetUVs()
    {
        return uvs;
    }


    //generate front face
    void Front_GenerateFace()
    {
            //declare vertices
            Vector3 vertice0 = pos;
            Vector3 vertice1 = pos + Vector3.up;
            Vector3 vertice2 = pos + Vector3.up + Vector3.right;
            Vector3 vertice3 = pos + Vector3.right;
            vertices.Add(vertice0); //0
            vertices.Add(vertice1); //1
            vertices.Add(vertice2); //2
            vertices.Add(vertice3); //3

            //Frist triangle
            triangles.Add(lastVertex);
            triangles.Add(lastVertex + 1);
            triangles.Add(lastVertex + 2);

            //Second triangle
            triangles.Add(lastVertex);
            triangles.Add(lastVertex + 2);
            triangles.Add(lastVertex + 3);

            AddFrontUV();

            lastVertex += 4;
    }

    //generate back face
    //TODO : BUG not showing back face
    void Back_GenerateFace()
    {
            //declare vertices
            Vector3 vertice0 = pos + Vector3.forward + Vector3.right;
            Vector3 vertice1 = pos + Vector3.forward + Vector3.up + Vector3.right;
            Vector3 vertice2 = pos + Vector3.forward + Vector3.up;
            Vector3 vertice3 = pos + Vector3.forward ;
            vertices.Add(vertice0); //0
            vertices.Add(vertice1); //1
            vertices.Add(vertice2); //2
            vertices.Add(vertice3); //3
            
            //Frist triangle
            triangles.Add(lastVertex);
            triangles.Add(lastVertex + 1);
            triangles.Add(lastVertex + 2);

            //Second triangle
            triangles.Add(lastVertex);
            triangles.Add(lastVertex + 2);
            triangles.Add(lastVertex + 3);

            AddBackUV();

            lastVertex += 4;
        
    }

    
    //generate left face
    void Left_GenerateFace()
    {

        //declare vertices
        Vector3 vertice0 = pos + Vector3.forward;
        Vector3 vertice1 = pos + Vector3.up + Vector3.forward;
        Vector3 vertice2 = pos + Vector3.up;
        Vector3 vertice3 = pos;
        vertices.Add(vertice0); //0
        vertices.Add(vertice1); //1
        vertices.Add(vertice2); //2
        vertices.Add(vertice3); //3

        //Frist triangle
        triangles.Add(lastVertex);
        triangles.Add(lastVertex + 1);
        triangles.Add(lastVertex + 2);

        //Second triangle
        triangles.Add(lastVertex);
        triangles.Add(lastVertex + 2);
        triangles.Add(lastVertex + 3);

        //apply texture to UV
        AddLeftUV();

        lastVertex += 4;
        
    }

    //generate right face
    void Right_GenerateFace()
    {

            //declare vertices
            Vector3 vertice0 = pos + Vector3.right;
            Vector3 vertice1 = pos + Vector3.up + Vector3.right;
            Vector3 vertice2 = pos + Vector3.up + Vector3.forward + Vector3.right;
            Vector3 vertice3 = pos + Vector3.forward + Vector3.right;
            vertices.Add(vertice0); //0
            vertices.Add(vertice1); //1
            vertices.Add(vertice2); //2
            vertices.Add(vertice3); //3

            //Frist triangle
            triangles.Add(lastVertex);
            triangles.Add(lastVertex + 1);
            triangles.Add(lastVertex + 2);

            //Second triangle
            triangles.Add(lastVertex);
            triangles.Add(lastVertex + 2);
            triangles.Add(lastVertex + 3);

            //apply texture to UV
            AddRightUV();

            lastVertex += 4;
        
    }

    //generate top face
    void Top_GenerateFace()
    {

            //declare vertices
            Vector3 vertice0 = pos + Vector3.up;
            Vector3 vertice1 = pos + Vector3.up + Vector3.forward;
            Vector3 vertice2 = pos + Vector3.up + Vector3.forward + Vector3.right;
            Vector3 vertice3 = pos + Vector3.up + Vector3.right;
            vertices.Add(vertice0); //0
            vertices.Add(vertice1); //1
            vertices.Add(vertice2); //2
            vertices.Add(vertice3); //3

            //Frist triangle
            triangles.Add(lastVertex);
            triangles.Add(lastVertex + 1);
            triangles.Add(lastVertex + 2);

            //Second triangle
            triangles.Add(lastVertex);
            triangles.Add(lastVertex + 2);
            triangles.Add(lastVertex + 3);

            //apply texture to UV
            AddTopUV();
        
        lastVertex += 4;

    }

    //generate bottom face
    void Bottom_GenerateFace()
    {

            //declare vertices
            Vector3 vertice0 = pos;
            Vector3 vertice1 = pos + Vector3.forward;
            Vector3 vertice2 = pos + Vector3.forward + Vector3.right;
            Vector3 vertice3 = pos + Vector3.right;
            vertices.Add(vertice0); //0
            vertices.Add(vertice1); //1
            vertices.Add(vertice2); //2
            vertices.Add(vertice3); //3

            //Frist triangle
            triangles.Add(lastVertex + 2);
            triangles.Add(lastVertex + 1);
            triangles.Add(lastVertex);

            //Second triangle
            triangles.Add(lastVertex + 3);
            triangles.Add(lastVertex + 2);
            triangles.Add(lastVertex);

            //apply texture to UV
            AddBottomUV();

            lastVertex += 4;
        
    }

    virtual public void AddFrontUV(){
        //apply texture to UV
            uvs.Add(Vector2.zero);
            uvs.Add(Vector2.up * textureWidth/2);
            uvs.Add(Vector2.one * textureWidth/2);
            uvs.Add(Vector2.right * textureWidth/2);
    }

    virtual public void AddBackUV(){
        //apply texture to UV
        AddFrontUV();
    }

    virtual public void AddLeftUV(){
        //apply texture to UV
        AddFrontUV();
    }

    virtual public void AddRightUV(){
        //apply texture to UV
        AddFrontUV();
    }

    virtual public void AddTopUV(){
        //apply texture to UV
        AddFrontUV();
    }

    virtual public void AddBottomUV(){
        //apply texture to UV
        AddFrontUV();
    }
}
