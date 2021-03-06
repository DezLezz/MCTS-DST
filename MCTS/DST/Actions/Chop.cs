﻿using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;


namespace MCTS.DST.Actions
{
    class Chop : ActionDST
    {

        public int Target;
        public float Duration;

        public Chop(int target) : base("Chop_" + target)
        {
            this.Target = target;
            this.Duration = 0.05f;
        }

        public override bool CanExecute(WorldModelDST worldModelDST)
        {
            foreach (var item in worldModelDST.equipableInInventoryList)
            {
                Console.WriteLine("hey its ya boi error");
                
                if (item.Value.name == "axe")
                {
                    return true;
                }
            }
            return false;
        }

        public override List<Pair<string, string>> Decompose(WorldModelDST worldModelDST)
        {
            List<Pair<string, string>> ListOfActions = new List<Pair<string, string>>(1);
            Pair<string, string> pair;

            string targetString = Target.ToString();

            int item_id = 0;

            foreach(var item in worldModelDST.equipableInInventoryList)
            { 
                if(item.Value.name == "axe")
                {
                    item_id = item.Key;
                    break;
                }
            }

            pair = new Pair<string, string>("Action(EQUIP, " + item_id +", -, -, -)", "-");
            ListOfActions.Add(pair);
            pair = new Pair<string, string>("Action(CHOP, -, -, -, -)", targetString);
            ListOfActions.Add(pair);
            pair = new Pair<string, string>("Action(UNEQUIP, -, -, -, -)", item_id.ToString());
            ListOfActions.Add(pair);

            return ListOfActions;
        }

    }
}
