using System;

namespace PvP_OOP_5_Players
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    class Game
    {
        private PlayerBlue _playerBlue;
        private PlayerRed _playerRed;

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
                Console.WriteLine($"{i + 1} - {units[i].Name}");
            }

            int numberOfUnits = units.Length;
            int unitsNumber;
            bool IsGenerated = false;

            Console.WriteLine($"Красный Игрок выберите себе юнита.");

            while (IsGenerated == false)
            {
                if (Utilite.TryGetPositiveNumber(out unitsNumber, numberOfUnits) == true)
                {
                    unitsNumber--;
                    _playerRed = new PlayerRed(units[unitsNumber]);
                    IsGenerated = true;
                }
            }

            Console.WriteLine($"Красный игрок выбрал {_playerRed.Unit.Name}");
            Console.WriteLine($"Синий Игрок выберите себе юнита.");
            IsGenerated = false;

            while (IsGenerated == false)
            {
                if (Utilite.TryGetPositiveNumber(out unitsNumber, numberOfUnits) == true)
                {
                    unitsNumber--;
                    _playerBlue = new PlayerBlue(units[unitsNumber]);
                    IsGenerated = true;
                }
            }

            Console.WriteLine($"Синий игрок выбрал {_playerBlue.Unit.Name}");
            Console.ReadKey();
        }

        public void Start()
        {
            DefiningStrikeInitiative();
            _playerBlue.Unit.UseDebaff(_playerRed);
            _playerRed.Unit.UseDebaff(_playerBlue);
            StartFate();
        }

        private void DefiningStrikeInitiative()
        {
            Console.WriteLine($"Определение очередности ходов. Юнит с большей скоростью наносит атаку быстрее: " +
                $"\n{_playerRed.Unit.Name} имеет - {_playerRed.Unit.Speed} скорость" +
                $"\n{_playerBlue.Unit.Name} имеет - {_playerBlue.Unit.Speed} скорость");

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

            Console.ReadKey();
        }

        private void StartFate()
        {
            while (_playerRed.Unit.Health >= 0 && _playerBlue.Unit.Health >= 0)
            {
                if (_playerRed.Initiative == 1)
                {
                    CombatMechanics.Attack(_playerRed, _playerBlue);
                    CheckHealth();
                    CombatMechanics.Counterattack(_playerRed, _playerBlue);
                    CheckHealth();
                    CombatMechanics.Attack(_playerBlue, _playerRed);
                    CheckHealth();
                    CombatMechanics.Counterattack(_playerBlue, _playerRed);
                    CheckHealth();
                    ShowHealth();
                }
                else
                {
                    CombatMechanics.Attack(_playerBlue, _playerRed);
                    CheckHealth();
                    CombatMechanics.Counterattack(_playerBlue, _playerRed);
                    CheckHealth();
                    CombatMechanics.Attack(_playerRed, _playerBlue);
                    CheckHealth();
                    CombatMechanics.Counterattack(_playerRed, _playerBlue);
                    CheckHealth();
                    ShowHealth();
                }

                Console.ReadKey();
            }

            ShowHealth();
            Console.WriteLine($"Игра завершена.");
            Console.ReadKey();
        }

        private void CheckHealth()
        {
            if (_playerRed.Unit.Health >= 0 && _playerBlue.Unit.Health >= 0)
                return;
        }

        private void ShowHealth()
        {
            Console.WriteLine($"У Крассного юнита {_playerRed.Unit.Name} осталось {_playerRed.Unit.Health} здоровья" +
                $"\nУ Синего юнита {_playerBlue.Unit.Name} осталось  {_playerBlue.Unit.Health}  здоровья" +
                $"\n______");
        }
    }

    abstract class Unit
    {
        public Unit(string name, int attack, int defense, int minDamage, int maxDamage, int health, int speed, int abilityCounterattack)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Health = health;
            Speed = speed;
            AbilityCounterattack = abilityCounterattack;
        }

        public string Name { get; protected set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }
        public int Damage { get; private set; }
        public int Health { get; private set; }
        public int Speed { get; private set; }
        public int AbilityCounterattack { get; private set; }

        public abstract void UseDebaff(Player opponent);

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
        public AncientBehemoth() : base("Древнее чудовище", 19, 19, 30, 50, 1300, 9, 1) { }

        public override void UseDebaff(Player opponent)
        {
            double modifierPercentageRatio = 0.8;
            int baseModifier = 1;
            int defense = ((opponent.Unit.Defense - (int)Math.Round(modifierPercentageRatio * opponent.Unit.Defense)) - baseModifier);

            opponent.Unit.GetParameter("Defense", defense);
        }
    }

    class Archangel : Unit
    {
        public Archangel() : base("Архангел", 30, 30, 50, 50, 1250, 18, 1) { }

        public override void UseDebaff(Player opponent)
        {
            if (opponent.Unit.Name == "Архидьявол")
            {
                double modifierPercentageRatio = 0.5;
                int minDamage = (int)(MinDamage + Math.Round(MinDamage * modifierPercentageRatio));
                int maxDamage = (int)(MaxDamage + Math.Round(MaxDamage * modifierPercentageRatio));

                GetParameter("MinDamage", minDamage);
                GetParameter("MaxDamage", maxDamage);
            }
        }
    }

    class ArchDevil : Unit
    {
        public ArchDevil() : base("Архидьявол", 26, 28, 30, 40, 1200, 17, 1) { }

        public override void UseDebaff(Player opponent)
        {
            if (opponent.Unit.Name == "Архангел")
            {
                double modifierPercentageRatio = 0.5;
                int minDamage = (int)(MinDamage + Math.Round(MinDamage * modifierPercentageRatio));
                int maxDamage = (int)(MaxDamage + Math.Round(MaxDamage * modifierPercentageRatio));

                GetParameter("MinDamage", minDamage);
                GetParameter("MaxDamage", maxDamage);
            }

            int abilityCounterattack = opponent.Unit.AbilityCounterattack - 1;
           opponent.Unit.GetParameter("AbilityCounterattack", abilityCounterattack);
        }
    }

    class GhostDragon : Unit
    {
        public GhostDragon() : base("Костяной дракон", 19, 17, 25, 50, 1200, 14, 1) { }

        public override void UseDebaff(Player opponent)
        {
            double modifierPercentageRatio = 0.3;
            int health = (int)(opponent.Unit.Health - Math.Round(opponent.Unit.Health * modifierPercentageRatio));
            opponent.Unit.GetParameter("Health", health);
        }
    }

    class Haspid : Unit
    {
        public Haspid() : base("Аспид", 29, 20, 30, 55, 1300, 12, 1) { }

        public override void UseDebaff(Player opponent)
        {
            double modifierPercentageRatio = 0.1;
            int health = (int)(opponent.Unit.Health - Math.Round(opponent.Unit.Health * modifierPercentageRatio));
            opponent.Unit.GetParameter("Health", health);
        }

        public override void CalculateDamege()
        {
            float healthHaspidBase = 1300;
            float quantityUnits = 1;
            float modifierQuantityUnits = 1;
            float modifierPercentageRatio = 100;
            int damage = Utilite.GetRandomNumber(MinDamage, MaxDamage);
            damage += (int)Math.Round(Math.Sqrt(healthHaspidBase * (quantityUnits + modifierQuantityUnits) / (Health * quantityUnits + healthHaspidBase) - modifierQuantityUnits) * modifierPercentageRatio);

            GetParameter("Damage", damage);
        }
    }

    class CombatMechanics
    {
        public static void Attack(Player attacker, Player defending)
        {
            int baseDamageModifier;
            attacker.Unit.CalculateDamege();

            if (attacker.Unit.Attack < defending.Unit.Defense)
            {
                baseDamageModifier = (int)Math.Round((attacker.Unit.Attack - defending.Unit.Defense) * 0.025 * attacker.Unit.Damage);
            }
            else
            {
                baseDamageModifier = (int)Math.Round((attacker.Unit.Attack - defending.Unit.Defense) * 0.05 * attacker.Unit.Damage);
            }

            int health = defending.Unit.Health - (attacker.Unit.Damage + baseDamageModifier);
            Console.WriteLine($"{attacker.Unit.Name} наносит {attacker.Unit.Damage + baseDamageModifier} Урона");
            defending.Unit.GetParameter("Health", health);
        }

        public static void Counterattack(Player attacker, Player defending)
        {
            if (defending.Unit.AbilityCounterattack == 1)
            {
                int baseDamageModifier;
                attacker.Unit.CalculateDamege();

                if (defending.Unit.Attack < attacker.Unit.Defense)
                {
                    baseDamageModifier = (int)Math.Round((defending.Unit.Attack - attacker.Unit.Defense) * 0.025 * defending.Unit.Damage);
                }
                else
                {
                    baseDamageModifier = (int)Math.Round((defending.Unit.Attack - attacker.Unit.Defense) * 0.05 * defending.Unit.Damage);
                }

                int health = attacker.Unit.Health - (defending.Unit.Damage + (baseDamageModifier));
                attacker.Unit.GetParameter("Health", health);
                Console.WriteLine($"Ответный удар - {defending.Unit.Name} наносит {defending.Unit.Damage + baseDamageModifier} Урона");
            }
        }
    }

    class Player
    {
        public Player(Unit unit, int initiative)
        {
            Unit = unit;
            Initiative = initiative;
        }

        public Unit Unit { get; protected set; }
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
            return number < 0;
        }
    }
}