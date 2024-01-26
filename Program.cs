using System;
using System.Collections.Generic;

// Archangel At - 30; Def -30 ; Dam - 50-50; Hp - 250; Sed - 18 ; Spel - 30% morals. 150% dam ArchDevil
// ArchDevil At - 26; Def -28 ; Dam - 30-40; Hp - 200; Sed - 17 ; Spel - -30% udaca. 150% dam Archangel, ne imeet otbetnogo udara
// GhostDragon At - 19; Def - 17 ; Dam - 25-50; Hp - 200; Sed - 14 ; Spel - -30% morals. 20% spell Старость (снижает здоровье на 50% на 3 раунда)
// AncientBehemoth At - 19; Def - 19 ; Dam - 30-50; Hp - 300; Sed - 9 ; Spel - Снижает броню цели при ударе, (80%*защита цели - 1) округление вверх.
// Haspid At - 29; Def - 20 ; Dam - 30-55; Hp - 300; Sed - 12 ; Spel - 30% отравить цель - здоровье снижается на 10% за раунт в течении 3х раундов. spell  - Месть увеличение урона в зависимости от недостающего здоровья - [Корень квадратный ((HP) / (HPnow + HP) - 1) * 100%)]



namespace PvP_OOP_5_Players
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class Menu
    {
        public Menu()
        {
            PlayerRed playerRed = new PlayerRed();
            PlayerBlue playerBlue = new PlayerBlue();
        }

        private List    <Unit> _units = new List <Unit> ();

        private void CreateListUnits()
        {
            _units.Add(new AncientBehemoth());
            _units.Add(new Archangel());
            _units.Add(new ArchDevil());
            _units.Add(new GhostDragon());
            _units.Add(new Haspid());
        }


    }


    class Unit
    {

        public Unit(int attack, int defense, int minDamage,int maxDamage, int health, int speed, int morale, int luck, bool abilityCounterattack)
        {
            Attack = attack;
            Defense = defense;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Health = health;
            Speed = speed;
            Morale = morale;
            Luck = luck;
            AbilityCounterattack = abilityCounterattack;
        }

        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }
        public int Damage { get; private set; }
        public int Health { get; private set; }
        public int Speed { get; private set; }
        public int Morale { get; private set; }
        public int Luck { get; private set; }
        public bool AbilityCounterattack { get; private set; }


    }

    class AncientBehemoth : Unit
    {
        public AncientBehemoth() : base(19, 19, 30, 50, 300, 9, 0, 0, true) { }

        public void ApplyAbilityAttack(int defense)
        {
            defense = defense - (0.8 * defense - 1);
            return defense;
        }
    }

    class Archangel : Unit
    {
        public Archangel() : base(30, 30, 50, 50, 250, 18, 1, 0, true) { }
    }

    class ArchDevil : Unit
    {
        public ArchDevil() : base(26, 28, 30, 40, 200, 17, 0, 0, true) { }
    }

    class GhostDragon : Unit
    {
        public GhostDragon() : base(19, 17, 25, 50, 200, 14, 0, 0, true) { }
    }

    class Haspid : Unit
    {
        public Haspid() : base(29, 20, 30, 55, 300, 12, 0, 0, true) { }
    }

    class Fite
    {
        public void ConductingAttack(Unit attacker, Unit defending)
        {
            double baseDamageModifier;

            if (attacker.Attack < defending.Defense)
            {
                baseDamageModifier = (attacker.Attack - defending.Defense) * 0.025 * attacker.Damage;
                Math.Ceiling(baseDamageModifier);
            }
            else
            {
                baseDamageModifier = (attacker.Attack - defending.Defense) * 0.05 * attacker.Damage;
                Math.Ceiling(baseDamageModifier);
            }

            defending.Health = defending.Health - (attacker.Damage + Convert.ToInt32(baseDamageModifier));
        }
    }

    class Player
    {
        public Player()
        {
            
        }

        public void Choice(out string userInput)
        {
            const string ConsoleHaspid = "1";
            const string ConsoleGhostDragon = "2";
            const string ConsoleArchDevil = "3";
            const string ConsoleArchangel = "4";
            const string ConsoleAncientBehemoth = "5";
            Console.WriteLine($"Игрок, выберите себе юнита.");
            Console.WriteLine($" {ConsoleHaspid} - Аспид");
            Console.WriteLine($"  {ConsoleGhostDragon} - Костеной дракон");
            Console.WriteLine($" {ConsoleArchDevil} - Аспид");
            Console.WriteLine($" {ConsoleArchangel} - Аспид");
            Console.WriteLine($" {ConsoleAncientBehemoth} - Аспид");
             userInput = Console.ReadLine();
        }

    }
    class PlayerRed:Player
    {

    }

    class PlayerBlue:Player
    {

    }


    class Utilite
    {
        public static bool TryGetPositiveNumber(out int numder)
        {
            string userInput;

            do
            {
                userInput = Console.ReadLine();
            }
            while (GetInputValue(userInput, out numder));

            if (GetNumberRange(numder))
            {
                Console.WriteLine("Хорошая попытка.");
                return false;
            }

            return true;
        }

        private static bool GetInputValue(string input, out int number)
        {
            if (int.TryParse(input, out number) == false)
            {
                Console.WriteLine("Не корректный ввод.");
                return true;
            }

            return false;
        }

        private static bool GetNumberRange(int number)
        {
            int positiveValue = 0;

            if (number < positiveValue)
                return true;

            return false;
        }
    }
}
