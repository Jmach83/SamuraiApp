using System.Collections.Generic;

namespace SamuraiApp.Domain
{
    public class Samurai
    {
        public Samurai()
        {
            SecretIdentity = new SecretIdentity();
            Qoutes = new List<Qoute>();
            SamuraiBattles = new List<SamuraiBattle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Qoute> Qoutes { get; set; }
        public int BattleId { get; set; }
        public List<SamuraiBattle> SamuraiBattles { get; set; }
        public SecretIdentity SecretIdentity { get; set; }
        public List<Battle> Battles()
        {
            var battles = new List<Battle>();
            foreach (var join in SamuraiBattles)
            {
                battles.Add(join.Battle);
            }
            return battles;
        }
    }
}
