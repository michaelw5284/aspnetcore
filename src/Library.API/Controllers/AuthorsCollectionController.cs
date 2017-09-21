namespace Library.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Library.API.Entities;
    using Library.API.Helpers;
    using Library.API.Models;
    using Library.API.Services;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

    [Route("api/authorcollections")]
    public class AuthorsCollectionController : Controller
    {
        private ILibraryRepository libraryRepository;

        public AuthorsCollectionController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        //[HttpPost]
        //public IActionResult test()
        //{
        //    return this.Ok();
        //}

        [HttpPost]
        public IActionResult CreateAuthorCollection([FromBody] IEnumerable<AuthorForCreationDto> authorCollection)
        {
            if (authorCollection == null)
            {
                return this.BadRequest();
            }

            var authorEntities = Mapper.Map<IEnumerable<Author>>(authorCollection);

            foreach (var author in authorEntities)
            {
                this.libraryRepository.AddAuthor(author);
            }

            if (!this.libraryRepository.Save())
            {
                throw new Exception("Error creating author collection");
            }

            var authorCollectionToReturn = Mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            var idsAsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));


            return this.CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorCollectionToReturn);
        }


        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollections([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            // INFO Use of a custom model binder
            // How to return a collection of CreatedAt resources in the header
            if (ids == null)
            {
                return this.BadRequest();
            }

            var authorEntites = this.libraryRepository.GetAuthors(ids);

            if (ids.Count() != authorEntites.Count())
            {
                return this.NotFound();
            }

            var authorsToReturn = Mapper.Map<IEnumerable<AuthorDto>>(authorEntites);

            return this.Ok(authorsToReturn);
        }

    }
}
