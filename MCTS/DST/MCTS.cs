using System;
using MCTS.DST.Actions;
using MCTS.DST.WorldModels;
using System.Collections.Generic;


namespace MCTS.DST
{

    public class MCTSAlgorithm
    {
        public const float C = 1.4f;
        public bool InProgress { get; private set; }
        public int MaxIterations { get; set; }
        public int MaxIterationsProcessedPerFrame { get; set; }
        public int MaxPlayoutDepthReached { get; private set; }
        public int MaxSelectionDepthReached { get; private set; }
        public float TotalProcessingTime { get; private set; }
        public int NumberOfIterations { get; private set; }
        public MCTSNode BestFirstChild { get; set; }
        public List<ActionDST> BestActionSequence { get; private set; }
        public ActionDST BestAction { get; private set; }

        protected int CurrentIterations { get; set; }
        protected int CurrentIterationsInFrame { get; set; }
        protected int CurrentDepth { get; set; }

        protected WorldModelDST CurrentState { get; set; }
        public MCTSNode InitialNode { get; set; }
        protected System.Random RandomGenerator { get; set; }

        public MCTSAlgorithm(WorldModelDST currentState)
        {
            this.InProgress = false;
            this.CurrentState = currentState;
            this.MaxIterations = 100;
            this.MaxIterationsProcessedPerFrame = 100;
            this.RandomGenerator = new System.Random();
            this.TotalProcessingTime = 0.0f;
            this.NumberOfIterations = 0;
        }

        public void InitializeMCTSearch()
        {
            this.MaxPlayoutDepthReached = 4;
            this.MaxSelectionDepthReached = 2;
            this.CurrentIterations = 0;
            this.CurrentIterationsInFrame = 0;
            //this.CurrentStateWorldModel.Initialize();
            this.InitialNode = new MCTSNode(this.CurrentState)
            {
                Action = null,
                Parent = null,
            };
            this.InProgress = true;
            this.BestFirstChild = null;
            this.BestActionSequence = new List<ActionDST>();
            this.BestAction = null;
        }

        public ActionDST Run()
        {
            MCTSNode selectedNode;
            float reward;

            //var startTime = Time.realtimeSinceStartup;
            for ( int i = 0; i<MaxIterations; i++)
            {

                
                CurrentIterationsInFrame++;
                this.NumberOfIterations++;
                if(CurrentIterationsInFrame >= MaxIterationsProcessedPerFrame)
                {
                    break;
                }
                selectedNode = Selection(InitialNode);

                foreach (var nextNode in selectedNode.ChildNodes)
                {
                    BestActionSequence.Add(nextNode.Action);
                }

                reward = Playout(selectedNode.State);

                Backpropagate(selectedNode, reward);

            }
            
            this.BestAction = BestFinalAction(InitialNode);
            //this.TotalProcessingTime += Time.realtimeSinceStartup - startTime;
            this.InProgress = false;
            //return BestFinalAction(BestChild(InitialNode));
            return this.BestAction;
            
        }

        protected MCTSNode Selection(MCTSNode nodeToDoSelection)
        {

            ActionDST nextAction;
            MCTSNode currentNode = nodeToDoSelection;
            MCTSNode bestChild;
            //this.MaxSelectionDepthReached = 0;

            while(currentNode.State.IsTerminal() == false){
                this.MaxSelectionDepthReached++;
                nextAction = currentNode.State.GetNextAction();
                if (nextAction !=null){
                    return Expand(currentNode, nextAction);
                }
                else{
                    bestChild = BestUCTChild(currentNode);
                    if (bestChild == null)
                    {
                        return currentNode;
                    }
                    else
                    {
                        currentNode = bestChild;
                    }
                }
            }
            return currentNode;
        }


        protected float Playout(WorldModelDST initialPlayoutState)
        {

            ActionDST nextAction;
            WorldModelDST currentState = initialPlayoutState;

            var totalRewards = 0.0f;
            //this.MaxPlayoutDepthReached = 0;

            for( var i = 0; i < 500; i++){
                this.CurrentDepth = 0;
                while (currentState.IsTerminal() == false && this.CurrentDepth < 2)
                {
                    this.MaxPlayoutDepthReached++;
                    if (currentState.GetExecutableActions().Length > 0)
                    {
                        var random_index = RandomGenerator.Next(currentState.GetExecutableActions().Length);
                        nextAction = currentState.GetExecutableActions()[random_index];
                        currentState = currentState.GenerateChildWorldModel();
                        nextAction.ApplyActionEffects(currentState);
                        this.CurrentDepth++;
                    }
                    else {break; }
                }
                totalRewards += currentState.GetScore();
            }
            

            float reward = totalRewards/500; 

            return reward;
        }

        protected virtual void Backpropagate(MCTSNode node, float reward)
        {
            while(node != null){
                node.N = node.N + 1;
                node.Q = node.Q + reward;
                node = node.Parent;
            }
        }

        protected MCTSNode Expand(MCTSNode parent, ActionDST action)
        {
            WorldModelDST new_state = parent.State.GenerateChildWorldModel();
            action.ApplyActionEffects(new_state);

            MCTSNode childNode = new MCTSNode(new_state){ 
                Parent = parent,
                Action = action,
            };
            parent.ChildNodes.Add(childNode);
            return childNode;
        }

        protected virtual MCTSNode BestUCTChild(MCTSNode node)
        {

            MCTSNode bestChild = null;
            double max_val = float.MinValue;
            foreach (MCTSNode child in node.ChildNodes){
                var current_val = child.Q / child.N  +  MCTSAlgorithm.C * Math.Sqrt(Math.Log(child.Parent.N)/child.N);
                if (current_val > max_val){
                    max_val = current_val;
                    bestChild = child;
                }
            }
            
            return bestChild;

        }

        protected MCTSNode BestChild(MCTSNode node)
        {
            MCTSNode bestChild = null;
            double max_val = float.MinValue;
            foreach(MCTSNode child in node.ChildNodes){
                double current_val = child.Q / child.N;
                if (current_val > max_val){
                    max_val = current_val;
                    bestChild = child;

                   
                }
            }
            
            return bestChild;
        }

        protected ActionDST BestFinalAction(MCTSNode node)
        {
            var bestChild = this.BestChild(node);
            if (bestChild == null) return null;

            this.BestFirstChild = bestChild;

            //this is done for debugging proposes only
            this.BestActionSequence = new List<ActionDST>();
            this.BestActionSequence.Add(bestChild.Action);
            node = bestChild;

            while(!node.State.IsTerminal())
            {
                bestChild = this.BestChild(node);
                if (bestChild == null) break;
                this.BestActionSequence.Add(bestChild.Action);
                node = bestChild;
            }
            Console.WriteLine("Q " + this.BestFirstChild.Q);
            return this.BestFirstChild.Action;
        }

    
    }
}
