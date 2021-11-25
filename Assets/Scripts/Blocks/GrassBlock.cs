// System.Object Block 
using System.Collections.Generic;
using UnityEngine;
public class GrassBlock : BasicBlock
{
    public GrassBlock(Vector3 pos, Texture2D textureAtlas, int textureWidth): base(pos, textureAtlas, textureWidth)
    {
        blockID = 0;
        blockName = "Grass";
        blockType = "grass";
        blockHardness = 2;
        blockResistance = 2;
    }
    
}
