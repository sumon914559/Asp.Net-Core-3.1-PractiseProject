using DLL.Model;
using DLL.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DLL.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>,  IdentityRoleClaim<int>,  IdentityUserToken<int>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) :base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private static readonly MethodInfo _propertyMethod = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod(typeof(bool));
        private const string IsDeletedProperty = "IsDeleted";
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entity.ClrType))
                {
                    entity.AddProperty(IsDeletedProperty, typeof(bool));
                    modelBuilder.Entity(entity.ClrType).HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }
            }

            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaveChanges();

            return base.SaveChanges(acceptAllChangesOnSuccess);

        }

        private void BeforeSaveChanges()
        {
            var userEmail =  GetCurrentUserEmail();
            var entries = ChangeTracker.Entries();
            foreach (var entity in entries)
            {
                var nowTime = DateTime.Now;

                if (entity.Entity is ITrackable trackable)
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            trackable.CreatedAt = nowTime;
                            trackable.UpdatedAt = nowTime;
                            trackable.CreatedBy = userEmail;
                            trackable.UpdatedBy = userEmail;
                            break;
                        case EntityState.Modified:
                            trackable.UpdatedAt = nowTime;
                            trackable.UpdatedBy = userEmail;
                            break;
                        case EntityState.Deleted:
                            entity.Property(IsDeletedProperty).CurrentValue = true;
                            entity.State = EntityState.Modified;
                            trackable.UpdatedAt = nowTime;
                            break;

                    }
                }
            }
        }

        private string GetCurrentUserEmail()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var email = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
                return email;
            }

            return null;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            BeforeSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, name: "it");
            var prop = Expression.Call(_propertyMethod, parm, Expression.Constant(IsDeletedProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }

    }
}
