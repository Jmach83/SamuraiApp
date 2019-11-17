using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeUI
{
    public class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        static void Main(string[] args)
        {
            //InsertSamurai();
            //InsertMultplySamurais();
            //InsertMultplyDiffrentObjects();
            //SimpleSamuraiQuery();
            //MoreQueries();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultiplySamurai();
            //MultiplyDatabaseOperations();
            //InsertBattle();
            //QueryAndUpdateBattle_Disconnected();
            //AddMoreSamurais();
            //DeleteWhileTracked();
            //DeleteWhileNotTracked();
            //DeleteUsingId(3);
            //InsertNewPkFkGraph();
            //InsertNewPkFkGraphMultiplyChildren();
            //AddChildToExistingObjectWhileTracked();
            //AddChildToExistingObjectWhileNotTracked(1);
            //EargerLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //var dynamicList = ProjectDynamic();
            //ProjectsWithQoutes();
            //FilteringWithRelatedData();
            //ModifyingRelatedDataWhenTracked();
            //ModifyingRelatedDataWhenNotTracked();
            //PrepopulateSamuraisAndBattles();
            //JoinBattleAndSamurai();
            //EnlistSamuraiIntoBattle();
            //EnlistSamuraiIntoBattleUntracked();
            //AddNewSamuraiViaDisconnectedBatlleObject();
            //GetSamuraiWithBattles();
            //RemoveJoinBetweenSamuraiAndBattleSimple();
            //RemoveBattleFromSamurai();
            //AddSecretIdentityUsingSamuraiId();
            //AddSecretIdentityToExistingSamurai();
            //EditASecretIdentity();
            //ReplaceASecretIdentity();
            //ReplaceASecretIdentityNotTracked();
            //ReplaceASecretIdentityNotInMemory();
            //CreateSamurai();
            //RetrieveSamuraiCreatedPastWeek();
            //CreateThenEditSamuraiWithQoute();
            //CreateSamuraiWithBetterName();
            //GetAllSamurais();
            //RetrieveAndUpdateBetterName();
            //CreateAndFixUpNullBetterName();
            ReplaceBetterName();

        }

        private static void ReplaceBetterName()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Chrisjen");
            _context.Entry(samurai).Reference(s => s.BetterName).TargetEntry.State = EntityState.Detached;
            samurai.BetterName = PersonFullName.Create("Shohreh", "Aghdashloo");
            _context.Samurais.Update(samurai);
            _context.SaveChanges();
        }

        private static void CreateAndFixUpNullBetterName()
        {
            _context.Samurais.Add(new Samurai { Name = "Chrisjen" });
            _context.SaveChanges();
            _context = new SamuraiContext();
            var persistedSamurai = _context.Samurais.FirstOrDefault(s => s.Name == "Chrisjen");
            if(persistedSamurai is null) { return;  }
            if (persistedSamurai.BetterName.isEmpty())
            {
                persistedSamurai.BetterName = null;
            }
        }

        private static void RetrieveAndUpdateBetterName()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.BetterName.GivenName == "Jack");
            //samurai.BetterName.GivenName = "Jill";
            _context.SaveChanges();
        }

        private static void GetAllSamurais()
        {
            var allSamurais = _context.Samurais.ToList();
        }

        private static void CreateSamuraiWithBetterName()
        {
            var samurai = new Samurai
            {
                Name = "Jack le Black",
                BetterName = PersonFullName.Create("Jack", "Black")
            };

            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void CreateThenEditSamuraiWithQoute()
        {
            var samurai = new Samurai { Name = "Ronin" };
            var qoute = new Qoute { Text = "Aren't I MARVELous?" };
            samurai.Qoutes.Add(qoute);
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
            qoute.Text += " See what I did there?";
            _context.SaveChanges();
        }

        private static void RetrieveSamuraiCreatedPastWeek()
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            //var newSamurais = _context.Samurais.Where(s => EF.Property<DateTime>(s, "Created") >= oneWeekAgo)
                //.ToList();

            var samuraisCreated = _context.Samurais
                .Where(s => EF.Property<DateTime>(s, "Created") >= oneWeekAgo)
                .Select(s => new { s.Id, s.Name, Created = EF.Property<DateTime>(s, "Created") }) //To get Created propety that is not part of Samurai Model
                .ToList();
        }

        private static void CreateSamurai()
        {
            var samurai = new Samurai { Name = "Ronin" };
            _context.Add(samurai);
            //Done in samuraiContext now, savechange override
            //var timestamp = DateTime.Now;
            //_context.Entry(samurai).Property("Created").CurrentValue = timestamp;
            //_context.Entry(samurai).Property("LastModified").CurrentValue = timestamp;
            _context.SaveChanges();
        }

        private static void ReplaceASecretIdentityNotInMemory()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.SecretIdentity != null);
            samurai.SecretIdentity = new SecretIdentity { RealName = "Bobbie Draper" };
            _context.SaveChanges();
        }

        private static void ReplaceASecretIdentityNotTracked()
        {
            Samurai samurai;
            using(var seperateOperation = new SamuraiContext())
            {
                samurai = seperateOperation.Samurais.Include(s => s.SecretIdentity)
                    .FirstOrDefault(s => s.Id == 42);
            }
            samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };
            _context.Attach(samurai);
            _context.SaveChanges();

        }

        private static void ReplaceASecretIdentity()
        {
            var samurai = _context.Samurais.Include(s => s.SecretIdentity)
                .FirstOrDefault(s => s.Id == 42);
            samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };
            _context.SaveChanges();
        }

        private static void EditASecretIdentity()
        {
            var samurai = _context.Samurais.Include(s => s.SecretIdentity)
                .FirstOrDefault(s => s.Id == 42);
            samurai.SecretIdentity.RealName = "T'Challa";
            _context.SaveChanges();
        }

        private static void AddSecretIdentityToExistingSamurai()
        {
            Samurai samurai;
            using(var seperateOperation = new SamuraiContext())
            {
                samurai = seperateOperation.Samurais.Find(42);
            }
            samurai.SecretIdentity = new SecretIdentity { RealName = "Julia" };
            _context.Attach(samurai);
            _context.SaveChanges();
        }

        private static void AddSecretIdentityUsingSamuraiId()
        {
            //Note: Samurai 1 does not have a secret identity yet!
            var identity = new SecretIdentity { SamuraiId = 42, };
            _context.Add(identity);
            _context.SaveChanges();
        }

        private static void RemoveBattleFromSamurai()
        {
            //Goal: remove join between Katsushiro(Id=45) and Siege of Osaka (Id=5)
            var samurai = _context.Samurais.Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle)
                .SingleOrDefault(s => s.Id == 42);
            var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 5);
            samurai.SamuraiBattles.Remove(sbToRemove); //remove via List<T>
            //_context.Remove(sbToRemove); //remove using dbContext
            _context.ChangeTracker.DetectChanges(); //for debugging
            _context.SaveChanges();
        }

        private static void RemoveJoinBetweenSamuraiAndBattleSimple()
        {
            var join = new SamuraiBattle { BattleId = 5, SamuraiId = 42 };
            _context.Remove(join);
            _context.SaveChanges();
        }

        private static void GetSamuraiWithBattles()
        {
            var samuraiWithBattles = _context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle).FirstOrDefault(s => s.Id == 42);
            var battle = samuraiWithBattles.SamuraiBattles.First().Battle;
            var allTheBattles = new List<Battle>();
            foreach(var samuraiBattle in samuraiWithBattles.SamuraiBattles)
            {
                allTheBattles.Add(samuraiBattle.Battle);
            }
        }

        private static void AddNewSamuraiViaDisconnectedBatlleObject()
        {
            Battle battle;
            using(var seperateOperation = new SamuraiContext())
            {
                battle = seperateOperation.Battles.Find(5);
            }
            var newSamurai = new Samurai { Name = "Sampsonsan" };
            battle.SamuraiBattles.Add( new SamuraiBattle { Samurai = newSamurai });
            _context.Attach(battle);
            _context.SaveChanges();
        }

        private static void EnlistSamuraiIntoBattleUntracked()
        {
            Battle battle;
            using(var seperateOperation = new SamuraiContext())
            {
                battle = seperateOperation.Battles.Find(5);
            }
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 42 });
            _context.Attach(battle); //begin track entity
            _context.ChangeTracker.DetectChanges(); //to show debug info
            _context.SaveChanges();
        }

        private static void EnlistSamuraiIntoBattle()
        {
            var battle = _context.Battles.Find(5);
            battle.SamuraiBattles
                .Add(new SamuraiBattle { SamuraiId = 42 });
            _context.SaveChanges();
        }

        private static void JoinBattleAndSamurai()
        {
            //Kikuchiyo is is 42, Siege of Osaka id is 5
            var sbJoin = new SamuraiBattle { SamuraiId = 42, BattleId = 5 };
            _context.Add(sbJoin);
            _context.SaveChanges();
        }

        private static void PrepopulateSamuraisAndBattles()
        {
            _context.AddRange(
                new Samurai { Name = "Kikuchiyo" },
                new Samurai { Name = "Kambei Shimada" },
                new Samurai { Name = "Shichiroji" },
                new Samurai { Name = "Katsushiro Okamoto" },
                new Samurai { Name = "Heihachi Hayashida" },
                new Samurai { Name = "Kyuzo" },
                new Samurai { Name = "Gorobei Katayama" }
            );

            _context.Battles.AddRange(
                new Battle { Name = "Battle of Okehazama", StartDate = new DateTime(1560, 05, 1), EndDate = new DateTime(1560, 6, 15) },    
                new Battle { Name = "Battle of Shiroyama", StartDate = new DateTime(1877, 9, 24), EndDate = new DateTime(1877, 9, 24) },    
                new Battle { Name = "Siege of Osaka", StartDate = new DateTime(1614, 1, 1), EndDate = new DateTime(1615, 12, 31) },    
                new Battle { Name = "Boshin War ", StartDate = new DateTime(1868, 1, 1), EndDate = new DateTime(1869, 1, 1) }
            );
            _context.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Qoutes).FirstOrDefault();
            var qoute = samurai.Qoutes[0];
            qoute.Text += " Did you hear that?";
            using(var newContext = new SamuraiContext())
            {
                //newContext.Qoutes.Update(qoute);
                newContext.Entry(qoute).State = EntityState.Modified; //only update the quote and not whole samurai (whole graph)
                newContext.SaveChanges();
            }
        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Qoutes).FirstOrDefault();
            samurai.Qoutes[0].Text += " Did hear that?";
            //_context.Qoutes.Remove(samurai.Qoutes[3]);
            _context.SaveChanges();
        }

        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais
                .Where(s => s.Qoutes.Any(q => q.Text.Contains("Happy")))
                .ToList();
        }

        private static void ProjectsWithQoutes()
        {
            //var somePropertiesWithQoutes = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, s.Qoutes.Count })
            //    .ToList();

            //var somePropertiesWithSomeQoutes = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, HappyQoutes = s.Qoutes.Where(q => q.Text.Contains("Happy")) })
            //    .ToList();

            ////EF Core projections don't connect graphs (Issue) - (Samurai -> qoutes -> count )
            //var samuraisWithHappyQuotes = _context.Samurais
            //    .Select(s => new { Samurai = s, Quotes = s.Qoutes.Where(q => q.Text.Contains("Happy")).ToList() }).ToList();

            var samurais = _context.Samurais.ToList();
            var happyQoutes = _context.Qoutes.Where(q => q.Text.Contains("Happy")).ToList();
                                
        }

        private static object ProjectDynamic()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            return someProperties.ToList<dynamic>();
        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new {  s.Id, s.Name }).ToList();
            var idsAndNames = _context.Samurais.Select(s => new IdAndNames(s.Id, s.Name)).ToList();
        }

        internal class IdAndNames
        {
            private int id;
            private string name;

            public IdAndNames(int id, string name)
            {
                this.id = id;
                this.name = name;
            }
        }

        private static void EargerLoadSamuraiWithQuotes()
        {
            //List<Samurai> samuraiWithQoutes = _context.Samurais.Include(s => s.Qoutes).ToList();
            Samurai samuraiWithQoutes = _context.Samurais.Where(s => s.Name.Contains("Julie"))
                                                            .Include(s => s.Qoutes)
                                                            .Include(s => s.SecretIdentity)
                                                            .FirstOrDefault();
        }

        private static void AddChildToExistingObjectWhileNotTracked(int samuraiId)
        {
            Qoute qoute = new Qoute
            {
                Text = "Now that i saved you, will you feed me dinner",
                SamuraiId = samuraiId
            };
            using (SamuraiContext newContext = new SamuraiContext())
            {
                newContext.Qoutes.Add(qoute);
                newContext.SaveChanges();
            }
        }

        private static void AddChildToExistingObjectWhileTracked()
        {
            Samurai samurai = _context.Samurais.First();
            samurai.Qoutes.Add(new Qoute
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }

        private static void InsertNewPkFkGraphMultiplyChildren()
        {
            Samurai samurai = new Samurai
            {
                Name = "Kyuzo",
                Qoutes = new List<Qoute>
                {
                    new Qoute { Text = "Watch our for my sharp sword!"},
                    new Qoute { Text = "I told you to watch out for my sharp sword! Oh well!" }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewPkFkGraph()
        {
            Samurai samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Qoutes = new List<Qoute>
                {
                    new Qoute
                    {
                        Text = "I've come to save you"
                    }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void DeleteUsingId(int samuraiId)
        {
            Samurai samurai = _context.Samurais.Find(samuraiId);
            _context.Remove(samurai);
            _context.SaveChanges();
            //Alternate to not get object first and then delete: stored procedure !
            // _context.Database.ExecuteSqlCommand("exec DeleteById {0}", samuraiId);
        }

        private static void AddMoreSamurais()
        {
            _context.AddRange(
                new Samurai { Name = "Kambei Shimada" },
                new Samurai { Name = "Shichiroji" },
                new Samurai { Name = "Katsushiro Okamoto" },
                new Samurai { Name = "Heihachi Hayashida" },
                new Samurai { Name = "kyuzo" },
                new Samurai { Name = "Gorobei Katayama" }
                );
            _context.SaveChanges();
        }

        private static void DeleteWhileTracked()
        {
            Samurai samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Kambei Shimada");
            _context.Samurais.Remove(samurai);
            //Alternates:
            // _context.Remove(samurai);
            // _context.Samurais.Remove(_context.Samurais.Find(1));
            _context.SaveChanges();
        }

        private static void DeleteWhileNotTracked()
        {
            Samurai samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Kambei Shimada");
            using (SamuraiContext contextNewAppInstance = new SamuraiContext())
            {
                contextNewAppInstance.Samurais.Remove(samurai);
                //contextNewAppInstance.Entry(samurai).State=EntityState.Deleted;
                contextNewAppInstance.SaveChanges();
            }
        }

        private static void InsertBattle()
        {
            _context.Battles.Add(new Battle
            {
                Name = "Batlle of Okehazama",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15)
            });

            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);
            using (SamuraiContext newContextInstance = new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void MultiplyDatabaseOperations()
        {
            Samurai samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "Hiro";
            _context.Samurais.Add(new Samurai { Name = "Kikuchiyo" });
            _context.SaveChanges();
        }

        private static void RetrieveAndUpdateMultiplySamurai()
        {
            List<Samurai> samurais = _context.Samurais.ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();

        }

        private static void RetrieveAndUpdateSamurai()
        {
            Samurai samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
        }

        private static void MoreQueries()
        {
            //string name = "Sampson";
            //List<Samurai> samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            //Samurai samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);
            //Samurai samuraiById = _context.Samurais.Find(2); //DbSet method, Benefit if Object already in memory and being tracked, EF dont waste time on query the db
            //List<Samurai> samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "J%")).ToList();

        }

        private static void SimpleSamuraiQuery()
        {
            using (SamuraiContext context = new SamuraiContext())
            {
                List<Samurai> samurais = context.Samurais.ToList();

                foreach (Samurai samurai in samurais)
                {
                    Console.WriteLine(samurai.Name);
                }

            }
        }

        private static void InsertMultplyDiffrentObjects()
        {
            Samurai samurai = new Samurai { Name = "Oda Nobunga" };
            Battle battle = new Battle
            {
                Name = "Battle of Nagashino",
                StartDate = new DateTime(1575, 06, 16),
                EndDate = new DateTime(1575, 06, 28)
            };

            using (SamuraiContext context = new SamuraiContext())
            {
                context.AddRange(samurai, battle);
                context.SaveChanges();
            }
        }

        private static void InsertMultplySamurais()
        {
            Samurai samurai = new Samurai { Name = "Julie" };
            Samurai samuraiSammy = new Samurai { Name = "Sampson" };

            using (SamuraiContext context = new SamuraiContext())
            {
                context.Samurais.AddRange(samurai, samuraiSammy);
                context.SaveChanges();
            }
        }

        private static void InsertSamurai()
        {
            Samurai samurai = new Samurai { Name = "Jonas" };
            using (SamuraiContext context = new SamuraiContext())
            {
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
        }
    }


}
