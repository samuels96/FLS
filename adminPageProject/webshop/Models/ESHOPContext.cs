using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace webshop.Models
{
    public partial class ESHOPContext : DbContext
    {
        public ESHOPContext()
        {
        }

        public ESHOPContext(DbContextOptions<ESHOPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<BasketProduct> BasketProduct { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<CatalogueProductView> CatalogueProductView { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Efmigrationshistory> Efmigrationshistory { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderAndOrderBasketView> OrderAndOrderBasketView { get; set; }
        public virtual DbSet<OrderAudit> OrderAudit { get; set; }
        public virtual DbSet<OrderBasket> OrderBasket { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductBasketJoint> ProductBasketJoint { get; set; }
        public virtual DbSet<ProductOutOfStockNotification> ProductOutOfStockNotification { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=fls-school-database.c1t2au2qimus.us-east-1.rds.amazonaws.com;port=3306;user=admin;password=datadata;database=ESHOP");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ESHOP");
            modelBuilder.Entity<Admins>(entity =>
            {
                entity.ToTable("admins");

                entity.HasIndex(e => e.Email)
                    .HasName("idx_email");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(45);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(32);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(16);
            });

            modelBuilder.Entity<BasketProduct>(entity =>
            {
                entity.ToTable("basket_product");

                entity.HasIndex(e => e.BasketId)
                    .HasName("fk_basket_product_order_basket1_idx");

                entity.HasIndex(e => e.ProductId)
                    .HasName("fk_basket_product_product1_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BasketId).HasColumnName("BasketID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Basket)
                    .WithMany(p => p.BasketProduct)
                    .HasForeignKey(d => d.BasketId)
                    .HasConstraintName("fk_basket_product_order_basket1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.BasketProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_basket_product_product1");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brand");

                entity.HasIndex(e => e.Name)
                    .HasName("brand_name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'No description'");

                entity.Property(e => e.LogoUrl)
                    .HasColumnName("LogoURL")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<CatalogueProductView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("catalogueProductView");

                entity.Property(e => e.BrandDescription)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'No description'");

                entity.Property(e => e.BrandLogo).HasMaxLength(255);

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'No description'");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("imageUrl")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            modelBuilder.Entity<Efmigrationshistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId)
                    .HasName("PRIMARY");

                entity.ToTable("__efmigrationshistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("FKOrder910145");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DeliveryAddress)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ExpectedDeliveryDate).HasColumnType("date");

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKOrder910145");
            });

            modelBuilder.Entity<OrderAndOrderBasketView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("orderAndOrderBasketView");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DeliveryAddress)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ExpectedDeliveryDate).HasColumnType("date");

                entity.Property(e => e.OrderBasketId).HasColumnName("OrderBasketID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductBasketJointId).HasColumnName("ProductBasketJointID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Status).HasMaxLength(100);
            });

            modelBuilder.Entity<OrderAudit>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("order_audit");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.NewStatus).HasMaxLength(100);

                entity.Property(e => e.OldStatus).HasMaxLength(100);

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.StatusChanged).HasColumnType("date");
            });

            modelBuilder.Entity<OrderBasket>(entity =>
            {
                entity.ToTable("order_basket");

                entity.HasIndex(e => e.OrderId)
                    .HasName("FKorder_bask998727");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderBasket)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FKorder_bask998727");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasIndex(e => e.BrandId)
                    .HasName("FKProduct960541");

                entity.HasIndex(e => e.Category)
                    .HasName("FKProduct493591_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'No description'");

                entity.Property(e => e.ImageUrl).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FKProduct960541");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.Category)
                    .HasConstraintName("FKProduct493591");
            });

            modelBuilder.Entity<ProductBasketJoint>(entity =>
            {
                entity.ToTable("product_basket_joint");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BasketId).HasColumnName("BasketID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
            });

            modelBuilder.Entity<ProductOutOfStockNotification>(entity =>
            {
                entity.HasKey(e => e.ProductStockId)
                    .HasName("PRIMARY");

                entity.ToTable("product_outOfStock_notification");

                entity.Property(e => e.ProductStockId).HasColumnName("product_stockId");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.ProductId).HasColumnName("productId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
