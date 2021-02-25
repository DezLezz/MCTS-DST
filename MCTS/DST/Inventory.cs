using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCTS.DST
{
    class Inventory
    {
        public int Charcoal { get; set; }
        public int Cutgrass { get; set; }
        public int Flint { get; set; }
        public int Log { get; set; }
        public int Poop { get; set; }
        public int Rocks { get; set; }
        public int Twigs { get; set; }
        public int Torch { get; set; }
        public int Axe { get; set; }

        public int Pickaxe { get; set; }

    public Inventory(Dictionary<string, int> inventory_dic)
        {
            this.Charcoal = inventory_dic["charcoal"];
            this.Cutgrass = inventory_dic["cutgrass"];
            this.Flint = inventory_dic["flint"];
            this.Log = inventory_dic["log"];
            this.Poop = inventory_dic["poop"];
            this.Rocks = inventory_dic["rocks"];
            this.Twigs = inventory_dic["twigs"];
            this.Torch = inventory_dic["torch"];
            this.Axe = inventory_dic["axe"];
            this.Pickaxe = inventory_dic["pickaxe"];

        }

        public Inventory(Inventory inv)
        {
            this.Charcoal = inv.Charcoal;
            this.Cutgrass = inv.Cutgrass;
            this.Flint = inv.Flint;
            this.Log = inv.Log;
            this.Poop = inv.Poop;
            this.Rocks = inv.Rocks;
            this.Twigs = inv.Twigs;
            this.Torch = inv.Torch;
            this.Axe = inv.Axe;
            this.Pickaxe = inv.Pickaxe;
        }
    }


}
