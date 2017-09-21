namespace Library.API.Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.InteropServices.ComTypes;
    using System.Threading.Tasks;

    using AutoMapper;

    using Library.API.Models;
    using Library.API.Services;
    using Library.API.Helpers;

    using Microsoft.AspNetCore.Mvc;
    using Library.API.Entities;

    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private ILibraryRepository libraryRepository;


        public AuthorsController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        [HttpGet()]
        public IActionResult GetAuthors()
        {


            var authorsFromRepo = this.libraryRepository.GetAuthors();
            var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            return this.Ok(authors);

            return this.StatusCode(500, "This is an error");
        }

        [HttpGet("{id}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid id)
        {
            var authorFromRepo = this.libraryRepository.GetAuthor(id);
            if (authorFromRepo == null)
            {
                return this.NotFound();
            }
            var author = Mapper.Map<AuthorDto>(authorFromRepo);
            return new JsonResult(author);

        }

        // INFO Create an author together with a list of books - see the repository and the DTO
        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorForCreationDto author)
        {
            if (author == null)
            {
                return this.BadRequest();
            }

            var authorEntity = Mapper.Map<Author>(author);

            this.libraryRepository.AddAuthor(authorEntity);
            if (!this.libraryRepository.Save())
            {

                // INFO - could do this either way.  Throw is handed in middleware
                throw new Exception("Creating author failed");

                // return this.StatusCode(500, "A problem happened and could not save");
            }
            var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);

            // INFO - how to return the route of a created entity
            return this.CreatedAtRoute("GetAuthor", new { id = authorToReturn.Id }, authorToReturn);
       
        }


    }
}
