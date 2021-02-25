using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;


namespace MCTS.DST.Actions
{

    public class Eat : ActionDST
    {
        public int Target;
        public float Duration;

        public Eat(int target) : base("Eat_" + target)
        {
            this.Target = target;
            this.Duration = 0.05f;
        }

        //Fazer Decompose

        public override void ApplyActionEffects(WorldModelDST worldModel)
        {
            //{ "seeds", 4},
            //{ "seeds_cooked", 4},
            //{ "carrot", 12},
            //{ "carrot_cooked", 12},
            //{ "berries_juicy", 12},
            //{ "berries_juicy_cooked", 18}, 
            //{ "berries", 9},
            //{ "berries_cooked", 12}

            foreach (var food in worldModel.foodInInventoryList)
            {
                if (food.Key == this.Target)
                {
                    worldModel.Walter.Hunger += food.Value.hungerFill;
                    worldModel.Walter.foodAmount -= food.Value.hungerFill;
                }
            }
        }

        public override bool CanExecute(WorldModelDST worldModelDST)
        {
            if (worldModelDST.Walter.foodAmount > 0 && worldModelDST.Walter.Hunger <= 140) return true;
            return false;
        }

        public override List<Pair<string, string>> Decompose(WorldModelDST worldModelDST)
        {
            List<Pair<string, string>> ListOfActions = new List<Pair< string, string>>(1);
            Pair<string, string> pair;

            string targetString = Target.ToString();

            pair = new Pair<string, string>("Action(EAT, -, -, -, -)", targetString);

            ListOfActions.Add(pair);

            return ListOfActions;                   
        }

        public override Pair<string, int> NextActionInfo()
        {
            return new Pair<string, int>("", 0);
        }
    }
}
