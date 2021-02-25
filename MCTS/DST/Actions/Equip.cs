using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.Actions
{
    class Equip : ActionDST
    {
        public string Name_item;

        public Equip(string name_item) : base("Equip_" + name_item)
        {
            this.Name_item = name_item;

        }
        public override bool CanExecute(WorldModelDST worldModelDST)
        {
            foreach (var item in worldModelDST.equipableInInventoryList)
            {
                if (item.Value.name == Name_item)
                {
                    return true;
                }
            }
            return false;
        }

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
           if (Name_item == "torch")
            {
                worldModel.Walter.Equiped = "torch";
            }
           else if (Name_item == "axe")
            {
                worldModel.Walter.Equiped = "axe";
            }
            else if (Name_item == "pickaxe")
            {
                worldModel.Walter.Equiped = "pickaxe";
            }

        }

        public override List<Pair<string, string>> Decompose(WorldModelDST worldModelDST)
        {
            List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
            Pair<string, string> pair;



            int item_id = 0;

            foreach (var item in worldModelDST.equipableInInventoryList)
            {
                if (item.Value.name == Name_item)
                {
                    item_id = item.Key;
                    break;
                }
            }

            pair = new Pair<string, string>("Action(EQUIP, " + item_id + ", -, -, -)", "-");
            ListOfActions.Add(pair);


            return ListOfActions;
        }
    }
}
