// System.Object Block 
using System.Collections.Generic;
using UnityEngine;
public class DirtBlock : BasicBlock
{
    public DirtBlock(Vector3 pos, Texture2D textureAtlas, int textureWidth): base(pos, textureAtlas, textureWidth)
    {
        blockID = 1;
        blockName = "Dirt";
        blockType = "dirt";
        blockHardness = 2;
        blockResistance = 2;
    }

    override public void AddFrontUV(){
        //apply texture to UV
            uvs.Add(new Vector2(0, 1) * textureWidth/2);
            uvs.Add(new Vector2(0, 2) * textureWidth/2);
            uvs.Add(new Vector2(1, 2) * textureWidth/2);
            uvs.Add(new Vector2(1, 1) * textureWidth/2);
    }

}
