using EComersObjectLib;
using EComersObjectLib.Objects;
using EComersObjectLib.SapObjects;
using Microsoft.EntityFrameworkCore;

namespace EComersDBworkerLib
{
    public class EComersContext:DbContext
    {
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserStatus> UsersStatus { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<DepartmentStatus> DepartmentStatus { get; set; }
        public DbSet<Description> Description { get; set; }
        public DbSet<Employe> Employe { get; set; }
        public DbSet<EmployeHistory> EmployeHistory { get; set;}
        public DbSet<EmployeStatus> EmployeStatus { get; set;}
        public DbSet<FunctionsType> FunctionsTypes { get; set; }
        public DbSet<FuncTypeStatus> FuncTypeStatus { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<RatingHistory> RatingHistory { get; set; }
        public DbSet<RatingStatus> RatingStatus { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ClientBalance> ClientBalance { get; set; }
        public DbSet<ClientBalanceHistory> ClientBalanceHistory { get; set; }
        public DbSet<BalanceStatus> BalanceStatus { get; set; }
        public DbSet<ClientBalanceType> ClientBalanceType { get; set; }
        public DbSet<PayDocument> PayDocument { get; set; }
        public DbSet<IncomeDocument> IncomeDocument { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<GeneralStatus> GeneralStatus { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; } 
        public DbSet<OITM> OITM { get; set; }
        public DbSet<DeliveryForInvoice> DeliveryForInvoice { get; set; }
        public DbSet<ODLN> ODLN { get; set; }
        public DbSet<DeliveryItem> DeliveryItem { get; set; }

        public EComersContext()
        {
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(new EComersDBConfig().GetEComersConnectonString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("ecomers");

            modelBuilder.Entity<Employe>().HasKey(p => p.EmployeId);
            modelBuilder.Entity<Employe>().HasMany(p => p.EmployeHistory).WithOne(p => p.Employe).HasForeignKey(p => p.EmployeId);
            modelBuilder.Entity<Employe>().HasOne(p => p.CreatedUser).WithMany(p => p.CreatedEmployesUser).HasForeignKey(p => p.CreatedUserId);
            modelBuilder.Entity<Employe>().HasOne(p => p.UpdatedUser).WithMany(p => p.UpdatedEmployesUser).HasForeignKey(p => p.UpdatedUserId);

            modelBuilder.Entity<EmployeHistory>().HasKey(p => p.EmployeHistoryId);
            modelBuilder.Entity<EmployeHistory>().HasOne(p => p.EmployeStatus).WithMany(p=>p.EmployeHistory).HasForeignKey(p => p.StatusId);
            modelBuilder.Entity<EmployeHistory>().HasOne(p => p.FunctionsType).WithMany(p => p.EmployeHistory).HasForeignKey(p => p.FuncTypeId);
            modelBuilder.Entity<EmployeHistory>().Property(p => p.BirthDay).HasColumnType("date");
            modelBuilder.Entity<EmployeStatus>().HasKey(p => p.StatusId);

            modelBuilder.Entity<Role>().HasKey(p => p.RoleId);
            modelBuilder.Entity<Role>().HasMany(p => p.Users).WithOne(p => p.Role).HasForeignKey(p => p.RoleId);
            modelBuilder.Entity<Role>().HasOne(p => p.CreatedUser).WithMany(p => p.CreatedRoles).HasForeignKey(p => p.CreatedUserId);
            modelBuilder.Entity<Role>().HasOne(p => p.UpdatedUser).WithMany(p => p.UpdatedRoles).HasForeignKey(p => p.UpdatedUserId);

            modelBuilder.Entity<User>().HasKey(p => p.UserId);

            modelBuilder.Entity<UserStatus>().HasKey(p => p.StatusId);
            modelBuilder.Entity<UserStatus>().HasMany(p => p.Users).WithOne(p => p.UserStatus).HasForeignKey(p => p.StatusId);

            modelBuilder.Entity<Rating>().HasKey(p => p.RatingId);
            modelBuilder.Entity<Rating>().HasOne(p => p.Employe).WithMany(p => p.Ratings).HasForeignKey(p => p.EmployeId);
            modelBuilder.Entity<Rating>().HasMany(p => p.RatingHistories).WithOne(p => p.Rating).HasForeignKey(p => p.RatingHistoryId);

            modelBuilder.Entity<RatingHistory>().HasKey(p => p.RatingHistoryId);

            modelBuilder.Entity<RatingStatus>().HasKey(p => p.StatusId);
            modelBuilder.Entity<RatingStatus>().HasMany(p => p.RatingHistory).WithOne(p => p.Status).HasForeignKey(p => p.RatingStatusId);

            modelBuilder.Entity<Department>().HasKey(p => p.DepartmentId);

            modelBuilder.Entity<DepartmentStatus>().HasKey(p => p.StatusId);
            modelBuilder.Entity<DepartmentStatus>().HasMany(p => p.Departments).WithOne(p => p.DepartmentStatus).HasForeignKey(p => p.StatusId);

            modelBuilder.Entity<Description>().HasKey(p => p.DescId);
            modelBuilder.Entity<Description>().HasAlternateKey(p => new { p.DescriptionId, p.LangCod }).HasName("DescriptionAltKey");

            modelBuilder.Entity<FunctionsType>().HasKey(p => p.FuncTypeId);
            modelBuilder.Entity<FunctionsType>().HasOne(p => p.CreatedUser).WithMany(p => p.CreatedFunctionsType).HasForeignKey(p => p.UserId);
            modelBuilder.Entity<FunctionsType>().HasOne(p => p.UpdatedUser).WithMany(p => p.UpdatedFunctionsType).HasForeignKey(p => p.UpdatedUserId);

            modelBuilder.Entity<FuncTypeStatus>().HasKey(p => p.StatusId);
            modelBuilder.Entity<FuncTypeStatus>().HasMany(p=>p.FunctionsTypes).WithOne(p => p.StatusType).HasForeignKey(p => p.StatusId);

            modelBuilder.Entity<Gender>().HasKey(p => p.GenderId);
            modelBuilder.Entity<Gender>().HasMany(p => p.Clients).WithOne(p => p.Gender).HasForeignKey(p => p.ClientGenderId);
            modelBuilder.Entity<Gender>().HasMany(p => p.EmployeHistoryes).WithOne(p => p.Gender).HasForeignKey(p => p.GenderId);

            modelBuilder.Entity<Client>().HasKey(p => p.ClientId);
            modelBuilder.Entity<Client>().HasMany(p => p.ClientBalances).WithOne(p => p.Client).HasForeignKey(p => p.ClientId);

            modelBuilder.Entity<ClientBalance>().HasKey(p => p.ClientBalanceId);
            modelBuilder.Entity<ClientBalance>().HasOne(p => p.CreatedUser).WithMany(p => p.CreatedClntBalUser).HasForeignKey(p => p.CreatedUserId);
            modelBuilder.Entity<ClientBalance>().HasOne(p => p.UpdatedUser).WithMany(p => p.UpdatedClntBalUser).HasForeignKey(p => p.UpdatedUserId);
            modelBuilder.Entity<ClientBalance>().HasMany(p => p.ClientBalanceHistory).WithOne(p => p.ClientBalance).HasForeignKey(p => p.ClientBalanceId);

            modelBuilder.Entity<ClientBalanceHistory>().HasKey(p => p.ClntBalHistoryId);

            modelBuilder.Entity<BalanceStatus>().HasKey(p => p.StatusId);
            modelBuilder.Entity<BalanceStatus>().HasMany(p => p.ClientBalanceHistory).WithOne(p => p.BalanceStatus).HasForeignKey(p => p.ClntBalStatusId);

            modelBuilder.Entity<ClientBalanceType>().HasKey(p => p.BalanceTypeId);
            modelBuilder.Entity<ClientBalanceType>().HasMany(p => p.ClientBalances).WithOne(p => p.ClientBalanceType).HasForeignKey(p => p.ClientBalanceTypeId);

            modelBuilder.Entity<Region>().HasKey(p => p.RegionId);
            modelBuilder.Entity<Region>().HasOne(p => p.CreatedUser).WithMany(p => p.CreatedRegions).HasForeignKey(p => p.CreatedUserId);
            modelBuilder.Entity<Region>().HasOne(p => p.UpdatedUser).WithMany(p => p.UpdatedRegions).HasForeignKey(p => p.UpdatedUserId);

            modelBuilder.Entity<PayDocument>().HasKey(p => p.PayDocId);
            modelBuilder.Entity<PayDocument>().HasOne(p => p.ClientBalanceHistory).WithOne(p => p.PayDocument).HasForeignKey<ClientBalanceHistory>(p => p.PayDocId);

            modelBuilder.Entity<IncomeDocument>().HasKey(p => p.IncomeDocId);
            modelBuilder.Entity<IncomeDocument>().HasOne(p => p.ClientBalanceHistory).WithOne(p => p.IncomeDocument).HasForeignKey<ClientBalanceHistory>(p => p.IncomeDocId);
            
            modelBuilder.Entity<GeneralStatus>().HasKey(p => p.StatusId);
            modelBuilder.Entity<GeneralStatus>().HasMany(p => p.Region).WithOne(p => p.GeneralStatus).HasForeignKey(p => p.StatusId);
            modelBuilder.Entity<GeneralStatus>().HasMany(p => p.Client).WithOne(p => p.GeneralStatus).HasForeignKey(p => p.ClientStatusId);
            modelBuilder.Entity<GeneralStatus>().HasMany(p => p.ClientBalanceType).WithOne(p => p.GeneralStatus).HasForeignKey(p => p.StatusId);

            modelBuilder.Entity<Parameter>().HasKey(p => p.ParamId);

            modelBuilder.Entity<Customer>().HasKey(p => p.CustomerId);

            modelBuilder.Entity<Order>().HasKey(p => p.OrderId);
            modelBuilder.Entity<Order>().HasMany(p => p.OrderItems).WithOne(p => p.OrderForServer).HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<OITM>().HasKey(p => p.ItemId);

            modelBuilder.Entity<DeliveryForInvoice>().HasKey(p => p.DeliveryForInvoiceId);
            modelBuilder.Entity<DeliveryForInvoice>().HasOne(p=>p.ODLN).WithOne(p=>p.DeliveryForInvoice).HasForeignKey<ODLN>(p => p.DeliveryForInvoiceId);
            modelBuilder.Entity<DeliveryForInvoice>().HasMany(p => p.DeliveryItems).WithOne(p => p.DeliveryForInvoice).HasForeignKey(p => p.DeliveryForInvoiceId);

            modelBuilder.Entity<ODLN>().HasKey(p => p.OdlnId);

            modelBuilder.Entity<DeliveryItem>().HasKey(p => p.DeliveryItemId);

        }
    }
}