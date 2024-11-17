using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Google.Apis.Books.v1;
using Google.Apis.Services;

namespace BookTracker.Pages
{
    public class BooksModel : PageModel
    {
        private readonly AppDbContext _context;

        public List<Book> Books { get; set; } = new List<Book>();

        // Lista książek pobranych z Google Books
        public List<GoogleBook> GoogleBooks { get; set; } = new List<GoogleBook>();

        public int PageSize { get; set; } = 5;  // Liczba książek na stronę
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        [BindProperty]
        public string Title { get; set; } = string.Empty; // Tytuł książki

        [BindProperty]
        public string Author { get; set; } = string.Empty; // Autor książki

        [BindProperty]
        public string Status { get; set; } = "To Read"; // Ustawienie domyślnego statusu

        [BindProperty]
        public string SearchQuery { get; set; } = string.Empty; // Zapytanie do Google Books

        public BooksModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet(int pageNumber = 1)
        {
            PageNumber = pageNumber;
            Books = _context.Books
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            TotalPages = (int)Math.Ceiling(_context.Books.Count() / (double)PageSize);
        }

        // Obsługa wyszukiwania książek w Google Books
        public void OnPostSearchGoogleBooks()
        {
            var service = new BooksService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyC-2tZ2sgdKxTm9hBeodfxMM2BUbj4NR3U" 
            });

            var request = service.Volumes.List(SearchQuery);
            var result = request.Execute();

            if (result.Items != null)
            {
                GoogleBooks = result.Items.Select(book => new GoogleBook
                {
                    Title = book.VolumeInfo?.Title ?? "No Title Available", // Handle null title
                    Author = book.VolumeInfo?.Authors?.FirstOrDefault() ?? "Unknown Author", // Handle null authors
                    CoverImageUrl = book.VolumeInfo?.ImageLinks?.Thumbnail ?? "default-cover-url.jpg" // Handle null image link
                }).ToList();
            }
        }

        // Dodanie książki z Google Books do lokalnej bazy danych
        public IActionResult OnPostAddGoogleBook(string title, string author, string coverImageUrl)
        {
            var newBook = new Book
            {
                Title = title,
                Author = author,
                Status = "To Read",
                DateAdded = DateTime.Now,
                CoverImageUrl = coverImageUrl // Przechowywanie URL okładki książki
            };

            _context.Books.Add(newBook);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newBook = new Book
            {
                Title = Title,
                Author = Author,
                Status = Status,
                DateAdded = DateTime.Now
            };

            _context.Books.Add(newBook);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostExportToCSV()
        {
            var books = _context.Books.ToList();
            var csv = new StringBuilder();
            csv.AppendLine("Title,Author,Status,DateAdded");

            foreach (var book in books)
            {
                csv.AppendLine($"{book.Title},{book.Author},{book.Status},{book.DateAdded:dd-MM-yyyy}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "books.csv");
        }
    }

    // Model dla książek pobieranych z Google Books
    public class GoogleBook
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
    }
}
