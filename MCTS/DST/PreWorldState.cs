using System;
using System.Collections.Generic;
using Utilities;
using KnowledgeBase;
using MCTS.DST.WorldModels;
using WellFormedNames;
using System.Linq;

namespace MCTS.DST { 

    public class PreWorldState
    { 
        public Character Walter { get; set; }

        public Dictionary<int, Entity> entityList { get; set; }
        public Dictionary<int, EntityFood> foodList { get; set; }
        public Dictionary<int, EntityFood> foodInInventoryList { get; set; }
        public Dictionary<int, EntityEquipable> equipableList { get; set; } 
        public Dictionary<int, EntityEquipable> equipableInInventoryList { get; set; } 
        public Dictionary<int, EntityResource> resourceList { get; set; }
        public Dictionary<int, EntityResource> resourceInInventoryList { get; set; }
        public Dictionary<int, EntityHarvest> harvestList { get; set; }
        public Dictionary<int, Entity> inventoryList { get; set; }
        public Dictionary<float, int> twigsInTheWorld { get; set; }
        public Dictionary<float, int> grassInTheWorld { get; set; }

        public KB KnowledgeBase;

        public float dayFase { get; set; }

        public string InLight { get; set; }

        public int counter;

        public PreWorldState(KB knowledgeBase) {

            this.KnowledgeBase = knowledgeBase;

            this.entityList = new Dictionary<int, Entity>();
            this.foodList = new Dictionary<int, EntityFood>();
            this.foodInInventoryList = new Dictionary<int, EntityFood>();
            this.equipableList = new Dictionary<int, EntityEquipable>();
            this.equipableInInventoryList = new Dictionary<int, EntityEquipable>();
            this.resourceList = new Dictionary<int, EntityResource>();
            this.resourceInInventoryList = new Dictionary<int, EntityResource>();
            this.harvestList = new Dictionary<int, EntityHarvest>();
            this.inventoryList = new Dictionary<int, Entity>();
            this.twigsInTheWorld = new Dictionary<float, int>();
            this.grassInTheWorld = new Dictionary<float, int>();

            this.counter = 0;

            var cycle = knowledgeBase.AskProperty((Name)"World(CurrentSegment)");
            this.dayFase = float.Parse(cycle.Value.ToString());
            Console.WriteLine(this.dayFase);

            var light = knowledgeBase.AskProperty((Name)"InLight(Walter)");
            this.InLight = light.Value.ToString();

            var hp = knowledgeBase.AskProperty((Name)"Health(Walter)");           
            int HP = int.Parse(hp.Value.ToString());

            var hunger = knowledgeBase.AskProperty((Name)"Hunger(Walter)");
            int Hunger = int.Parse(hunger.Value.ToString());

            var sanity = knowledgeBase.AskProperty((Name)"Sanity(Walter)");
            int Sanity = int.Parse(sanity.Value.ToString());

            var posx = knowledgeBase.AskProperty((Name)"PosX(Walter)");
            var PosX = int.Parse(posx.Value.ToString());

            var posz = knowledgeBase.AskProperty((Name)"PosZ(Walter)");
            var PosZ = int.Parse(posz.Value.ToString());

            var temperature = knowledgeBase.AskProperty((Name)"Temperature(Walter)");
            var Temperature = int.Parse(temperature.Value.ToString());

            this.Walter = new Character(HP, Hunger, Sanity, PosX, PosZ, Temperature);

            var subset = new List<SubstitutionSet> { new SubstitutionSet() };
            
            var entities = knowledgeBase.AskPossibleProperties((Name)"Entity([GUID])", Name.SELF_SYMBOL, subset);

            foreach (var ent in entities) {

                string strEntGuid = ent.Item2.FirstOrDefault().FirstOrDefault().SubValue.Value.ToString();
                int entGuid = int.Parse(strEntGuid);              //GUID da entidade
                string entPrefab = ent.Item1.Value.ToString();    //Nome da entidade

                //if (inventoryList.Keys.Contains(entGuid)) {       //If the entitie is already registered in the inventory and in the entitie lists, we just skip it
                //    continue;
                //}

                var equiped = knowledgeBase.AskProperty((Name)("IsEquipped(" + strEntGuid + ")")).Value.ToString();
                if (equiped == "True")
                {
                    this.Walter.Equiped = entPrefab;
                }


                    var inv = knowledgeBase.AskProperty((Name)("InInventory(" + strEntGuid + ")")).Value.ToString();

                if (inv == "True")
                {

                    string strEntPosx = "PosX(" + strEntGuid + ")";
                    var POSx = knowledgeBase.AskProperty((Name)strEntPosx);
                    int entPosx = int.Parse(POSx.Value.ToString());

                    string strEntPosz = "PosZ(" + strEntGuid + ")";
                    var POSz = knowledgeBase.AskProperty((Name)strEntPosz);
                    int entPosz = int.Parse(POSz.Value.ToString());

                    string strEntQuantity = "Quantity(" + strEntGuid + ")";
                    var quantity = knowledgeBase.AskProperty((Name)strEntQuantity);
                    int entQuantity = int.Parse(quantity.Value.ToString());

                    int tempFoodAmount = isFoodEntity(entPrefab);

                    if (tempFoodAmount > 0)
                    {
                        //string strCookable = ;
                        var cookable = knowledgeBase.AskProperty((Name)("IsCookable(" + strEntGuid + ")")).Value.ToString();
                        bool cookableFlag = false;

                        if (cookable == "True")
                        {
                            cookableFlag = true;
                        }

                        EntityFood f = new EntityFood(entGuid, entPosx, entPosz, entQuantity, cookableFlag, tempFoodAmount, entPrefab);
                        foodInInventoryList.Add(entGuid, f);
                        entityList.Add(entGuid, f);
                        inventoryList.Add(entGuid, f);

                        this.Walter.foodAmount += tempFoodAmount;
                    }

                    else if (isResourceEntity(entPrefab))
                    {
                        var fuel = knowledgeBase.AskProperty((Name)("IsFuel(" + strEntGuid + ")")).Value.ToString();
                        bool fuelFlag = false;

                        if (fuel == "True")
                        {
                            fuelFlag = true;
                        }

                        EntityResource r = new EntityResource(entGuid, entPosx, entPosz, entQuantity, fuelFlag, entPrefab);
                        resourceInInventoryList.Add(entGuid, r);
                        entityList.Add(entGuid, r);
                        inventoryList.Add(entGuid, r);

                        this.Walter.Inventory[entPrefab] += entQuantity;

                    }

                    else if (isEquipableEntity(entPrefab))
                    {

                        EntityEquipable e = new EntityEquipable(entGuid, entPosx, entPosz, entQuantity, entPrefab);
                        equipableInInventoryList.Add(entGuid, e);
                        entityList.Add(entGuid, e);

                        inventoryList.Add(entGuid, e);

                        this.Walter.Inventory[entPrefab] += entQuantity;

                    }
                }

                string inSightBool = knowledgeBase.AskProperty((Name)("InSight(" + strEntGuid + ")")).Value.ToString();       //Entidade à vista??
                
                if (inSightBool == "True") {       //Not sure which one it is

                    string strEntPosx = "PosX(" + strEntGuid + ")";
                    var POSx = knowledgeBase.AskProperty((Name)strEntPosx);
                    int entPosx = int.Parse(POSx.Value.ToString());

                    string strEntPosz = "PosZ(" + strEntGuid + ")";
                    var POSz = knowledgeBase.AskProperty((Name)strEntPosz);
                    int entPosz = int.Parse(POSz.Value.ToString());

                    string strEntQuantity = "Quantity(" + strEntGuid + ")";
                    var quantity = knowledgeBase.AskProperty((Name)strEntQuantity);
                    int entQuantity = int.Parse(quantity.Value.ToString());

                    int tempFoodAmount = isFoodEntity(entPrefab);

                    if (tempFoodAmount > 0) {
                        
                        var cookable = knowledgeBase.AskProperty((Name)("IsCookable(" + strEntGuid + ")")).Value.ToString();
                        bool cookableFlag = false;
                       
                        if (cookable == "True") {
                            cookableFlag = true;
                        }

                        EntityFood f = new EntityFood(entGuid, entPosx, entPosz, entQuantity, cookableFlag, tempFoodAmount, entPrefab);
                        foodList.Add(entGuid, f);
                        entityList.Add(entGuid, f);

                        this.Walter.foodAmount += tempFoodAmount;

                    }

                    else if (isResourceEntity(entPrefab)) {
                        var fuel = knowledgeBase.AskProperty((Name)("IsFuel(" + strEntGuid + ")")).Value.ToString();
                        bool fuelFlag = false;
                       
                        if (fuel == "True") {
                            fuelFlag = true;
                        }

                        EntityResource r = new EntityResource(entGuid, entPosx, entPosz, entQuantity, fuelFlag, entPrefab);
                        resourceList.Add(entGuid, r);
                        entityList.Add(entGuid, r);
                        
                    }

                    else if (isEquipableEntity(entPrefab)) {

                        EntityEquipable e = new EntityEquipable(entGuid, entPosx, entPosz, entQuantity, entPrefab);
                        equipableList.Add(entGuid, e);
                        entityList.Add(entGuid, e);
                        
                    }

                    else if (isHarvestEntity(entPrefab)) {

                        var collectable = knowledgeBase.AskProperty((Name)("IsCollectable(" + strEntGuid + ")")).Value.ToString();

                        if (collectable == "True")
                        {
                            EntityHarvest h = new EntityHarvest(entGuid, entPosx, entPosz, entQuantity, entPrefab);
                            harvestList.Add(entGuid, h);
                            entityList.Add(entGuid, h);
                        }

                        
                    }
                }

                if (this.counter <100)
                {
                    string strEntPosx = "PosX(" + strEntGuid + ")";
                    var POSx = knowledgeBase.AskProperty((Name)strEntPosx);
                    int entPosx = int.Parse(POSx.Value.ToString());

                    string strEntPosz = "PosZ(" + strEntGuid + ")";
                    var POSz = knowledgeBase.AskProperty((Name)strEntPosz);
                    int entPosz = int.Parse(POSz.Value.ToString());

                    if (entPrefab == "twigs")
                    {
                        this.twigsInTheWorld.Add(DistanceCalculator(entPosx, entPosz), entGuid);
                        counter++; 
                    }
                    else if (entPrefab == "cutgrass")
                    {
                        this.grassInTheWorld.Add(DistanceCalculator(entPosx, entPosz), entGuid);
                        counter++;
                    }

                }
            }
        }

