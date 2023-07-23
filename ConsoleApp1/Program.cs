/*
 * 1 - the answer of the first question is put or patch 
 * 
 * put for editing row
 * 
 * patch for editiong field
 * 
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Users
{
    public int Age { set; get; }
    public string Name { set; get; }
    public int Id { set; get; }
    public string Email { set; get; }

}

public class Bots
{
    public int UserId { set; get; }
    public int IsDeleted { set; get; }
    public int Id { set; get; }
    public string Name { set; get; }

}

public class Scenarios
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BotId { get; set; }
    public int Order { get; set; }
}

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Users> users = new List<Users>
            {
                new Users { Id = 10, Name = "test1", Email = "test1@test.com", Age = 30 },
                new Users { Id = 20, Name = "test2", Email = "test2@test.com", Age = 25 },
            };

            List<Bots> bots = new List<Bots>
            {
                new Bots { Id = 1, IsDeleted = 0, UserId = 10, Name = "Bot 1" },
                new Bots { Id = 2, IsDeleted = 0, UserId = 20, Name = "Bot 2" },
            };

            List<Scenarios> scenarios = new List<Scenarios>
            {
                new Scenarios { BotId = 1 , Order = 5  , Id= 100 , Name="scenario 1" },
                new Scenarios { BotId = 2 , Order = 8  , Id= 200 , Name="scenario 2" },

            };

            // 2.1- 
            var botsWithUserIdGreaterThan10 = from bot in bots
            where bot.UserId == 10
            select bot;

            foreach (var bot in botsWithUserIdGreaterThan10)
            {
                Console.WriteLine($"Bot Id: {bot.Id}, Name: {bot.Name}, UserId: {bot.UserId}");
            }

            // 2.2- 

            var usersCount = users.Count();

            Console.WriteLine($"Users Count : {usersCount}");

            // 2.3-

            var joinThreeTables = from user in users
                                  join bot in bots on user.Id equals bot.UserId
                                  join scenario in scenarios on bot.Id equals scenario.BotId
                                  select new
                                  {
                                      UserId = user.Id,
                                      UserName = user.Name,
                                      Email = user.Email,
                                      Age = user.Age,
                                      BotId = bot.Id,
                                      IsDeleted = bot.IsDeleted,
                                      BotName = bot.Name,
                                      ScenarioId = scenario.Id,
                                      ScenarioName = scenario.Name,
                                      ScenarioOrder = scenario.Order
                                  }; ;

            foreach (var data in joinThreeTables)
            {
                Console.WriteLine("Row :");
                Console.Write($"User: {data.UserId}, {data.UserName}, {data.Email}, Age: {data.Age} \t");
                Console.Write($"Bot: {data.BotId}, {data.BotName}, IsDeleted: {data.IsDeleted} \t");
                Console.Write($"Scenario: {data.ScenarioId}, {data.ScenarioName}, Order: {data.ScenarioOrder} \n");

            }

            // 2.4- 

            var isDeleted = from bot in bots
                                              where bot.IsDeleted == 1
                                              select bot;

            foreach (var bot in isDeleted)
            {
                bots.Remove(bot);
            }

            
            foreach (var bot in bots)
            {
                Console.WriteLine($"Bot Id: {bot.Id}, Name: {bot.Name}, IsDeleted: {bot.IsDeleted}");
            }

            // 3-
            var OlderThan20 = from user in users
                            where user.Age > 20
                            select user;

            foreach(var user in OlderThan20)
            {
                Console.WriteLine($"User: {user.Id}, {user.Name}, {user.Email}, Age: {user.Age}");
            }


            // 4-
            var scenariosForBot = from scenario in scenarios
                                  join bot in bots on scenario.BotId equals bot.UserId
                                  orderby scenario.Order
                                  select scenario;

            foreach (var scenario in scenariosForBot)
            {
                Console.WriteLine($"Scenario Id: {scenario.Id}, Name: {scenario.Name}, Order: {scenario.Order}");
            }

            // 5-
            
            var joinedData = from bot in bots
                             join scenario in scenarios on bot.Id equals scenario.BotId
                             select new
                             {
                                 BotId = bot.Id,
                                 BotName = bot.Name,
                                 ScenarioId = scenario.Id,
                                 ScenarioName = scenario.Name,
                                 ScenarioOrder = scenario.Order
                             };

            foreach (var data in joinedData)
            {
                Console.WriteLine($"Bot: {data.BotId}, {data.BotName}");
                Console.WriteLine($"Scenario: {data.ScenarioId}, {data.ScenarioName}, Order: {data.ScenarioOrder}");
                Console.WriteLine();
            }

        }
    }
}


/*
 * 6 - the data structure is Dictionary it takes pair <key , value>
 */