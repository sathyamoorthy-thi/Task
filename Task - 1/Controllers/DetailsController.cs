
using Microsoft.AspNetCore.Mvc;
using PaymentClaimApi.Data;
using PaymentClaimApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace PaymentClaimApi.Controllers
{

[Route("api/[controller]")]           //Routing Attribute..
[ApiController]


//[Authorize]
[EnableCors("MyPolicy")]
public class DetailsController : ControllerBase
{

  private readonly ApplicationDbContext _dbcontext;
  private readonly ILogger<DetailsController> _logger;

  public DetailsController(ApplicationDbContext dbcontext,ILogger<DetailsController> logger)
  {
    _dbcontext = dbcontext;

    _logger = logger;
  }

[HttpGet]
public  ActionResult<IEnumerable<ClaimDetails>> Get()
{
    return _dbcontext.detail.ToList();
}


[HttpGet("api/claims/{id:range(1, 100)}")]     //The range(1, 100) constraint specifies that the "id" parameter should be within the range of 1 to 100.
 
public ActionResult<ClaimDetails> GetById(int id)
{
    try
    {
        ClaimDetails claim = _dbcontext.detail.Find(id);

        if (claim == null)
        {
            return NotFound(); // Return a 404 Not Found response if the claim is not found
        }

        return claim;
    }
    catch (Exception ex)
    {
        _logger.LogError($"An error occurred while retrieving claim with id {id}: {ex.Message}");
        return StatusCode(500); // Return a 500 Internal Server Error response for any other exceptions
    }
}



[HttpPut]
public ActionResult<ClaimDetails> Update([FromBody] ClaimDetails claimdetails)
   {
       _dbcontext.detail.Update(claimdetails);
       _dbcontext.SaveChanges();
       return Ok();

   }

[HttpPost]

public ActionResult<ClaimDetails> Create([FromBody] ClaimDetails claimdetails)
{
    _dbcontext.detail.Add(claimdetails);
    _dbcontext.SaveChanges();
    return Ok();

}


[HttpDelete]
[Route("Delete/{id:int}")]
public ActionResult DeleteById(int id)
{
    var data = _dbcontext.detail.Find(id);
    _dbcontext.detail.Remove(data);
    _dbcontext.SaveChanges();
    return NoContent();
}


}
}