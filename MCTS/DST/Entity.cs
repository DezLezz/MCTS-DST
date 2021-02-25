using System;
using Utilities;
using System.Collections.Generic;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.WorldModels
{

    public class Entity
    {
        public int GUID { get; set; }
        public bool isCollectable { get; set; }
        public bool isCooker { get; set; }
        public bool isCookable { get; set; }
        public bool isEdible { get; set; }
        public bool isEquippable { get; set; }
        public bool isFuel { get; set; }
        public bool isFueled { get; set; }
        public bool isGrower { get; set; }
        public bool isHarvestable { get; set; }
        public bool isPickable { get; set; }
        public bool isStewer { get; set; }
        public bool isChoppable { get; set; }
        public bool isDiggable { get; set; }
        public bool isHammerable { get; set; }
        public bool isMinable { get; set; }
        public int posx { get; set; }
        public int posz { get; set; }
        public int quantity { get; set; }

        //public bool isInIventory;
        //public bool isEquipped;

        public Entity() {

        }

        public Entity(int GUID, int posx, int posz, int quantity,
                    bool isCollectable = false, 
                    bool isCooker  = false,
                    bool isCookable = false,
                    bool isEdible = false,
                    bool isEquippable = false,
                    bool isFuel  = false,
                    bool isFueled = false,
                    bool isGrower = false,
                    bool isHarvestable = false,
                    bool isPickable = false,
                    bool isStewer = false,
                    bool isChoppable = false,
                    bool isDiggable = false,
                    bool isHammerable = false,
                    bool isMinable = false) {
                        
            this.GUID = GUID;
            this.isCollectable = isCollectable;
            this.isCooker = isCooker;
            this.isCookable = isCookable;
            this.isEdible = isEdible;
            this.isEquippable = isEquippable;
            this.isFuel = isFuel;
            this.isFueled = isFueled;
            this.isGrower = isGrower;
            this.isHarvestable = isHarvestable;
            this.isPickable = isPickable;
            this.isStewer = isStewer;
            this.isChoppable = isChoppable;
            this.isDiggable = isDiggable;
            this.isHammerable = isHammerable;
            this.isMinable = isMinable;
            this.posx = posx;
            this.posz = posz;
            this.quantity = quantity;

        }

    }
        
}
