using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.UserApplection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Bokking_System.Data
{
    public class Hotel_dbcontext : IdentityDbContext<ApplicationUser>
    {
        public Hotel_dbcontext(DbContextOptions<Hotel_dbcontext> options)
            : base(options)
        {
        }

        public DbSet<Cls_Customr> Customers { get; set; }
        public DbSet<Cls_Room> Rooms { get; set; }
        public DbSet<Cls_Booking> Bookings { get; set; }
        public DbSet<Cls_Payments> Payments { get; set; }
        public DbSet<Cls_Reviews> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // هنا تقدر تزود أي إعدادات خاصة بالعلاقات أو التحويلات
            // مثال: تخزين الـ Enums كـ string بدل int
            builder.Entity<Cls_Room>()
                .Property(r => r.Status)
                .HasConversion<string>();

            builder.Entity<Cls_Room>()
                .Property(r => r.Type)
                .HasConversion<string>();

            builder.Entity<Cls_Booking>()
                .Property(b => b.Status)
                .HasConversion<string>();

            builder.Entity<Cls_Payments>()
                .Property(p => p.Method)
                .HasConversion<string>();

            builder.Entity<Cls_Payments>()
                .Property(p => p.Status)
                .HasConversion<string>();
        }
    }
}
