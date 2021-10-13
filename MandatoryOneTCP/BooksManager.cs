using MandatoryOneLibrary;
using System.Collections.Generic;

namespace MandatoryOneTCP
{
    public class BooksManager
    {
        private List<Book> _books = new List<Book>{
            new Book {Title = "Computer Networking, Global Edition",
                    Author = "James Kurose, Keith Ross",
                    PageNumber = 800, ISBN13 = "9781292405469"},
            new Book {Title = "Extreme Programming Explained",
                    Author = "Kent Beck, Cynthia Andres",
                    PageNumber = 224, ISBN13 = "9780321278654"},
            new Book {Title = "The Agile Samurai",
                    Author = "Jonathan Rasmusson",
                    PageNumber = 280, ISBN13 = "9781934356586"}
            };

        public IEnumerable<Book> GetAll()
        {
            return _books;
        }

        public Book GetByISBN(string isbn)
        {
            return _books.Find(x => x.ISBN13 == isbn);
        }

        public Book PostBook(Book book)
        {
            if (!_books.Contains(book))
            {
                _books.Add(book);
            }

            return book;
        }

        public Book PutBook(string ISBN, Book updates)
        {
            Book book = _books.Find(book1 => book1.ISBN13 == ISBN);
            if (book == null) return null;
            book.Title = updates.Title;
            book.PageNumber = updates.PageNumber;
            book.Author = updates.Author;
            return book;
        }

        public Book DeleteBook(string isbn)
        {
            Book book = _books.Find(x => x.ISBN13 == isbn);
            if (book != null)
            {
                _books.RemoveAll(x => x.ISBN13 == isbn);
                return book;
            }
            return null;
        }
    }
}
