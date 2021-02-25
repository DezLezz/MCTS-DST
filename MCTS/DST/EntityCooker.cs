using System;
using Utilities;
using System.Collections.Generic;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.WorldModels
{

    public class EntityCooker : Entity
    { 
        public EntityCooker(int guid, int posx, int posz, int quantity) {

            this.GUID = guid;
            this.posx = posx;
            this.posz = posz;
            this.quantity = quantity;

            this.isCollectable = false; 
            this.isCooker  = true;
            this.isCookable = false;
            this.isEdible = false;
            this.isEquippable = true;
            this.isFuel  = false;
            this.isFueled = true;
            this.isGrower = false;
            this.isHarvestable = false;
            this.isPickable = false;
            this.isStewer = false;
            this.isChoppable = false;
            this.isDiggable = false;
            this.isHammerable = false;
            this.isMinable = false;
        }
    } 
    
    
}