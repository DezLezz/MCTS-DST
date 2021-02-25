using System;
using System.Collections.Generic;
using Utilities;
using MCTS.DST.WorldModels;
using MCTS.DST;

namespace MCTS.DST.Actions
{

    public class ActionDST
    {
        private static int ActionID = 0;
        public string Name;
       
        public int ID { get; set; }

        public ActionDST(string name)
        {
            this.ID = ActionDST.ActionID++;
            this.Name = name;
        }

        public virtual List<Pair<string, string>> Decompose(WorldModelDST worldModelDST)
        {
            List<Pair<string, string>> list = new List<Pair<string, string>>(1);

            //Initialized it with a random action, there are better ways of doing this
            Pair<string, string> pair = new Pair<string, string>("Action(Wander, -, -, -, -)", "-");
            list.Add(pair);

            return list;
        }

        public virtual bool CanExecute(WorldModelDST woldModelDST)
        {
            return true;
        }

        public virtual bool CanExecute()
        {
            return true;
        }

        public virtual void Execute()
        {
        }

        public virtual void ApplyActionEffects(WorldModelDST worldState)
        {
        }

        public virtual Pair<string, int> NextActionInfo()
        {
            return new Pair<string, int>("", 0);
        }
    }
}
