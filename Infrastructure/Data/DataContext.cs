using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //relations
        modelBuilder.Entity<Author>()
        .HasMany(a => a.Books)
        .WithOne(b => b.Author)
        .HasForeignKey(b => b.AuthorId);

        modelBuilder.Entity<Book>()
        .HasMany(b => b.BorrowRecords)
        .WithOne(br => br.Book)
        .HasForeignKey(br => br.BookId);

        modelBuilder.Entity<Member>()
        .HasMany(m => m.BorrowRecords)
        .WithOne(br => br.Member)
        .HasForeignKey(br => br.MemberId);



        //anotations

        //author
        modelBuilder.Entity<Author>()
        .Property(a => a.Id)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<Author>()
        .Property(a => a.Name)
        .HasMaxLength(150)
        .IsRequired();

        //book
        modelBuilder.Entity<Book>()
        .Property(b => b.Id)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<Book>()
        .Property(b => b.Title)
        .HasMaxLength(200)
        .IsRequired();

        modelBuilder.Entity<Book>()
        .Property(b => b.Genre)
        .HasMaxLength(100);

        modelBuilder.Entity<Book>()
        .Property(b => b.Genre)
        .HasMaxLength(100)
        .IsRequired();

        modelBuilder.Entity<Book>()
        .Property(b => b.AuthorId)
        .IsRequired();

        //member

        modelBuilder.Entity<Member>()
        .Property(m => m.Id)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<Member>()
        .Property(m => m.Name)
        .HasMaxLength(150)
        .IsRequired();

        modelBuilder.Entity<Member>()
        .Property(m => m.Email)
        .HasMaxLength(200)
        .IsRequired();




        modelBuilder.Entity<BorrowRecord>()
        .Property(br => br.Id)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<BorrowRecord>()
        .Property(br => br.MemberId)
        .IsRequired();

        modelBuilder.Entity<BorrowRecord>()
        .Property(br => br.BookId)
        .IsRequired();

        


        
        
    }


}
