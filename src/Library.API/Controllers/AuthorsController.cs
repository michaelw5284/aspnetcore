namespace Library.API.Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices.ComTypes;
    using System.Threading.Tasks;

    using Library.API.Models;
    using Library.API.Services;
    using Library.API.Helpers;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/authors")]
    public class AuthorsController
    {
        private ILibraryRepository libraryRepository;


        public AuthorsController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var authorsfromRepo = this.libraryRepository.GetAuthors();
            var authors = new List<AuthorDto>();


            foreach (var author in authorsfromRepo)
            {
                authors.Add(new AuthorDto()
                {
                    Id = author.Id,
                    Name = $"{author.FirstName} {author.LastName}",
                    Age = author.DateOfBirth.GetCurrentAge(),
                    Genre = author.Genre
                });
            }
            return new JsonResult(authors);
        }


    }
}
