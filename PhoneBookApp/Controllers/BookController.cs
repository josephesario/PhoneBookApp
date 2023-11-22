using dbContext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

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
            using var transaction = _dbContext.Database.BeginTransaction();

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
                    var output1 = _dbContext.SaveChanges();

                    if (output1 > 0) {
                         transaction.Commit();
                         return Ok("Inserted Successfully");
                    }
                    else
                    {
                        transaction.Rollback();
                        return BadRequest("Insertion Failed");
                    }
                     

                }
                else
                {
                    transaction.Rollback();
                    return NoContent();
                }

            } catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Exeption: Date {DateTime.Now} - Exeption Info - {ex}");
            }



        }


        [HttpPatch("UpdatePhoneNumberBy/{phoneNumber}")]
        public async Task<ActionResult<TBook>> UpdatePhoneBook_Title(string phoneNumber , HBookTitle Title)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
              
                var output = await _dbContext.TBooks.Where(e => e.PhoneNumber.Equals(phoneNumber)).FirstOrDefaultAsync();

                if (output != null)
                {

                    bool status0 = await BookTitleAlreadyExistsAsync(Title.Title);
                    bool status1 = await BookAlPhonereadyExistsAsync(phoneNumber);

                    if (status1)
                    {
                        if (status0)
                        {
                            transaction.Rollback();
                            return Conflict("Title Already Exist, Use A different Title!!");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return NotFound();
                    }

                    output.Title = Title.Title;
                    var output1 = _dbContext.SaveChanges();

                    if (output1 > 0) { 
                        transaction.Commit();
                        return Ok("Updated Successfully");
                    }
                    else {
                        transaction.Rollback();
                        return BadRequest("Update Failed");
                    }


                }
                else
                {
                    transaction.Rollback();
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Exeption: Date {DateTime.Now} - Exeption Info - {ex}");
            }
        }

        [HttpPatch("UpdatePhoneNumberBy/{title}")]
        public async Task<ActionResult<TBook>> UpdatePhoneNumberBy(string title, HBookPhone Phone)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try { 

                var output = await _dbContext.TBooks.Where(e=>e.Title.Equals(title)).FirstOrDefaultAsync();
                
                if(output != null) {


                    bool status0  = await BookTitleAlreadyExistsAsync(title);
                    bool status1 = await BookAlPhonereadyExistsAsync(Phone.PhoneNumber);

                    if (status0)
                    {
                        if (status1)
                        {
                            transaction.Rollback();
                            return Conflict("Phone Number Already Exist, Use A different Phone number!!");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return NotFound();
                    }

                    output.PhoneNumber = Phone.PhoneNumber;
                    var output1 = _dbContext.SaveChanges();

                    if (output1 > 0) {

                        transaction.Commit();
                        return Ok("Updated Successfully");
                    }
                    else 
                    {
                        transaction.Rollback();
                        return BadRequest("Update Failed");
                    }


                }
                else
                {
                    transaction.Rollback();
                    return NoContent();
                }

            }catch(Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Exeption: Date {DateTime.Now} - Exeption Info - {ex}");
            }
        }



        public async Task<bool> BookAlreadyExistsAsync(HBook_Validation book_Validation)
        {

            var output = await _dbContext.TBooks.AnyAsync(e => e.Title.Equals(book_Validation.Title) || e.PhoneNumber.Equals(book_Validation.PhoneNumber));
            return output;
        }

        public async Task<bool> BookTitleAlreadyExistsAsync(string  title)
        {

            var output = await _dbContext.TBooks.AnyAsync(e => e.Title.Equals(title));
            return output;
        }

        public async Task<bool> BookAlPhonereadyExistsAsync(string phoneNumber)
        {

            var output = await _dbContext.TBooks.AnyAsync(e => e.PhoneNumber.Equals(phoneNumber));
            return output;
        }

    }
}