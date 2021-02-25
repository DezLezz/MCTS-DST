using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.Actions;
using MCTS.DST;
using System.Linq;

namespace MCTS.DST.WorldModels
{
    public class WorldModelDST
    { 
        public Character Walter { get; set; }
        public float dayFase { get; set; }

        public string InLight { get; set; }
        public bool BuiltTorchDayTime { get; set; }

        public List<ActionDST> ActionList { get; set; }
        protected IEnumerator<ActionDST> ActionEnumerator { get; set; }

        public WorldModelDST Parent { get; set; }
        public PreWorldState PreWorldState { get; set; }

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

        public WorldModelDST() { 
        }

        public WorldModelDST(WorldModelDST parent) {
            this.Walter = new Character(parent.Walter);
            this.ActionList = parent.ActionList;
            this.Parent = parent;

            this.BuiltTorchDayTime = parent.BuiltTorchDayTime;
            this.ActionEnumerator = this.ActionList.GetEnumerator();

            this.dayFase = parent.dayFase;
            this.InLight = parent.InLight;

            this.entityList = parent.entityList;
            this.foodList = parent.foodList;
            this.foodInInventoryList = parent.foodInInventoryList;
            this.equipableList = parent.equipableList;
            this.equipableInInventoryList = parent.equipableInInventoryList;
            this.resourceList = parent.resourceList;
            this.resourceInInventoryList = parent.resourceInInventoryList;
            this.harvestList = parent.harvestList;
            this.inventoryList = parent.inventoryList;
            this.twigsInTheWorld = parent.twigsInTheWorld;
            this.grassInTheWorld = parent.grassInTheWorld;
        }

        public WorldModelDST(PreWorldState preWorldState) {

            this.Parent = null;

            this.PreWorldState = preWorldState;

            this.dayFase = preWorldState.dayFase;
            this.InLight = preWorldState.InLight;
            this.BuiltTorchDayTime = false;

            this.Walter = preWorldState.Walter;

            this.entityList = preWorldState.entityList;
            this.foodList = preWorldState.foodList;
            this.foodInInventoryList = preWorldState.foodInInventoryList;
            this.equipableList = preWorldState.equipableList;
            this.equipableInInventoryList = preWorldState.equipableInInventoryList;
            this.resourceList = preWorldState.resourceList;
            this.resourceInInventoryList = preWorldState.resourceInInventoryList;
            this.harvestList = preWorldState.harvestList;
            this.inventoryList =  preWorldState.inventoryList;
            this.twigsInTheWorld = preWorldState.twigsInTheWorld;
            this.grassInTheWorld = preWorldState.grassInTheWorld;

            this.ActionList = new List<ActionDST>();   //Actions to do: Pick, PickUp, Build, Eat, Equip, Chop, Mine, Eat (again??), Cook, Wander, Light*, Drop*, WalkTo* (*probably not necessary)
            

            foreach (var food in foodInInventoryList) {                
                this.ActionList.Add(new Eat(food.Key));

            }

            foreach (var food in foodList)
            {
                this.ActionList.Add(new PickUp(food.Key));
            }

            foreach (var resource in resourceList) {             //To pick up just the resources from the ground
                this.ActionList.Add(new PickUp(resource.Key));
            }

            foreach (var equipment in equipableList) {                 //To take care of random free loot there might be
                this.ActionList.Add(new PickUp(equipment.Key));

            }

            foreach (var harvest in harvestList) {    
                this.ActionList.Add(new Pick(harvest.Key));
            }
            
            /*foreach (var tree in treeList) {
                //this.ActionList.Add(new Chop(tree.Key));
            }*/
            /*
            foreach (var stone in stoneList) {
                this.ActionList.Add(new Mine(stone.Key));
            }*/

            //this.ActionList.Add(new Build(this.Walter.posx, this.Walter.posz, "torch"));    //Not sure if this is the recipe name
            /*this.ActionList.Add(new Build(this.Walter.posx, this.Walter.posz, "axe"));    //Not sure if this is the recipe name
            this.ActionList.Add(new Build(this.Walter.posx, this.Walter.posz, "pickaxe"));    //Not sure if this is the recipe name
            this.ActionList.Add(new Build(this.Walter.posx, this.Walter.posz, "campfire"));   //Not sure if this is the recipe name
            */

            this.ActionList.Add(new Build(this.Walter.posx, this.Walter.posz, "torch"));    //Not sure if this is the recipe name
            this.ActionList.Add(new Wander());

            this.ActionEnumerator = ActionList.GetEnumerator();
        }

        public bool IsTerminal() {

            return this.Walter.HP <= 0;

        }

        public WorldModelDST GenerateChildWorldModel() {
            return new WorldModelDST(this);
        }

       
         public float GetScore() {
            int reward = 0;


            if (this.dayFase < 14)
            {   
                if(this.BuiltTorchDayTime == true)
                {
                    return 0;
                }

                reward += 10 * this.Walter.foodAmount;

                if (this.Walter.Inventory["cutgrass"] < 10){
                    reward += 10 * this.Walter.Inventory["cutgrass"];
                }
                else
                {
                    reward += 0;
                }

                if (this.Walter.Inventory["twigs"] < 10)
                {
                    reward += 10 * this.Walter.Inventory["twigs"];
                }
                else
                {
                    reward += 0;
                }

                /*if (this.Walter.Inventory["torch"] >= 1)
                {
                    reward -= this.Walter.Inventory["torch"];
                }*/


            } else
            {
                if(this.InLight == "False")
                {
                    if (this.Walter.Inventory["torch"] == 1)
                    {
                        reward += 100;
                    }

                }
            }

            return reward;
            //Console.WriteLine(this.dayFase);
            /*int reward = 0;
            if (this.dayFase < 12)
            {
                if (this.Walter.Equiped != "torch") reward += 100;
                reward += this.Walter.Inventory["torch"] * 1000;

                reward += 10 * this.Walter.foodAmount;

                reward += (int) Math.Log( this.Walter.Inventory["twigs"]);
                reward += (int) Math.Log(this.Walter.Inventory["cutgrass"]);

                reward -= (150 - this.Walter.Hunger) *100; 

            } else
            {
                if (this.Walter.Equiped == "torch") { reward += 1000; }
                else
                {
                    reward -= 1000;
                }

                reward += this.Walter.Inventory["torch"] * 1000;
                reward += 10 * this.Walter.foodAmount;

                reward += (int)Math.Log(this.Walter.Inventory["twigs"]);
                reward += (int)Math.Log(this.Walter.Inventory["cutgrass"]);
            }

            return reward;*/
         }
         

        public ActionDST[] GetExecutableActions()
        {
            return this.ActionList.Where(a => a.CanExecute(this)).ToArray();
        }

        public ActionDST GetNextAction()
        {
            ActionDST action = null;
            //returns the next action that can be executed or null if no more executable actions exist
            if (this.ActionEnumerator.MoveNext())
            {
                action = this.ActionEnumerator.Current;
            }

            while (action != null && !action.CanExecute(this))
            {
                if (this.ActionEnumerator.MoveNext())
                {
                    action = this.ActionEnumerator.Current;
                }
                else
                {
                    action = null;
                }
            }

            return action;
        }


    }

}