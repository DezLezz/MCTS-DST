using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;


namespace MCTS.DST.Actions
{

    public class PickUp : ActionDST
    {
        public int Target;
        public float Duration;

        public PickUp(int target) : base("PickUp_" + target)
        {
            this.Target = target;
            this.Duration = 0.05f;
        }

        //Fazer Decompose

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            if (worldModel.resourceList.ContainsKey(this.Target))
            {
                if (worldModel.Walter.Inventory.ContainsKey(worldModel.resourceList[this.Target].name))
                    worldModel.Walter.Inventory[worldModel.resourceList[this.Target].name] += 1;
            }
            else if (worldModel.foodList.ContainsKey(this.Target))
            {
                worldModel.Walter.foodAmount += worldModel.foodList[this.Target].hungerFill;
            }
            else if (worldModel.equipableList.ContainsKey(this.Target))
            {
                worldModel.Walter.Inventory[worldModel.equipableList[this.Target].name] += 1;
            }
            else
            {
                Console.WriteLine("ups");
            }
        }

        public override List<Pair<string, string>> Decompose(WorldModelDST worldModelDST)
        {
            List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
            Pair<string, string> pair;

            string targetString = Target.ToString();

            pair = new Pair<string, string>("Action(PICKUP, -, -, -, -)", targetString);

            ListOfActions.Add(pair);

            return ListOfActions;
        }

        public override Pair<string, int> NextActionInfo()
        {
            return new Pair<string, int>("", 0);
        }
    }
}
