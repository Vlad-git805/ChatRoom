namespace _02_ChatClient.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<_02_ChatClient.Model1>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "_02_ChatClient.Model1";
        }

        protected override void Seed(_02_ChatClient.Model1 context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