        public int isFoodEntity(string name) {
            int result;
            Dictionary<string, int> foodArray = new Dictionary<string, int>()
                                            {
                                                {"seeds", 4},
                                                {"seeds_cooked", 4},
                                                {"cactus_flower", 12},
                                                {"carrot", 12},
                                                {"carrot_cooked", 12},
                                                {"berries_juicy", 12},
                                                {"berries_juicy_cooked", 18},
                                                {"cactus_meat", 12},
                                                {"catus_meat_cooked", 12}, 
                                                {"berries", 9},
                                                {"berries_cooked", 12}
                                            };
            if(foodArray.TryGetValue(name, out result))
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
        public bool isEquipableEntity(string name) {
            List<string> eList = new List<string>() {"torch"};
            return eList.Contains(name);
        }
        public bool isHarvestEntity(string name) {
            List<string> eList = new List<string>() {"berrybush", "berrybush2", "cactus", "carrot_planted", "berrybush_juicy","sapling", "grass"};
            return eList.Contains(name);
        }
    
        public bool isResourceEntity(string name) {
            List<string> rList = new List<string>() { "cutgrass", "twigs"};
            return rList.Contains(name);
        }

        public void updateInventory(string name, int amount) {
            this.Walter.Inventory[name] += amount;
        }

        public float DistanceCalculator(Entity ent) {
            return Convert.ToSingle(Math.Pow(Convert.ToDouble(this.Walter.posx - ent.posx), 2) + Math.Pow(Convert.ToDouble(this.Walter.posz - ent.posz), 2));
        }

        public float DistanceCalculator(int posx, int posz)
        {
            return Convert.ToSingle(Math.Pow(Convert.ToDouble(this.Walter.posx - posx), 2) + Math.Pow(Convert.ToDouble(this.Walter.posz - posz), 2));
        }

    }

}
