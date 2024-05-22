using System;
using System.Xml.Linq;

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
            int number = Utilite.GetRandomNumber(5, 10);
        }
    }

    class Game
    {
        private PlayerBlue _playerBlue;
        private PlayerRed _playerRed;
        private bool isWork = true;

        public Game()
        {
            Create();
        }


        private void Create()
        {
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
                Console.Write($"{i + 1} - {units[i].Name}");
            }

            int numberOfUnits = units.Length;
            int unitsNumber;

            Console.WriteLine($"Красный Игрок выберите себе юнита.");

            while (Utilite.TryGetPositiveNumber(out unitsNumber, numberOfUnits) == false)
            {
                unitsNumber--;
            }

            _playerRed = new PlayerRed(units[unitsNumber]);

            Console.WriteLine($"Синий Игрок выберите себе юнита.");

            while (Utilite.TryGetPositiveNumber(out unitsNumber, numberOfUnits) == false)
            {
                unitsNumber--;
            }

            _playerBlue = new PlayerBlue(units[unitsNumber]);
        }

        public void Start()
        {
            DefiningStrikeInitiative();

            StartFate();
        }

        private void Stop()
        {
            Console.WriteLine($"Игра завершена.");
            Console.ReadKey();

            isWork = false;
        }

        private void DefiningStrikeInitiative()
        {
            Console.WriteLine($"Определение очередности ходов. Юнит с большей скоростью наносит атаку быстрее: " +
                $"{_playerRed.Unit.Name} имеет - {_playerRed.Unit.Speed} скорость" +
                $"{_playerBlue.Unit.Name} имеет - {_playerBlue.Unit.Speed} скорость");


            if (_playerRed.Unit.Speed >= _playerBlue.Unit.Speed)
            {
                _playerRed.TakeInitiative();
                Console.WriteLine($"Первым атакует красный игрок");
            }
            else
            {
                _playerBlue.TakeInitiative();
                Console.WriteLine($"Первым атакует Синий игрок");
            }
        }

        private void StartFate()
        {
            while (_playerRed.Unit.Health > 0 && _playerBlue.Unit.Health > 0)
            {

            }
        }
    }

    class Unit
    {

        public Unit(string name, int attack, int defense, int minDamage, int maxDamage, int health, int speed, int morale, int luck, int abilityCounterattack)
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
        public int AbilityCounterattack { get; private set; }

        public virtual void UseDebaff(Player opponent) { }

        public virtual void CalculateDamege()
        {
            Damage = Utilite.GetRandomNumber(MinDamage, MaxDamage);
        }


        public virtual void GetParameter(string name, int value)
        {
            if (name == "Defense")
            {
                Defense = value;
                return;
            }
            if (name == "Morale")
            {
                Morale = value;
                return;
            }
            if (name == "Luck")
            {
                Luck = value;
                return;
            }
            if (name == "Health")
            {
                Health = value;
                return;
            }
            if (name == "AbilityCounterattack")
            {
                AbilityCounterattack = value;
                return;
            }
            if (name == "MinDamage")
            {
                MinDamage = value;
                return;
            }
            if (name == "MaxDamage")
            {
                MaxDamage = value;
                return;
            }
            if (name == "Damage")
            {
                Damage = value;
                return;
            }

        }
    }

    class AncientBehemoth : Unit
    {
        public AncientBehemoth() : base("Древнее чудовище", 19, 19, 30, 50, 1300, 9, 0, 0, 1) { }

        public override void UseDebaff(Player opponent)
        {
            int defense = (int)(opponent.Unit.Defense - (Math.Round(0.8 * opponent.Unit.Defense) - 1));

            opponent.Unit.GetParameter("Defense", defense);
        }
    }

    class Archangel : Unit
    {
        public Archangel() : base("Архангел", 30, 30, 50, 50, 1250, 18, 1, 0, 1) { }

        public override void UseDebaff(Player opponent)
        {
            if (opponent.Unit.Name == "Архидьявол")
            {
                int minDamage = (int)(MinDamage + Math.Round(MinDamage * 0.5));
                int maxDamage = (int)(MaxDamage + Math.Round(MaxDamage * 0.5));

                GetParameter("MinDamage", minDamage);
                GetParameter("MaxDamage", maxDamage);
            }
        }
    }

    class ArchDevil : Unit
    {
        public ArchDevil() : base("Архидьявол", 26, 28, 30, 40, 1200, 17, 0, 0, 1) { }

        public override void UseDebaff(Player opponent)
        {
            if (opponent.Unit.Name == "Архангел")
            {
                int minDamage = (int)(MinDamage + Math.Round(MinDamage * 0.5));
                int maxDamage = (int)(MaxDamage + Math.Round(MaxDamage * 0.5));

                GetParameter("MinDamage", minDamage);
                GetParameter("MaxDamage", maxDamage);
            }

            int luck = opponent.Unit.Luck - 1;
            int abilityCounterattack = opponent.Unit.AbilityCounterattack - 1;

            opponent.Unit.GetParameter("Luck", luck);
            opponent.Unit.GetParameter("AbilityCounterattack", abilityCounterattack);
        }
    }

    class GhostDragon : Unit
    {
        public GhostDragon() : base("Костяной дракон", 19, 17, 25, 50, 1200, 14, 0, 0, 1) { }

        public override void UseDebaff(Player opponent)
        {
            int health = (int)(opponent.Unit.Health - Math.Round(opponent.Unit.Health * 0.3));
            int morale = opponent.Unit.Morale - 1;

            opponent.Unit.GetParameter("Health", health);
            opponent.Unit.GetParameter("Morale", morale);
        }
    }

    class Haspid : Unit
    {
        public Haspid() : base("Аспид", 29, 20, 30, 55, 1300, 12, 0, 0, 1) { }

        public override void UseDebaff(Player opponent)
        {
            int health = (int)(opponent.Unit.Health - Math.Round(opponent.Unit.Health * 0.1));

            opponent.Unit.GetParameter("Health", health);
        }

        public override void CalculateDamege()
        {
            int damage = Utilite.GetRandomNumber(MinDamage, MaxDamage);

            damage = damage + (int)Math.Round(Math.Sqrt(2300 / (Health + 1300) - 1) * 100);

            GetParameter("Damage", damage);
        }
    }

    class CombatMechanics
    {
        public void Attack(Player attacker, Player defending)
        {
            double baseDamageModifier;

            if (attacker.Unit.Attack < defending.Unit.Defense)
            {
                baseDamageModifier = (attacker.Unit.Attack - defending.Unit.Defense) * 0.025 * attacker.Unit.Damage;
                Math.Ceiling(baseDamageModifier);
            }
            else
            {
                baseDamageModifier = (attacker.Unit.Attack - defending.Unit.Defense) * 0.05 * attacker.Unit.Damage;
                Math.Ceiling(baseDamageModifier);
            }

            int health = defending.Unit.Health - (attacker.Unit.Damage + (int)Math.Round(baseDamageModifier));

            defending.Unit.GetParameter("Health", health);
        }
        public void Counterattack(Player attacker, Player defending)
        {

        }
    }

    class Player
    {
        public Player(Unit unit, int initiative)
        {
            Unit = unit;
            Initiative = initiative;
        }

        public Unit Unit { get; private set; }
        public int Initiative { get; private set; }

        public void TakeInitiative()
        {
            Initiative = 1;
        }

    }
    class PlayerRed : Player
    {
        public PlayerRed(Unit unit) : base(unit, 0) { }
    }

    class PlayerBlue : Player
    {
        public PlayerBlue(Unit unit) : base(unit, 0) { }
    }


    class Utilite
    {
        private static Random s_random = new Random();

        public static int GetRandomNumber(int minValue, int maxValue)
        {
            return s_random.Next(minValue, maxValue);
        }
        public static bool TryGetPositiveNumber(out int numder, int upperLimitChoice)
        {
            string userInput;

            do
            {
                userInput = Console.ReadLine();
            }
            while (TryGetNumber(userInput, out numder));

            if (IsNegativeNumber(numder))
            {
                Console.WriteLine("Хорошая попытка.");
                return false;
            }

            if (numder > upperLimitChoice)
            {
                Console.WriteLine("Не правильно выбран юнит.");
                return false;
            }

            return true;
        }

        private static bool TryGetNumber(string input, out int number)
        {
            if (int.TryParse(input, out number) == false)
            {
                Console.WriteLine("Не корректный ввод.");
                return true;
            }

            return false;
        }

        private static bool IsNegativeNumber(int number)
        {
            return number > 1;
        }
    }
}
