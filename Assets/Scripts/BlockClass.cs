using System.Collections;
using UnityEngine;
[CreateAssetMenu(fileName = "newBlockClass", menuName = "BlockClass")]
public class BlockClass : ScriptableObject
{
    public string blockName;

    [Tooltip("Front, back, left, right, top, bottom")]
    public Texture2D[] blockFaceTextures; //front, back, left, right, top, bottom
    public int blockID;
    public int blockType;
    public int blockHardness;
    public int blockResistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
