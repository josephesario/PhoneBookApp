using dbContext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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

        [HttpGet("GetAllPhoneBooks")]
        public async Task<ActionResult<TBook>> GetAllPhoneBooks()
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var output = await _dbContext.TBooks.ToListAsync();
                if (output.Count > 0)
                {
                    transaction.Commit();
                    return Ok(output);
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

       
        [HttpGet("GetPhoneBookBySearch/{search}")]
        public async Task<ActionResult<TBook>> GetPhoneBookBySearch([Required] string search)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {

                var output = await _dbContext.TBooks.Where(e => e.FistName.Contains(search) || e.LastName.Contains(search) || e.PhoneNumber.Contains(search)).ToListAsync();

                if (output.Count > 0)
                {
                    transaction.Commit();
                    return Ok(output);
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


        [HttpPost("AddPhoneBook")]
        public async Task<IActionResult> AddPhoneBook(HBook hBook)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try {

               

                if (hBook != null) {


                    HBook_Validation hBook_Validation = new()
                    {
                        PhoneNumber = hBook.PhoneNumber,
                        FistName = hBook.FistName,
                        LastName = hBook.LastName
                    };

                    bool accountExists = await BookAlreadyExistsAsync(hBook_Validation);



                    TBook book = new()
                    {
                        PhoneNumber = hBook.PhoneNumber,
                        RegistrationDate = DateTime.Now,
                        FistName = hBook.FistName,
                        LastName = hBook.LastName,

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

                    bool status0 = await BookFullNameAlreadyExistsAsync(Title.FistName,Title.LastName);
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

                    output.FistName = Title.FistName;
                    output.LastName = Title.LastName;

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



        [HttpPatch("UpdatePhoneNumberBy/{FistName}/{LastName}")]
        public async Task<ActionResult<TBook>> UpdatePhoneNumberBy(string FistName,string LastName, HBookPhone Phone)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try { 

                var output = await _dbContext.TBooks.Where(e=>e.FistName.Equals(FistName) && e.LastName.Equals(LastName)).FirstOrDefaultAsync();
                
                if(output != null) {


                    bool status0  = await BookFullNameAlreadyExistsAsync(FistName, LastName);
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

            var output = await _dbContext.TBooks.AnyAsync(e => e.FistName.Equals(book_Validation.FistName) && e.LastName.Equals(book_Validation.LastName) || e.PhoneNumber.Equals(book_Validation.PhoneNumber));
            return output;
        }

        public async Task<bool> BookFullNameAlreadyExistsAsync(string  FistName, string LastName)
        {

            var output = await _dbContext.TBooks.AnyAsync(e => e.FistName.Equals(FistName) && e.LastName.Equals(LastName));
            return output;
        }

        public async Task<bool> BookAlPhonereadyExistsAsync(string phoneNumber)
        {

            var output = await _dbContext.TBooks.AnyAsync(e => e.PhoneNumber.Equals(phoneNumber));
            return output;
        }

    }
}