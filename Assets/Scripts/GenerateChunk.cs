using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GenerateChunk : MonoBehaviour
{

    public Mesh mesh;

    public bool generateUnderground = false;

    public float freq = 10f;
    public float amp = 10f;

    public float xOffset = 0.1f;
    public float zOffset = 0.1f;

    public int chunkSize = 16;
    public int chunkHeight = 16;
    public int chunkGeneratedHeight = 0;

    public Texture2D textureAtlas;
    public Vector3 pos;
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();

    public BasicBlock[,,] chunkData;

    public int lastVertex = 0;
    public int textureWidth = 16;
    private GenerateTerrain parentScript;


    // Start is called before the first frame update
    public void TheStart()
    {
        pos = transform.position;
        //find Object Ground
        parentScript = GameObject.Find("Ground").GetComponent<GenerateTerrain>();

        chunkData = parentScript.chunksData[transform.name];
       
        //initialize the mesh
        mesh = new Mesh();

        //create mesh data
        //GenerateVoxelTerrain();

        //draw terrain
        DrawTerrain();

        //set mesh data
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.SetUVs(0, uvs);

        //recalculate lighting
        mesh.RecalculateNormals();

        //set the mesh
        GetComponent<MeshFilter>().mesh = mesh;

        //set texture 
        GetComponent<MeshRenderer>().material.mainTexture = textureAtlas;

    }

    void Update()
    {
        GetComponent<MeshRenderer>().enabled = CheckIfIsVisibleByCamera();
    }

    //generate grass block on coordenates
    private void GenerateVoxelTerrain()
    {
        chunkData = new BasicBlock[chunkSize, chunkHeight, chunkSize];
        for( int x = 0; x < chunkSize; x++){
            xOffset += xOffset;
            for( int z = 0; z < chunkSize; z++){
                int blockType = 0;
                zOffset += zOffset;
                float y = Mathf.FloorToInt(GetHeightWithNoise(pos.x+x,pos.z+z));
                if(y >= chunkHeight){
                    y = chunkHeight-1;
                }
                if(y < 0){
                    y = 0;
                }
                if(y>chunkGeneratedHeight){
                    chunkGeneratedHeight = (int)y;
                }
                AddCubesToChunkData(x, (int)y, z, blockType);
                for (float yAux = y-1; yAux >= 0; yAux--)
                {   blockType = 1;
                    if(generateUnderground)AddCubesToChunkData(x, (int)yAux, z, blockType);
                }
            }
        }

    }

    void AddCubesToChunkData(int x, int y, int z, int blockType){
        switch(blockType){
            case 0:
                chunkData[x, y, z] = new GrassBlock(pos, textureAtlas, textureWidth);
                break;
            case 1:
                chunkData[x, y, z] = new DirtBlock(pos, textureAtlas, textureWidth);
                break;
        }
    }

    float GetHeightWithNoise(float x, float y){
        return Remap(Mathf.PerlinNoise(x/freq, y/freq),0,1,0,chunkHeight-1);
    }

    float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void DrawTerrain(){
        //interate chunkData
        for(int x = 0; x < chunkSize; x++){
            for(int y = 0; y < chunkHeight; y++){
                for(int z = 0; z < chunkSize; z++){
                    if(chunkData[x, y, z] != null){
                        DrawCube(x, y, z);
                    }
                }
            }
        }

    }

    void DrawCube(int x, int y, int z){
        chunkData[x, y, z].pos = new Vector3(x, y, z);
        chunkData[x, y, z].lastVertex = lastVertex;
                        
        bool[] faceCheck = facesAdjacent(x, y, z);

        chunkData[x, y, z].DrawCube(faceCheck[0], faceCheck[1], faceCheck[2], faceCheck[3], faceCheck[4], faceCheck[5]);
        if(!faceCheck[0] || !faceCheck[1] || !faceCheck[2] || !faceCheck[3] || !faceCheck[4] || !faceCheck[5]){
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(1, 1, 1);
            boxCollider.center = new Vector3(x+0.5f, y+0.5f, z+0.5f);
        }
        vertices.AddRange(chunkData[x, y, z].GetVertices());
        triangles.AddRange(chunkData[x, y, z].GetTriangles());
        uvs.AddRange(chunkData[x, y, z].GetUVs());
        lastVertex = chunkData[x, y, z].lastVertex;
    }

    bool[] facesAdjacent(int x, int y, int z){
        Vector3 chunkPos = transform.position;
        BasicBlock[,,] adjacentChunk;
        //get parent script
        
        bool isBackAdjacent = false;
        bool isBackAdjacentOutOfChunk = z+1 >= chunkSize;
        if(isBackAdjacentOutOfChunk){
            adjacentChunk = findChunk(chunkPos, new Vector3(0,0,1));
            bool isBackChunkRendered = adjacentChunk != null;
            if (isBackChunkRendered)
            {
                isBackAdjacent =  adjacentChunk[x, y, 0] != null;
            }
        } else {
            isBackAdjacent = chunkData[x, y, z+1] != null;
        }
                        
        bool isFrontAdjacent = false;
        bool isFrontAdjacentOutOfChunk = z-1 < 0;
        if(isFrontAdjacentOutOfChunk){
            adjacentChunk = findChunk(chunkPos, new Vector3(0,0,-1));
            bool isFrontChunkRendered = adjacentChunk != null;
            if (isFrontChunkRendered)
            {
                isFrontAdjacent = adjacentChunk[x, y, chunkSize-1] != null;
            }
        } else {
            isFrontAdjacent = chunkData[x, y, z-1] != null;
        }

        bool isLeftAdjacent = false;
        bool isLeftAdjacentOutOfChunk = x-1 < 0;
        if(isLeftAdjacentOutOfChunk){
            adjacentChunk = findChunk(chunkPos, new Vector3(-1,0,0));
            bool isLeftChunkRendered = adjacentChunk != null;
            if (isLeftChunkRendered)
            {
                isLeftAdjacent = adjacentChunk[chunkSize-1, y, z] != null;
            }
        } else {
            isLeftAdjacent = chunkData[x-1, y, z] != null;
        }

        bool isRightAdjacent = false;
        bool isRightAdjacentOutOfChunk = x+1 >= chunkSize;
        if(isRightAdjacentOutOfChunk){
            adjacentChunk = findChunk(chunkPos, new Vector3(1,0,0));
            bool isRightChunkRendered = adjacentChunk != null;
            if (isRightChunkRendered)
            {
                isRightAdjacent = adjacentChunk[0, y, z] != null;
            }
        } else {
            isRightAdjacent = chunkData[x+1, y, z] != null;
        }

        bool isTopAdjacent = y+1 < chunkHeight && chunkData[x, y+1, z] != null;

        bool isBottomAdjacent = y == 0 || chunkData[x, y-1, z] != null;
        return  new bool[6] {isFrontAdjacent, isBackAdjacent, isLeftAdjacent, isRightAdjacent, isTopAdjacent, isBottomAdjacent};
    }

    BasicBlock[,,] findChunk(Vector3 currentPos, Vector3 newPos){
        if(parentScript.chunksData.ContainsKey((Mathf.Floor(currentPos.x/chunkSize) + newPos.x) + "," +  (newPos.z + Mathf.Floor(currentPos.z/chunkSize)) )){
            return parentScript.chunksData[(Mathf.Floor(currentPos.x/chunkSize) + newPos.x) + "," +  (newPos.z + Mathf.Floor(currentPos.z/chunkSize)) ];
        } else {
            return null;
        }
    }

    bool CheckIfIsVisibleByCamera(){

        //get camera object with tag MainCamera
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        Camera camera = cameraObject.GetComponent<Camera>();

        //get camera position
        Vector3 cameraPos = camera.transform.position;

        //get chunk position
        Vector3 chunkPos = transform.position;
        Vector3 chunkPosFront = new Vector3(chunkPos.x, chunkPos.y, chunkPos.z + chunkSize);
        Vector3 chunkPosLeft = new Vector3(chunkPos.x - chunkSize, chunkPos.y , chunkPos.z);
        Vector3 chunkPosRight = new Vector3(chunkPos.x + chunkSize, chunkPos.y , chunkPos.z);
        Vector3 chunkPosBack = new Vector3(chunkPos.x , chunkPos.y , chunkPos.z - chunkSize);

        //get camera viewport
        Vector3 cameraViewportFront = camera.WorldToViewportPoint(chunkPosFront);
        Vector3 cameraViewportLeft = camera.WorldToViewportPoint(chunkPosLeft);
        Vector3 cameraViewportRight = camera.WorldToViewportPoint(chunkPosRight);
        Vector3 cameraViewportBack = camera.WorldToViewportPoint(chunkPosBack);


        //check if chunk is visible in camera
        bool isVisibleFront = cameraViewportFront.z >= 0;
        bool isVisibleLeft = cameraViewportLeft.z >= 0;
        bool isVisibleRight = cameraViewportRight.z >= 0;
        bool isVisibleBack = cameraViewportBack.z >= 0;


        return isVisibleFront || isVisibleLeft || isVisibleRight || isVisibleBack;
 
    }
    
}
