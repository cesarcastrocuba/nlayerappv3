using Microsoft.EntityFrameworkCore;
using NLayerApp.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NLayerApp.Infrastructure.Data.Seedwork.UnitOfWork
{
    public class BaseContext : DbContext, IQueryableUnitOfWork
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

        #region Constructors
        public BaseContext()
        {
        }
        public BaseContext(DbContextOptions options) 
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
        public virtual void Attach<TEntity>(TEntity item)
            where TEntity : class
        {
            //attach and set as unchanged

            base.Entry<TEntity>(item).State = EntityState.Unchanged;
        }
        public virtual void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            //this operation also attach item in object state manager
            base.Entry<TEntity>(item).State = EntityState.Modified;
        }
        public virtual void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
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
        public virtual void CommitAndRefreshChanges()
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
        public virtual async Task<int> CommitAndRefreshChangesAsync()
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
        public virtual void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = EntityState.Unchanged);
        }
        public virtual IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            //Not implemented yet
            throw new NotImplementedException();
        }
        public virtual int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            //Not implemented yet
            throw new NotImplementedException();
        }
        public virtual void Refresh(object entity)
        {
            base.Entry(entity).Reload();
        }

        #endregion

    }
}
