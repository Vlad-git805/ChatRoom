using System.Data.Entity;

namespace _02_ChatClient
{
    public class Initializer : CreateDatabaseIfNotExists<Model1>
    {
        protected override void Seed(Model1 ctx)
        {
            base.Seed(ctx);

            //ctx.Users.Add(new User() { Name = "Vlad", Password = "sadl", Tag = "#fleks228" });
            //ctx.Users.Add(new User() { Name = "Dima", Password = "123", Tag = "#123321" });
            //ctx.Users.Add(new User() { Name = "Kol9", Password = "111", Tag = "#qweqwe" });
            //ctx.SaveChanges();
        }
    }
}