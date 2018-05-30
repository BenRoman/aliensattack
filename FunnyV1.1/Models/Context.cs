using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Web;

namespace FunnyV1.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base("FunnyV1")
        { }
        public DbSet<Alien> Aliens { get; set; }
        public DbSet<Human> Humen { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ShipType> ShipTypes { get; set; }
        public DbSet<SpaceShip> SpaceShips { get; set; }
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<ExType> ExTypes { get; set; }
        public DbSet<ExOn> ExOns { get; set; }
        public DbSet<Escape> Escapes { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Kidnapping> Kidnappings { get; set; }
        public DbSet<KidnappingWhom> KidnappingWhoms { get; set; }
        public DbSet<Excursion> Excursions { get; set; }
        public DbSet<ExcursionFor> ExcursionFors { get; set; }
        public DbSet<Murder> Murders { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();



        //    modelBuilder.Entity<Alien>()
        //     .HasRequired(f => f.Role)
        //     .WithRequiredDependent()
        //     .WillCascadeOnDelete(true);

        //    modelBuilder.Entity<Escape>()
        //       .HasRequired(f => f.Human)
        //       .WithRequiredDependent()
        //       .WillCascadeOnDelete(true);

        //    modelBuilder.Entity<Escape>().HasRequired(f => f.Ship).WithRequiredDependent().WillCascadeOnDelete(true);


        //    modelBuilder.Entity<Excursion>().HasRequired(f => f.Alien).WithRequiredDependent().WillCascadeOnDelete(true);
        //    modelBuilder.Entity<Excursion>().HasRequired(f => f.SpaceShip).WithRequiredDependent().WillCascadeOnDelete(true);

        //    modelBuilder.Entity<ExcursionFor>().HasRequired(f => f.Human).WithRequiredDependent().WillCascadeOnDelete(true);
        //    modelBuilder.Entity<ExcursionFor>().HasRequired(f => f.Excursion).WithRequiredDependent().WillCascadeOnDelete(true);

        //    modelBuilder.Entity<Experiment>().HasRequired(f => f.ExType).WithRequiredDependent().WillCascadeOnDelete(true);
        //    modelBuilder.Entity<Experiment>().HasRequired(f => f.Ship).WithRequiredDependent().WillCascadeOnDelete(true);

        //    modelBuilder.Entity<ExOn>().HasRequired(f => f.Human).WithRequiredDependent().WillCascadeOnDelete(true);
        //    modelBuilder.Entity<ExOn>().HasRequired(f => f.Experiment).WithRequiredDependent().WillCascadeOnDelete(true);

        //    modelBuilder.Entity<Human>().HasRequired(f => f.Role).WithRequiredDependent().WillCascadeOnDelete(true);
        //    modelBuilder.Entity<Human>().HasRequired(f => f.SpaceShip).WithRequiredDependent().WillCascadeOnDelete(true);


        //    modelBuilder.Entity<KidnappingWhom>().HasRequired(f => f.Human).WithRequiredDependent().WillCascadeOnDelete(true);
        //    modelBuilder.Entity<KidnappingWhom>().HasRequired(f => f.Kidnapping).WithRequiredDependent().WillCascadeOnDelete(true);

        //    modelBuilder.Entity<Kidnapping>().HasRequired(f => f.Ship).WithRequiredDependent().WillCascadeOnDelete(true);
        //    modelBuilder.Entity<Kidnapping>().HasRequired(f => f.Alien).WithRequiredDependent().WillCascadeOnDelete(true);


        //    modelBuilder.Entity<Murder>().HasRequired(f => f.Alien).WithRequiredDependent().WillCascadeOnDelete(true);
        //    modelBuilder.Entity<Murder>().HasRequired(f => f.Human).WithRequiredDependent().WillCascadeOnDelete(true);

        //    modelBuilder.Entity<SpaceShip>().HasRequired(f => f.Ship).WithRequiredDependent().WillCascadeOnDelete(true);


        //    modelBuilder.Entity<Transfer>().HasRequired(f => f.Human).WithRequiredDependent().WillCascadeOnDelete(true);
        //    modelBuilder.Entity<Transfer>().HasRequired(f => f.SpaceShipFrom).WithRequiredDependent().WillCascadeOnDelete(true);
        //    modelBuilder.Entity<Transfer>().HasRequired(f => f.SpaceShipTo).WithRequiredDependent().WillCascadeOnDelete(true);






        //}
    }


    public class UserDbInitializer : DropCreateDatabaseAlways<UserContext>
    {
        protected override void Seed(UserContext db)
        {
            Role alien = new Role { Name = "alien" };
            Role human = new Role { Name = "human" };
            db.Roles.Add(alien);
            db.Roles.Add(human);
            //var connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\FunnyV1.mdf';Integrated Security=True";
            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO ShipTypes(Type, Description) VALUES ('Military ship', 'Warships are intended for the purchase of alien planets'), ('Farm-ship' , 'Place of breeding persons of an enthusiastic race (in our case - people)') , ('Prayer ship', 'Grasshoppers plan not only their laws but also the faith')", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            base.Seed(db);
        }
    }
}