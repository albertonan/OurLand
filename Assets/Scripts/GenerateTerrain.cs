using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public GameObject chunk;
    public Camera camera;
    public Texture2D textureAtlas;
    public int textureWidth = 32;
    public bool generateUnderground = true;
    public long seed = 31412341234;
    public float freq = 10f;
    public float exponent = 3;
    public float caveFreq = 0.1f;
    public float caveExponent = 0.5f;
    public float amp = 10f;
    public float xOffset = 0.1f;
    public float zOffset = 0.1f;

    public int chunckRenderDistance = 1;
    public int chunkSize = 16;
    public int chunkHeight = 16;
    public IDictionary<string,BasicBlock[,,]> chunksData;

    private Vector3 myPos;
    private Vector3 cameraPos;

    public GameObject[] chunks;
    public int chunkGeneratedHeight = 0;
    // Start is called before the first frame update
    void Start()
    {
        chunksData = new Dictionary<string,BasicBlock[,,]>();
        GenerateStartingChunks();
        //get GameObject MainCamera
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraPos = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cameraPos = camera.transform.position;
        chunks = GameObject.FindGameObjectsWithTag("Chunk");
        DestroyChunks();
        LoadChunks();
    }

    private void DestroyChunks()
    {
        foreach (GameObject chunk in chunks)
        {
            if(Mathf.Abs(chunk.transform.position.x-cameraPos.x)/chunkSize > chunckRenderDistance+1 
                || Mathf.Abs(chunk.transform.position.z-cameraPos.z)/chunkSize > chunckRenderDistance+1)
            {
                Destroy(chunk);
                chunksData.Remove(chunk.name);
            }
        }
    }

    //Load new Chunks
    private void LoadChunks()
    {   
        GenerateChunksData();
        for(int x=-chunckRenderDistance; x<chunckRenderDistance; x++){
            for(int z = -chunckRenderDistance; z<chunckRenderDistance; z++){
                //check if chunk already is instantiated
                //find gameobject with name "Chunk_x_z"
                GameObject chunk = GameObject.Find(getChunkName(x,z));
                if (!chunk)
                {
                    //instantiate chunk
                    GenerateChunk(x, z);
                    return;
                }
            }
        }
    }

   //generate grass block on coordenates
    private void GenerateStartingChunks()
    {   
        GenerateChunksData();
        for(int x=-chunckRenderDistance; x<chunckRenderDistance; x++){
            for(int z = -chunckRenderDistance; z<chunckRenderDistance; z++){
                GenerateChunk(x, z);
            }
        }
    }

    private void GenerateChunksData()
    {
        for(int x=-chunckRenderDistance-2; x<chunckRenderDistance+2; x++){
            for(int z = -chunckRenderDistance-2; z<chunckRenderDistance+2; z++){
                if(!chunksData.ContainsKey(getChunkName(x,z))){
                    Vector3 pos = getChunkPos(x, z);
                    chunksData.Add(getChunkName(x,z),GenerateChunkData(pos));
                }
            }  
        }
    }

    private BasicBlock[,,] GenerateChunkData(Vector3 pos)
    {
        BasicBlock[,,] chunkData = new BasicBlock[chunkSize, chunkHeight, chunkSize];
        for( int x = 0; x < chunkSize; x++){
            for( int z = 0; z < chunkSize; z++){
                int blockType = 0;
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
                bool isCave = GetCaveWithNoise(pos.x+x, y, z+pos.z);
                if(!isCave){
                    chunkData[x, (int)y, z] = returnCubeByType(pos, x, (int)y, z, blockType);
                }
                for (float yAux = y-1; yAux >= 0; yAux--)
                {   
                    isCave = GetCaveWithNoise(pos.x+x, yAux, z+pos.z);
                    if(!isCave){
                        blockType = 1;
                        if(generateUnderground){
                            chunkData[x, (int)yAux, z] = returnCubeByType(pos, x, (int)yAux, z, blockType);
                        }
                    }
                }
            }
        }
        return chunkData;
    }

    BasicBlock returnCubeByType(Vector3 pos, int x, int y, int z, int blockType){
        switch(blockType){
            case 0:
                return new GrassBlock(pos, textureAtlas, textureWidth);
            case 1:
                return new DirtBlock(pos, textureAtlas, textureWidth);
            default:
                return new BasicBlock(pos, textureAtlas, textureWidth);
        }
    }

    float GetHeightWithNoise(float x, float z){
        float nx = (x)/chunkSize -0.5f;
        float nz = (z)/chunkSize -0.5f;
        float y = freq * Mathf.PerlinNoise(nx * freq, nz * freq);
        y += freq/2 * Mathf.PerlinNoise(nx * freq * 2, nz * freq * 2);
        y += freq/4 * Mathf.PerlinNoise(nx * freq * 4, nz * freq * 4);
        y = y / (freq + freq/2 + freq/4);
        y = Mathf.Pow(y*1.2f, exponent);
        y = Remap(y,0,1,0,chunkHeight-1);
        return y;
    }

    bool GetCaveWithNoise(float x,float y, float z){
        float caveFreqAux = Remap(y,0,chunkHeight,caveFreq*1.3f,caveFreq/1.3f);
        float nx = x/caveFreqAux;
        float ny = y/caveFreqAux;
        float nz = z/caveFreqAux;
        float xy = Mathf.PerlinNoise(nx, ny);
        float yz = Mathf.PerlinNoise(ny, nz);
        float xz = Mathf.PerlinNoise(nx, nz);
        float yx = Mathf.PerlinNoise(ny, nx);
        float zy = Mathf.PerlinNoise(nz, ny);
        float zx = Mathf.PerlinNoise(nz, nx);
        float xyz = (xy + yz + xz + yx + zy + zx)/6;
        float tolerance = 0.5f;
        if(y>10){
            tolerance = Remap(y,0,chunkGeneratedHeight,0.5f,0.7f);
        } else {
            tolerance = Remap(y,0,10,0.8f,0.5f);
        }
        return xyz > tolerance;
    }

    float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    string getChunkName(int x, int z){
        return (x + Mathf.Floor(cameraPos.x/chunkSize)) + "," +  (z + Mathf.Floor(cameraPos.z/chunkSize));
    }
    Vector3 getChunkPos(int x, int z){
        return new Vector3((x + Mathf.Floor(cameraPos.x/chunkSize))*chunkSize, 0, (z + Mathf.Floor(cameraPos.z/chunkSize))*chunkSize);
    }

    private void GenerateChunk(int x, int z)
    {   
        Vector3 pos = new Vector3((x + Mathf.Floor(cameraPos.x/chunkSize))*chunkSize, 0, (z + Mathf.Floor(cameraPos.z/chunkSize))*chunkSize);
        GameObject newChunk = Instantiate(chunk, pos, Quaternion.identity);
        newChunk.name = getChunkName(x, z);
        GenerateChunk chunkScript = newChunk.GetComponent<GenerateChunk>();
        chunkScript.textureAtlas = textureAtlas;
        chunkScript.textureWidth = textureWidth;
        chunkScript.amp = amp;
        chunkScript.freq = freq;
        chunkScript.xOffset = xOffset;
        chunkScript.zOffset = zOffset;
        chunkScript.chunkSize = chunkSize;
        chunkScript.chunkHeight = chunkHeight;
        chunkScript.generateUnderground = generateUnderground;
        chunkScript.TheStart();
    }


    
}
