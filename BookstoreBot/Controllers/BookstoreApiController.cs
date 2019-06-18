using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookstoreBot.Models;
using BookstoreBot.Repositories;

namespace BookstoreBot.Controllers
{
    public class BookstoreApiController : ApiController
    {
        [Route("api/a")]
        public IEnumerable<Book> getBooks()
        {
            List<Book> CarSalesNumber= new List<Book>
            {
                new Book { BookId = "1", BooksName= "BMW", AuthorName = "1G" },
                new Book { BookId = "2", BooksName = "BENZ", AuthorName = "2G" }
            };
            return CarSalesNumber;
        }
        [Route("api/b")]
        public IEnumerable<Book> getBooka()
        {
            List<Book> CarSalesNumber = new List<Book>
            {
                new Book { BookId = "3", BooksName= "BMW", AuthorName = "1G" },
                new Book { BookId = "4", BooksName = "BENZ", AuthorName = "2G" }
            };
            return CarSalesNumber;
        }
        
    }
}
