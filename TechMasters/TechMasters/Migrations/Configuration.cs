namespace TrabalhoPartico.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using TrabalhoPartico.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TrabalhoPartico.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TrabalhoPartico.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            try
            {
                context.Ids.AddOrUpdate(new ids("Venda"));
                context.Ids.AddOrUpdate(new ids("Produto"));
                context.Ids.AddOrUpdate(new ids("ProdQuant"));
                context.Ids.AddOrUpdate(new ids("Encomenda"));
                context.Ids.AddOrUpdate(new ids("Categoria"));
                context.SaveChanges();
            }
            catch (Exception) { }

            try
            {
                context.Roles.AddOrUpdate(new IdentityRole("Cliente"));
                context.SaveChanges();
            }
            catch (Exception) { }

            try
            {
                context.Roles.AddOrUpdate(new IdentityRole("GestorProd"));
                context.SaveChanges();
            }
            catch (Exception) { }

            try
            {
                context.Roles.AddOrUpdate(new IdentityRole("Admin"));
                context.SaveChanges();
            }
            catch (Exception) { }

            /*  Add Admin and Gestor Prod no register... Primeira conta é admin, Segunda conta é Gestor de Produtos
            ApplicationUser user = new ApplicationUser { UserName = "admin@techmaster.com", Email = "techmasteradmin@techmaster.com" };
            context.Users.Add(user);
            
            user = new ApplicationUser { UserName = "gestor@techmaster.com", Email = "techmastergestor@techmaster.com" };
            context.Users.Add(user);
            */
        }
    }
}
