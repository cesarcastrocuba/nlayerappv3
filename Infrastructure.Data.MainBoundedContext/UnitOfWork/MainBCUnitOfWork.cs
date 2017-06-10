namespace NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork
{
    using Microsoft.EntityFrameworkCore;
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using NLayerApp.Domain.Seedwork;
    using NLayerApp.Infrastructure.Data.Seedwork;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    public class MainBCUnitOfWork
        :DbContext,IQueryableUnitOfWork
    {
        #region Private Methods
        private void Audit()
        {
            // Get the authenticated user name 
            string userName = "Anonymous";

            var user = ClaimsPrincipal.Current;
            if (user != null)
            {
                var identity = user.Identity;
                if (identity != null)
                {
                    userName = identity.Name;
                }
            }

            foreach (var auditedEntity in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (auditedEntity.State == EntityState.Added ||
                    auditedEntity.State == EntityState.Modified)
                {

                    auditedEntity.Entity.LastModifiedAt = DateTime.UtcNow;
                    auditedEntity.Entity.LastModifiedBy = userName;

                    if (auditedEntity.State == EntityState.Added)
                    {
                        auditedEntity.Entity.CreatedAt = DateTime.UtcNow;
                        auditedEntity.Entity.CreatedBy = userName;
                    }
                    else
                    {
                        auditedEntity.Property(p => p.CreatedAt).IsModified = false;
                        auditedEntity.Property(p => p.CreatedBy).IsModified = false;
                    }
                }
            }


        }

        private int SaveAndAuditChanges()
        {
            this.Audit();
            return base.SaveChanges();
        }

        private async Task<int> SaveAndAuditChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.Audit();
            return await base.SaveChangesAsync();
        }


        #endregion        

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

        #region IQueryableUnitOfWork Members

        public virtual DbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class
        {
            return base.Set<TEntity>();
        }
        public void Attach<TEntity>(TEntity item)
            where TEntity : class
        {
            //attach and set as unchanged

            base.Entry<TEntity>(item).State = EntityState.Unchanged;
        }
        public void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            //this operation also attach item in object state manager
            base.Entry<TEntity>(item).State = EntityState.Modified;
        }
        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            //if it is not attached, attach original and set current values
            base.Entry<TEntity>(original).CurrentValues.SetValues(current);
        }
        public virtual void Commit()
        {
            this.SaveAndAuditChanges();
        }
        public virtual async Task<int> CommitAsync()
        {
            return await this.SaveAndAuditChangesAsync();
        }
        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    this.Commit();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });

                }
            } while (saveFailed);

        }
        public async Task<int> CommitAndRefreshChangesAsync()
        {
            bool saveFailed = false;
            int i = 0;
            do
            {
                try
                {
                    i = await this.CommitAsync();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });

                }
            } while (saveFailed);

            return i;

        }
        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = EntityState.Unchanged);
        }
        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            //Not implemented yet
            throw new NotImplementedException();
        }
        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            //Not implemented yet
            throw new NotImplementedException();
        }
        public void Refresh(object entity)
        {
            base.Entry(entity).Reload();
        }

        #endregion


    }
}
