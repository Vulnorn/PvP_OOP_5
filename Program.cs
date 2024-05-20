using System;
using System.Collections;
using System.Collections.Generic;

// Archangel At - 30; Def -30 ; Dam - 50-50; Hp - 1250; Sed - 18 ; Spel - 30% morals. 150% dam ArchDevil
// ArchDevil At - 26; Def -28 ; Dam - 30-40; Hp - 1200; Sed - 17 ; Spel - -30% udaca. 150% dam Archangel, ne imeet otbetnogo udara
// GhostDragon At - 19; Def - 17 ; Dam - 25-50; Hp - 200; Sed - 14 ; Spel - -30% morals. 20% spell Старость (снижает здоровье на 50% на 3 раунда)
// AncientBehemoth At - 19; Def - 19 ; Dam - 30-50; Hp - 300; Sed - 9 ; Spel - Снижает броню цели при ударе, (80%*защита цели - 1) округление вверх.
// Haspid At - 29; Def - 20 ; Dam - 30-55; Hp - 300; Sed - 12 ; Spel - 30% отравить цель - здоровье снижается на 10% за раунт в течении 3х раундов. spell  - Месть увеличение урона в зависимости от недостающего здоровья - [Корень квадратный ((HP) / (HPnow + HP) - 1) * 100%)]

namespace PvP_OOP_5_Players
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Work();
        }
    }

    class Game
    {
        public bool IsWork { get; private set; }
        public void Work()
        {
            while (IsWork)
            {
                Start();
            }
        }

        private void Start()
        {


            IsWork = true;

            Unit[] units =
                {
            new AncientBehemoth(),
            new Archangel(),
            new ArchDevil(),
            new GhostDragon(),
            new Haspid()
            };

            for (int i = 0; i < units.Length; i++)
            {
                Console.Write($"{i + 1} - ");
                units[i].ShowName();
            }

            int unitsnumber;

            Console.WriteLine($"Красный Игрок выберите себе юнита.");

            if (Utilite.TryGetPositiveNumber(out unitsnumber) == false || unitsnumber <= units.Length)
            {
                PlayerRed playerRed = new PlayerRed(units[unitsnumber]);
            }
            else
            {
                Stop();
                return;
            }

            Console.WriteLine($"Синий Игрок выберите себе юнита.");

            if (Utilite.TryGetPositiveNumber(out unitsnumber) == false || unitsnumber <= units.Length)
            {
                PlayerBlue playerBlue = new PlayerBlue(units[unitsnumber]);
            }
            else
            {
                Stop();
                return;
            }

            StartFight();
        }

        private void StartFight()
        {
            Fighth fighth = new Fighth(playerRed, playerBlue); 
        }

        private void Stop()
        {
            Console.WriteLine($"Игра завершена.");
            Console.ReadKey();

            IsWork = false;
        }
    }

    class Fighth
    {
        private PlayerRed _playerRed;
        private PlayerBlue _playerBlue;

        public Fighth (PlayerRed playerRed, PlayerBlue playerBlue)
        {
            _playerRed = playerRed;
            _playerBlue = playerBlue;
        }
    }

    class Unit
    {

        public Unit(string name, int attack, int defense, int minDamage, int maxDamage, int health, int speed, int morale, int luck, bool abilityCounterattack)
        {
            Name = name;
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

        public string Name { get; private set; }
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

        public void ShowName ()
        {
            Console.WriteLine(Name);
        }
    }

    class AncientBehemoth : Unit
    {
        public AncientBehemoth() : base("Древнее чудовище", 19, 19, 30, 50, 1300, 9, 0, 0, true) { }

        //public void ApplyAbilityAttack(int defense)
        //{
        //    defense = defense - (0.8 * defense - 1);
        //    return defense;
        //}
    }

    class Archangel : Unit
    {
        public Archangel() : base("Архангел", 30, 30, 50, 50, 1250, 18, 1, 0, true) { }
    }

    class ArchDevil : Unit
    {
        public ArchDevil() : base("Архидьявол", 26, 28, 30, 40, 1200, 17, 0, 0, true) { }
    }

    class GhostDragon : Unit
    {
        public GhostDragon() : base("Костяной дракон", 19, 17, 25, 50, 1200, 14, 0, 0, true) { }
    }

    class Haspid : Unit
    {
        public Haspid() : base("Аспид", 29, 20, 30, 55, 1300, 12, 0, 0, true) { }
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
        private Unit _unit;

        public Player(Unit unit) 
        { 
            _unit= unit;
        }

    }
    class PlayerRed : Player
    {
        public PlayerRed(Unit unit) : base(unit) { }
    }

    class PlayerBlue : Player
    {
        public PlayerBlue(Unit unit) : base(unit) { }
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
