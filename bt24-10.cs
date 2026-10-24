// Student.cs
namespace EntityFrameworkCodeFirst
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; } = null!;
    }
}

// Class.cs
namespace EntityFrameworkCodeFirst
{
    public class Class
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public List<Student> Students { get; set; } = new();
    }
}

// AppDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCodeFirst
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassId);
        }
    }
}

// Program.cs
using EntityFrameworkCodeFirst;

using var db = new AppDbContext();

db.Database.EnsureCreated();

var newClass = new Class
{
    ClassName = "CNTT"
};

db.Classes.Add(newClass);
db.SaveChanges();

db.Students.Add(new Student
{
    FullName = "Nguyen Van A",
    Age = 20,
    ClassId = newClass.ClassId
});

db.SaveChanges();

Console.WriteLine("Them du lieu thanh cong!");
