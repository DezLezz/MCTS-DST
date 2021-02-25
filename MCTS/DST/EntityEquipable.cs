using System;
using Utilities;
using System.Collections.Generic;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.WorldModels
{

    public class EntityEquipable : Entity
    { 

        public string name {get; set;}
        public EntityEquipable(int guid, int posx, int posz, int quantity, string name) {

            this.GUID = guid;
            this.posx = posx;
            this.posz = posz;
            this.quantity = quantity;
            this.name = name;

            this.isCollectable = false; 
            this.isCooker  = false;
            this.isCookable = false;
            this.isEdible = false;
            this.isEquippable = true;
            this.isFuel  = false;
            this.isFueled = false;
            this.isGrower = false;
            this.isHarvestable = false;
            this.isPickable = false;
            this.isStewer = false;
            this.isChoppable = true;
            this.isDiggable = false;
            this.isHammerable = false;
            this.isMinable = false;
        }

    } 
    
    
}