using LimaBooks.Service.BookService;
using LimaBooks.Shared.Dtos;
using LimaBooks.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LimaBooks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            try
            {
                var result = await _bookService.GetByFilter(new BookFilter());
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(new { Error = $"{exception.Message}" });
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Filter([FromBody] BookFilter filter)
        {
            try
            {
                var result = await _bookService.GetByFilter(filter);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(new { Error = $"{exception.Message}" });
            }
        }

        [HttpPost("save")]
        public virtual async Task<IActionResult> Post([FromBody] BookDto dto)
        {
            try
            {
                var result = await _bookService.Save(dto);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(new { Error = $"{exception.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete([FromRoute] int? id)
        {
            try
            {
                await _bookService.Delete(id);
                return Ok(new { Deleted = true });
            }
            catch (Exception exception)
            {
                return BadRequest(new { Error = $"{exception.Message}" });
            }
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] BookDto dto)
        {
            try
            {
                var result = await _bookService.Update(dto);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(new { Error = $"{exception.Message}" });
            }
        }
    }
}
