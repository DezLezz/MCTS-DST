using System;
using Utilities;
using System.Collections.Generic;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.WorldModels
{

    public class EntityHarvest : Entity
    { 
        public string name { get; set; }
        public EntityHarvest(int guid, int posx, int posz, int quantity, string name) {

            this.GUID = guid;
            this.posx = posx;
            this.posz = posz;
            this.quantity = quantity;
            this.name = name;

            this.isCollectable = true; 
            this.isCooker  = false;
            this.isCookable = false;
            this.isEdible = false;
            this.isEquippable = false;
            this.isFuel  = false;
            this.isFueled = false;
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