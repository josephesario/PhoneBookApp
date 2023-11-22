using dbContext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PhoneBookApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BookController : ControllerBase
    {

        private readonly PhoneBookDbContext _dbContext;

        public BookController(PhoneBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("AddPhoneBook")]

        public async Task<IActionResult> AddPhoneBook(HBook hBook)
        {

            try {

                if (hBook != null) {


                    HBook_Validation hBook_Validation = new()
                    {
                        PhoneNumber = hBook.PhoneNumber,
                        Title = hBook.Title
                    };

                    bool accountExists = await BookAlreadyExistsAsync(hBook_Validation);



                    TBook book = new()
                    {
                        BookId = hBook.BookId,
                        PhoneNumber = hBook.PhoneNumber,
                        RegistrationDate = DateTime.Now,
                        Title = hBook.Title,

                    };

                    var output = await _dbContext.TBooks.AddAsync(book);
                    return Ok(output);

                }
                else
                {
                    return NoContent();
                }

            } catch (Exception ex)
            {
                return BadRequest($"Exeption: Date {DateTime.Now} - Exeption Info - {ex}");
            }



        }



        public async Task<bool> BookAlreadyExistsAsync(HBook_Validation book_Validation)
        {

            var output = await _dbContext.TBooks.AnyAsync(e => e.Title.Equals(book_Validation.Title) || e.PhoneNumber.Equals(book_Validation.PhoneNumber));
            return output;
        }

    }
}