using System;

namespace project
{
  public class Rand
  {

//losowanie liczb
    public int Run(int min, int max)  
    {
      int range = (max - min) + 1;
      Random rng = new Random();
      return min + rng.Next() % range;
    }
  }


//atrybuty bohaterów 
  public class Hero
  {
    public string Name; 
    private int Strength;
    private int Dexterity; //zrecznosc
    private int Intelligence;
    public double HP;
    public double MP;

    private void Init(int strength = 10, int dexterity = 10, int intelligence = 10)
    {
      this.Strength = strength;
      this.Dexterity = dexterity; //zrecznosc
      this.Intelligence = intelligence;
      HP = 50 + strength;
      MP = 10 + (3 * intelligence);
    }

    public int GetStrength() { return this.Strength; }
    public int GetDexterity() { return this.Dexterity; }
    public int GetIntelligence() { return this.Intelligence; }

    public void UpStrength() { this.Strength += 5; this.HP += 5; }
    public void UpDexterity() { this.Dexterity += 5; } 
    public void UpIntelligence() { this.Intelligence += 5; this.MP += (3 * this.Intelligence); }

    public void Medicines() { 
      if (this.MP >= 15) {
        this.HP += 10; this.MP -= 15;
      } else {
        Console.WriteLine("You don't have enought mana to Medicines! You lose your turn...");
      }
       }



//kasta bohatera
    public Hero(string name, string myclass) 
    {
      Name = name;
      switch(myclass.ToLower())
      {
        case "warrior": Init(15, 10, 5); break;
        case "assassin": Init(5, 15, 10); break;
        case "sorcerer": Init(5, 5, 20); break;
        default: Init(); break;
      }
    }


//funkcja ataku
    public void Attack(Hero enemy)
    {
      Rand rand = new Rand();
      double damage = Strength * rand.Run(5, 10) / 10; //obliczanie ataku 

      if(rand.Run(0, 100) > enemy.GetDexterity()) //losowanie numerku 0-100 jezeli wiekszy to obrywa przeciwnik
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(" Bang!");
        Console.ResetColor();
        enemy.HP -= damage;
      } else {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(" Dodge!"); //unik
        Console.ResetColor();
      } 
    }


//super attak
    public void SuperAttack(Hero enemy) {
      
      if (this.MP >= 35) {
        Rand rand = new Rand();
        double damage = 3 * (Strength * rand.Run(5, 10) / 10);

        Console.WriteLine($"{this.Name} is using Super Attack!");

        if (rand.Run(0, 100) > enemy.GetDexterity()) {
          Console.ForegroundColor = ConsoleColor.DarkRed;
          Console.WriteLine(" Super Bang!");
          Console.ResetColor();
          enemy.HP -= damage;
        } else {
          Console.ForegroundColor = ConsoleColor.DarkYellow;
          Console.WriteLine("Super Attack dodged!");
          Console.ResetColor();
        }

        this.MP -= 35;

      } else {
        Console.WriteLine("You don't have enought mana to Super Attack! You lose your turn...");
      }

    }


  public void EnemyWeakness(Hero enemy) {
    if (this.MP >= 20) {

      enemy.HP -= 5;
      enemy.MP -= 5;
      enemy.Strength = Math.Max(0, enemy.Strength - 2);
      enemy.Dexterity = Math.Max(0, enemy.Dexterity - 2);
      enemy.Intelligence = Math.Max(0, enemy.Intelligence - 2);

      this.MP -= 20;
      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.WriteLine($"{this.Name} used EnemyWeakness on {enemy.Name}!");
      Console.ResetColor();

    } else {
      Console.WriteLine("You don't have enought mana to EnemyWeakness! You lose your turn...");
    }
  }

//funkcja ulepszenia
    public void LevelUp()
    {
      Console.Write("  1:Strength, 2:Dexterity, 3:Intelligence ... ");
      int opt = int.Parse(Console.ReadLine());

      switch(opt)
      {
        case 1: UpStrength(); break;
        case 2: UpDexterity(); break;
        case 3: UpIntelligence(); break;
      }

      Console.WriteLine();
    }


//zklęcia/regeneracja many
    public void Spell(Hero enemy)
    {

      Console.Write(" 1:Medicines +10HP, -15MP, 2:SuperAttack -35MP, 3:EnemyWeaknes -20MP ... ");
      int opt = int.Parse(Console.ReadLine());

      switch(opt)
      {
        case 1: Medicines(); break; //dopisac jezeli jest mana
        case 2: SuperAttack(enemy); break; 
        case 3: EnemyWeakness(enemy); break;
        default: Console.WriteLine("Nieznane zaklęcie."); break;
      }
    }
  }



