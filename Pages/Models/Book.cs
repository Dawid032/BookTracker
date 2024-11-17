using System;

namespace BookTracker.Models
{
    public class Book
    {
        public int Id { get; set; } // Id książki
        public string Title { get; set; } = string.Empty;  // Tytuł książki
        public string Author { get; set; } = string.Empty; // Autor książki
        public string Status { get; set; } = "To Read";    // Status książki
        public DateTime DateAdded { get; set; } = DateTime.Now; // Data dodania
         public string CoverImageUrl { get; set; } = string.Empty;
    }
}
