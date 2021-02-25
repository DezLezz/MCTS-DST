using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.Actions
{
    class Build : ActionDST
    {
        public string Recipe;
        public int World_x;
        public int World_z;

        public Build(int world_x, int world_z, string recipe) : base("Build_" + recipe)
        {
            this.Recipe = recipe;
            this.World_x = world_x;
            this.World_z = world_z;
        }


        public override bool CanExecute(WorldModelDST worldModelDST)
        {
            var inventory = worldModelDST.Walter.Inventory;

            if (worldModelDST.dayFase < 14 || worldModelDST.Walter.Equiped == "torch" || worldModelDST.Walter.Inventory["torch"] >= 1)
            {
                return false;
            }


            if (Recipe == "torch")
            {
                if (inventory["twigs"] >= 2 && inventory["cutgrass"] >= 2)
                {
                    return true;
                }
            }

            return false;
        }

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            if (this.Recipe == "torch")
            {
                worldModel.Walter.Equiped = "torch";
                worldModel.Walter.Inventory["torch"] += 1;
            }
        }

        public override List<Pair<string, string>> Decompose(WorldModelDST worldModelDST)
        {
            List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
            Pair<string, string> pair;

            pair = new Pair<string, string>("Action(BUILD, -, " + World_x + ", " + World_z + ", " + Recipe + ")", "-");

            ListOfActions.Add(pair);

            return ListOfActions;
        }
    }
}
