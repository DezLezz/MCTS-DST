using System;
using System.Collections.Generic;
using Utilities;

namespace MCTS.DST.WorldModels
{

    public class Character
    {
        public int HP { get; set; }
        public int Hunger { get; set; }
        public int Sanity { get; set; }
        public int posx { get; set; }
        public int posz { get; set; }
        public int temperature { get; set; }
        public int foodAmount {get; set;}

        public string Equiped { get; set; }

        public Dictionary<string, int> Inventory {get; set;}

        public Character()
        {
        }
        public Character(Character Walter)
        {
            this.HP = Walter.HP;
            this.Hunger = Walter.Hunger;
            this.Sanity = Walter.Sanity;
            this.posx = Walter.posx;
            this.posz = Walter.posz;
            this.temperature = Walter.temperature;
            this.foodAmount = 0;
            this.Equiped = "nothing";

            this.Inventory = new Dictionary<string, int>() {
                {"cutgrass", 0}, {"twigs", 0} ,{"torch", 0}
            };

            foreach (var key in Walter.Inventory)
            {
                this.Inventory[key.Key] = key.Value;
            }

            //this.Inventory = Walter.Inventory;
        }

        public Character(int hp, int hunger, int sanity, int posx, int posz, int temperature)
        {
            this.HP = hp;
            this.Hunger = hunger;
            this.Sanity = sanity;
            this.posx = posx;
            this.posz = posz;
            this.temperature = temperature;
            this.foodAmount = 0;
            this.Equiped = "nothing";

            this.Inventory = new Dictionary<string, int>() {
                 {"cutgrass", 0}, {"twigs", 0} ,{"torch", 0}
            };

        }

        public void increaseFoodAmount(int n) {
            this.foodAmount += n;
        }

        public void DecreaseHunger(int n)
        {
            if (this.Hunger - n < 0) this.Hunger = 0;
            else this.Hunger -= n;
        }

        public void IncreaseHunger(int n)
        {
            if (this.Hunger + n > 150) this.Hunger = 150;
            else this.Hunger += n;
        }

        public void DecreaseHP(int n)
        {
            if (this.HP - n < 0) this.HP = 0;
            else this.HP -= n;
        }

        public void IncreaseHP(int n)
        {
            if (this.HP + n > 150) this.HP = 150;
            else this.HP += n;
        }

        
    } 
}
