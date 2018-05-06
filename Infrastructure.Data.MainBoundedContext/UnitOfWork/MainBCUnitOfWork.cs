namespace NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork
{
    using Microsoft.EntityFrameworkCore;
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using NLayerApp.Infrastructure.Data.Seedwork.UnitOfWork;

    public class MainBCUnitOfWork : BaseContext
    {

        #region DBContext Override Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "NLayerApp");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>()
                .OwnsOne(c => c.BankAccountNumber);

            modelBuilder.Entity<Customer>()
                .OwnsOne(c => c.Address);

            modelBuilder.Entity<Order>()
                .OwnsOne(c => c.ShippingInformation);

            modelBuilder.Entity<Product>();
            modelBuilder.Entity<Software>();
            modelBuilder.Entity<Book>();

        }
        #endregion

        #region DbSet Members

        DbSet<Customer> _customers;
        public virtual DbSet<Customer> Customers
        {
            get 
            {
                if (_customers == null)
                    _customers = base.Set<Customer>();

                return _customers;
            }
        }

        DbSet<Product> _products;
        public virtual DbSet<Product> Products
        {
            get 
            {
                if (_products == null)
                    _products = base.Set<Product>();

                return _products;
            }
        }

        DbSet<Software> _software;
        public virtual DbSet<Software> Software
        {
            get
            {
                if (_software == null)
                    _software = base.Set<Software>();

                return _software;
            }
        }

        DbSet<Book> _books;
        public virtual DbSet<Book> Books
        {
            get
            {
                if (_books == null)
                    _books = base.Set<Book>();

                return _books;
            }
        }

        DbSet<Order> _orders;
        public virtual DbSet<Order> Orders
        {
            get 
            {
                if (_orders == null)
                    _orders = base.Set<Order>();

                return _orders;
            }
        }

        DbSet<Country> _countries;
        public virtual DbSet<Country> Countries
        {
            get
            {
                if (_countries == null)
                    _countries = base.Set<Country>();

                return _countries;
            }
        }


        DbSet<BankAccount> _bankAccounts;
        public virtual DbSet<BankAccount> BankAccounts
        {
            get 
            {
                if (_bankAccounts == null)
                    _bankAccounts = base.Set<BankAccount>();

                return _bankAccounts;
            }
        }

        #endregion

        #region Constructors
        public MainBCUnitOfWork()
        {
        }
        public MainBCUnitOfWork(DbContextOptions<MainBCUnitOfWork> options)
            : base(options)
        {
        }

        #endregion

    }
}
