namespace Library.API.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    using AutoMapper;

    using Library.API.Models;
    using Library.API.Services;
    using Library.API.Entities;

    [Route("api/authors/{authorid}/books")]
    public class BooksController : Controller
    {
        private ILibraryRepository libraryRepository;

        public BooksController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        [HttpGet]
        public IActionResult GetBooksForAuthor(Guid authorid)
        {
            if (!this.libraryRepository.AuthorExists(authorid))
            {
                return this.NotFound();
            }

            var booksForAuthorFromRepo = this.libraryRepository.GetBooksForAuthor(authorid);

            var booksForAuthor = Mapper.Map<IEnumerable<BooksDto>>(booksForAuthorFromRepo);
            return this.Ok(booksForAuthor);
        }

        [HttpGet("{id}", Name = "GetBookForAuthor")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid id)
        {
            if (!this.libraryRepository.AuthorExists(authorId))
            {
                return this.NotFound();
            }

            var bookForAuthorFromRepo = this.libraryRepository.GetBookForAuthor(authorId, id);
            if (bookForAuthorFromRepo == null)
            {
                return this.NotFound();
            }

            var bookForAuthor = Mapper.Map<IEnumerable<BooksDto>>(bookForAuthorFromRepo);
            return this.Ok(bookForAuthor);
        }

        public IActionResult CreateBookForAuthor(Guid authorId, [FromBody] BookForCreationDto book)
        {
            if (book == null)
            {
                return this.BadRequest();
            }

            if (!this.libraryRepository.AuthorExists(authorId))
            {
                return this.NotFound();
            }
            var bookEntity = Mapper.Map <Book>(book);

            this.libraryRepository.AddBookForAuthor(authorId, bookEntity);
            if (!this.libraryRepository.Save())
            {
                throw new Exception($"Unable to create book for author {authorId}");
            }

            var bookToReturn = Mapper.Map<BooksDto>(bookEntity);

            return this.CreatedAtRoute("GetBookForAuthor", new { authorId = authorId, id = bookToReturn.Id }, bookToReturn);
        }
    }
}
