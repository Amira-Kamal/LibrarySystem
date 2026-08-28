using LibrarySystem.Models;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new LibraryDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<LibraryDbContext>>());

        // Categories
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Fiction" },
                new Category { Name = "Non-Fiction" },
                new Category { Name = "Science" },
                new Category { Name = "Technology" },
                new Category { Name = "Business & Economics" },
                new Category { Name = "Poetry and Drama" },
                new Category { Name = "Academic/Reference" },
                new Category { Name = "Anthologies & Collections" }
            );
            context.SaveChanges();
        }

        // Authors
        if (!context.Authors.Any())
        {
            context.Authors.AddRange(
                new Author { FirstName = "J.D.", LastName = "Salinger", Nationality = "American", Bio = "American writer known for The Catcher in the Rye." },
                new Author { FirstName = "James", LastName = "Clear", Nationality = "American", Bio = "Author and speaker focused on habits and decision making." },
                new Author { FirstName = "J.K.", LastName = "Rowling", Nationality = "British", Bio = "British author best known for the Harry Potter series." },
                new Author { FirstName = "Eric", LastName = "Ries", Nationality = "American", Bio = "Entrepreneur and author of The Lean Startup." },
                new Author { FirstName = "Tara", LastName = "Westover", Nationality = "American", Bio = "American memoirist and historian." },
                new Author { FirstName = "Mark", LastName = "Manson", Nationality = "American", Bio = "Self-help author and blogger." },
                new Author { FirstName = "F. Scott", LastName = "Fitzgerald", Nationality = "American", Bio = "American novelist and short story writer." }
            );
            context.SaveChanges();
        }

        // Books
        if (!context.Books.Any())
        {
            var fiction = context.Categories.First(c => c.Name == "Fiction");
            var nonFiction = context.Categories.First(c => c.Name == "Non-Fiction");
            var business = context.Categories.First(c => c.Name == "Business & Economics");
            var poetry = context.Categories.First(c => c.Name == "Poetry and Drama");
            var academic = context.Categories.First(c => c.Name == "Academic/Reference");
            var anthologies = context.Categories.First(c => c.Name == "Anthologies & Collections");

            var salinger = context.Authors.First(a => a.LastName == "Salinger");
            var clear = context.Authors.First(a => a.LastName == "Clear");
            var rowling = context.Authors.First(a => a.LastName == "Rowling");
            var ries = context.Authors.First(a => a.LastName == "Ries");
            var westover = context.Authors.First(a => a.LastName == "Westover");
            var manson = context.Authors.First(a => a.LastName == "Manson");
            var fitzgerald = context.Authors.First(a => a.LastName == "Fitzgerald");

            context.Books.AddRange(
                new Book
                {
                    Title = "The Catcher in the Rye",
                    Description = "A story about teenage rebellion and alienation.",
                    ISBN = "978-0-316-76948-0",
                    PublishYear = 1951,
                    TotalCopies = 6,
                    AvailableCopies = 6,
                    Image = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1398034300i/5107.jpg",
                    CategoryId = fiction.CategoryId,
                    AuthorId = salinger.AuthorId
                },
                new Book
                {
                    Title = "Atomic Habits: An Easy & Proven Way to Build Good Habits & Break Bad Ones",
                    Description = "A practical guide to building good habits and breaking bad ones.",
                    ISBN = "978-0-7352-1129-2",
                    PublishYear = 2018,
                    TotalCopies = 6,
                    AvailableCopies = 6,
                    Image = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1535115320i/40121378.jpg",
                    CategoryId = nonFiction.CategoryId,
                    AuthorId = clear.AuthorId
                },
                new Book
                {
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Description = "The first book in the Harry Potter series.",
                    ISBN = "978-0-590-35340-3",
                    PublishYear = 1997,
                    TotalCopies = 2,
                    AvailableCopies = 2,
                    Image = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1474154022i/3.jpg",
                    CategoryId = fiction.CategoryId,
                    AuthorId = rowling.AuthorId
                },
                new Book
                {
                    Title = "The Lean Startup",
                    Description = "How Today's Entrepreneurs Use Continuous Innovation to Create Radically Successful Businesses.",
                    ISBN = "978-0-307-88789-4",
                    PublishYear = 2011,
                    TotalCopies = 7,
                    AvailableCopies = 7,
                    Image = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1333576876i/10127019.jpg",
                    CategoryId = business.CategoryId,
                    AuthorId = ries.AuthorId
                },
                new Book
                {
                    Title = "Educated: A Memoir",
                    Description = "A memoir about a young girl who leaves her survivalist family and goes on to earn a PhD.",
                    ISBN = "978-0-399-59050-4",
                    PublishYear = 2018,
                    TotalCopies = 12,
                    AvailableCopies = 12,
                    Image = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1506026635i/35133922.jpg",
                    CategoryId = nonFiction.CategoryId,
                    AuthorId = westover.AuthorId
                },
                new Book
                {
                    Title = "The Subtle Art of Not Giving a F*ck",
                    Description = "A counterintuitive approach to living a good life.",
                    ISBN = "978-0-06-245771-4",
                    PublishYear = 2016,
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    Image = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1465761302i/28257707.jpg",
                    CategoryId = nonFiction.CategoryId,
                    AuthorId = manson.AuthorId
                },
                new Book
                {
                    Title = "The Great Gatsby",
                    Description = "A novel set in the Jazz Age that tells the story of Jay Gatsby.",
                    ISBN = "978-0-7432-7356-5",
                    PublishYear = 1925,
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    Image = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1490528560i/4671.jpg",
                    CategoryId = fiction.CategoryId,
                    AuthorId = fitzgerald.AuthorId
                }
            );
            context.SaveChanges();
        }
    }
}
