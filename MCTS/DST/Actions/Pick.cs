using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;


namespace MCTS.DST.Actions
{

    public class Pick : ActionDST
    {
        public int Target;
        public float Duration;

        public Pick(int target) : base("Pick_" + target)
        {
            this.Target = target;
            this.Duration = 0.05f;
        }

        //Fazer Decompose

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            //Console.WriteLine("effects pick");
            //{"berrybush", "berrybush2", "cactus", "carrot_planted", "berrybush_juicy","sapling", "grass"}

             if (worldModel.harvestList[this.Target].name == "cactus")
             {
                 worldModel.Walter.foodAmount += 12;
             }
             else if (worldModel.harvestList[this.Target].name == "carrot_planted")
             {
                 worldModel.Walter.foodAmount += 12;
             }
             else if (worldModel.harvestList[this.Target].name == "sapling")
             {
                 worldModel.Walter.Inventory["twigs"] += 1;
             }
             else if (worldModel.harvestList[this.Target].name == "grass")
             {
                 worldModel.Walter.Inventory["cutgrass"] += 1;
             }
            //Console.WriteLine("after effects pick");
        }

        public override List<Pair<string, string>> Decompose(WorldModelDST worldModelDST)
        {
            List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
            Pair<string, string> pair;

            string targetString = Target.ToString();

            pair = new Pair<string, string>("Action(PICK, -, -, -, -)", targetString);

            ListOfActions.Add(pair);

            return ListOfActions;
        }

        public override Pair<string, int> NextActionInfo()
        {
            return new Pair<string, int>("", 0);
        }
    }
}
