using System;
using Utilities;
using System.Collections.Generic;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.WorldModels
{

    public class EntityFood : Entity
    { 

        public int hungerFill {get; set;}
        public string name { get; set; }
        public EntityFood(int guid, int posx, int posz, int quantity, bool isCookable, int hungerFill, string name) {

            this.GUID = guid;
            this.posx = posx;
            this.posz = posz;
            this.quantity = quantity;
            this.hungerFill = hungerFill;
            this.name = name;

            this.isCollectable = false; 
            this.isCooker  = false;
            this.isCookable = isCookable;
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