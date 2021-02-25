using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.Actions
{
    class Unequip : ActionDST
    {
        public string Name_item;

        public Unequip(string name_item) : base("Unquip" + name_item)
        {
            this.Name_item = name_item;

        }
        public override bool CanExecute(WorldModelDST worldModelDST)
        {
            if (worldModelDST.Walter.Equiped != "nothing")
            {
                return true;
            }
            return false;
        }

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
           
            worldModel.Walter.Equiped = "nothing";
          
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

            pair = new Pair<string, string>("Action(UNEQUIP, " + item_id + ", -, -, -)", "-");
            ListOfActions.Add(pair);


            return ListOfActions;
        }
    }
}