//mechanizm gry
  class Program
  {
    static void Main(string[] args)
    {
      int tour = 1;
      int gameRound = 1;

    //tytuł
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("         Little 1 vs 1");
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine();
    System.Threading.Thread.Sleep(2000);

    //wybór imienia i kasty dla player1
      Console.WriteLine(" Player 1, What is your name? ...");  
      string playerOneName = Console.ReadLine();

      Console.WriteLine(" Now " + playerOneName + ", choose your class (1:warrior, 2:assassin, 3:sorcerer) ...");
      string playerOneClass = Console.ReadLine().ToLower();

      while (playerOneClass != "warrior" && playerOneClass != "assassin" && playerOneClass != "sorcerer") {
        Console.WriteLine(" Invalid class. Please choose again (warrior, assassin, sorcerer):");
        playerOneClass = Console.ReadLine().ToLower();
      }

      Console.WriteLine();

    //wybór imienia i kasty dla player2
      Console.WriteLine(" Player 2, What is your name? ...");  
      string playerTwoName = Console.ReadLine();

      Console.WriteLine(" Now " + playerTwoName + ", choose your class (warrior, assassin, sorcerer) ...");
      string playerTwoClass = Console.ReadLine().ToLower();

      Console.WriteLine();
      System.Threading.Thread.Sleep(1000);
      Console.Clear();

      Console.WriteLine();
      Console.WriteLine("______________________________________________________________________________");
      Console.WriteLine();

    //pierwszy bohater
      Hero hero1 = new Hero(playerOneName, playerOneClass);
      Console.WriteLine(playerOneName + " Str:{0} Dex:{1} Int:{2} HP:{3} MP:{4}", hero1.GetStrength(), hero1.GetDexterity(), hero1.GetIntelligence(), hero1.HP, hero1.MP);

    //drugi bohater
      Hero hero2 = new Hero(playerTwoName, playerTwoClass);
      Console.WriteLine(playerTwoName + " Str:{0} Dex:{1} Int:{2} HP:{3} MP:{4}", hero2.GetStrength(), hero2.GetDexterity(), hero2.GetIntelligence(), hero2.HP, hero2.MP);

      Console.WriteLine();

      while(hero1.HP > 0 && hero2.HP > 0)
      {
        if (tour == 1) {
          Console.WriteLine(" ===== Round: " + gameRound + " ===== ");
        }

        if(tour == 1) Console.WriteLine(" Your Turn: " + hero1.Name);
        else Console.WriteLine("Your Turn: " + hero2.Name);
        Console.WriteLine();

        Console.Write(" 1:Attack, 2:Spell, 3:LevelUp ... ");
        int opt = int.Parse(Console.ReadLine());

        switch(opt)
        {
          case 1:
            if(tour == 1) hero1.Attack(hero2);
            else hero2.Attack(hero1);
          break;

          case 2:
            if(tour == 1) hero1.Spell(hero2);
            else hero2.Spell(hero1);
          break;

          case 3:
            if(tour == 1) hero1.LevelUp();
            else hero2.LevelUp();
          break;
        }
         

        if(tour == 2) {
          gameRound += 1;
          hero1.HP += 1; //regeneracja co runde +1 HP
          hero2.HP += 1;
        }
        Console.WriteLine(hero1.Name + " Str:{0} Dex:{1} Int:{2} HP:{3} MP:{4}", hero1.GetStrength(), hero1.GetDexterity(), hero1.GetIntelligence(), hero1.HP, hero1.MP);
        Console.WriteLine(hero2.Name + " Str:{0} Dex:{1} Int:{2} HP:{3} MP:{4}", hero2.GetStrength(), hero2.GetDexterity(), hero2.GetIntelligence(), hero2.HP, hero2.MP);
        Console.WriteLine();

        tour++;
        if(tour > 2) tour = 1;
      }

      // komunikat o zwycieztwie
      if(hero1.HP <= 0 || hero2.HP <= 0) {
        if(hero1.HP <= 0) {
            Console.WriteLine(" The Winner is " + hero2.Name + " !!!");
        } else {
            Console.WriteLine(" The Winner is " + hero1.Name + " !!!");
        }
      }
    }
  }
}

opisz mi tą grę bo potrzebuje opisu README na githubie. powedz od myslnikow jakie sa zasady, jakie zaklecie co robi itp.
