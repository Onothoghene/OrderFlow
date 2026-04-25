using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Address> Address { get; set; }
        //public DbSet<MenuItemRating> MenuItemRatings { get; set; }
        public DbSet<FileTemp> FileTemp { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Payment> Payment { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                int? authUser;

                if (_authenticatedUser.UserId != 0)
                {
                    authUser = _authenticatedUser.UserId;
                }
                else
                {
                    authUser = null;
                }

                if (entry.Entity.IsDeleted == true)
                {
                    entry.Entity.Deleted = _dateTime.NowUtc;
                    entry.Entity.DeletedBy = authUser;
                }
                else
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.Created = _dateTime.NowUtc;
                            entry.Entity.CreatedBy = authUser;
                            break;
                        case EntityState.Modified:
                            entry.Entity.LastModified = _dateTime.NowUtc;
                            entry.Entity.LastModifiedBy = authUser;
                            break;
                    }
                }

            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Universal filtering
            //builder.Entity<Stage>().HasQueryFilter(p => !p.IsDeleted);

            //builder.SeedAsync()

            //Fluent Navigations
            builder.Entity<UserProfile>(entity =>
            {
                //entity.HasIndex(e => e.AspUserId)
                //    .HasName("IX_User_AspNet")
                //    .IsUnique();

                //entity.Property(e => e.AspUserId)
                //    .IsRequired()
                //    .HasMaxLength(50);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(70);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(70);

                entity.Property(e => e.Email);

                entity.HasMany(d => d.Addresses)
                 .WithOne(p => p.CreatedByNavigation)
                 .HasForeignKey(d => d.CreatedBy)
                 .HasConstraintName("FK_UserProfile_Address");

                entity.HasMany(d => d.Orders)
                 .WithOne(p => p.CreatedByNavigation)
                 .HasForeignKey(d => d.CreatedBy)
                 .HasConstraintName("FK_UserProfile_Orders");

            });

            builder.Entity<Restaurant>(entity =>
            {
                entity.HasOne(d => d.Courier)
                    .WithOne(p => p.Restaurant)
                    .HasForeignKey<Courier>(c => c.RestaurantId)
                    .HasConstraintName("FK_Restaurant_Courier")
                    .OnDelete(DeleteBehavior.Cascade); ;

                entity.HasMany(d => d.Order)
                    .WithOne(p => p.Restaurant)
                    .HasForeignKey(c => c.RestaurantId)
                    .HasConstraintName("FK_Restaurant_Orders");
            });

            builder.Entity<Orders>(entity =>
            {
                entity.HasMany(d => d.OrderItems)
                  .WithOne(p => p.Orders)
                  .HasForeignKey(c => c.OrderId)
                  .HasConstraintName("FK_Orders_OrderItems");

                entity.HasOne(d => d.CreatedByNavigation)
                 .WithMany(p => p.Orders)
                 .HasForeignKey(d => d.CreatedBy)
                 .HasConstraintName("FK_Orders_UserProfile");

                entity.HasMany(d => d.Payments)
                  .WithOne(p => p.Order)
                  .HasForeignKey(c => c.OrderId)
                  .HasConstraintName("FK_Order_Payment");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(c => c.AddressId)
                    .HasConstraintName("FK_Orders_Address");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Order)
                    .HasForeignKey(c => c.RestaurantId)
                    .HasConstraintName("FK_Orders_Restaurant");
            });

            builder.Entity<OrderItems>(entity =>
            {
                entity.HasOne(d => d.Food)
                    .WithMany()
                    .HasForeignKey(c => c.FoodId)
                    .HasConstraintName("FK_OrderItems_Food");
            });

            builder.Entity<Comments>(entity =>
            {
                entity.HasOne(d => d.Food)
                    .WithMany(m => m.Comments)
                    .HasForeignKey(c => c.FoodId)
                    .HasConstraintName("FK_OrderItems_MenuItem");

                entity.HasOne(d => d.CreatedByNavigation)
                 .WithMany()
                 .HasForeignKey(d => d.CreatedBy)
                 .HasConstraintName("FK_Comments_UserProfile");
            });

            builder.Entity<Address>(entity =>
            {
                entity.HasOne(d => d.CreatedByNavigation)
                 .WithMany(p => p.Addresses)
                 .HasForeignKey(d => d.CreatedBy)
                 .HasConstraintName("FK_Address_UserProfile");

                entity.HasMany(d => d.Orders)
                 .WithOne(p => p.Address)
                 .HasForeignKey(d => d.AddressId)
                 .HasConstraintName("FK_Address_Orders");
            });

            builder.Entity<FileTemp>(entity =>
            {
                entity.HasOne(d => d.MenuItem)
                 .WithMany(r => r.Images)
                 .HasForeignKey(d => d.MenuItemId)
                 .HasConstraintName("FK_FileTemp_MenuItem")
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MenuItem>(entity =>
            {
                entity.HasMany(m => m.Comments)
                      .WithOne(c => c.Food)
                      .HasForeignKey(c => c.FoodId)
                      .HasConstraintName("FK_Comments_MenuItem")
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(m => m.CartItems)
                      .WithOne(c => c.Food)
                      .HasForeignKey(c => c.FoodId)
                      .HasConstraintName("FK_CartItem_MenuItem")
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.RowVersion)
                      .IsRowVersion();
            });

            builder.Entity<CartItems>(entity =>
            {
                entity.HasOne(d => d.Food)
                    .WithMany()
                    .HasForeignKey(c => c.FoodId)
                    .HasConstraintName("FK_CartItems_Food");

                entity.HasOne(d => d.CreatedByNavigation)
                 .WithMany(p => p.CartItems)
                 .HasForeignKey(d => d.CreatedBy)
                 .HasConstraintName("FK_CartItems_UserProfile");
            });

            builder.Entity<Payment>(entity =>
            {
                entity.HasOne(d => d.Order)
                 .WithMany(p => p.Payments)
                 .HasForeignKey(d => d.OrderId)
                 .HasConstraintName("FK_Payment_Order");
            });

            builder.Entity<Courier>(entity =>
            {
                entity.HasOne(d => d.Restaurant)
                     .WithOne(p => p.Courier)
                     .HasForeignKey<Restaurant>(c => c.CourierId)
                     .HasConstraintName("FK_Courier_Restaurant");
            });

            //builder.Entity<MenuItemRating>(entity =>
            //{
            //    entity.HasOne(d => d.CreatedByNavigation)
            //     .WithMany()
            //     .HasForeignKey(d => d.CreatedBy)
            //     .HasConstraintName("FK_MenuItemRating_UserProfile")
            //     .OnDelete(DeleteBehavior.Restrict);

            //    entity.HasOne(d => d.MenuItem)
            //     .WithMany(r => r.Ratings)
            //     .HasForeignKey(d => d.MenuItemId)
            //     .HasConstraintName("FK_MenuItemRating_MenuItem")
            //     .OnDelete(DeleteBehavior.Cascade);
            //});

            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }
            base.OnModelCreating(builder);
        }
    }
}
