using HMS.Implementation.Interface;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using Microsoft.AspNetCore.Mvc;

public class AmenityController : Controller
{
    private readonly IAmenityService _amenityService;

    public AmenityController(IAmenityService amenityService)
    {
        _amenityService = amenityService;
    }

    [HttpGet("get-amenity")]
    public async Task<IActionResult> Index()
    {
        var amenity = await _amenityService.GetAmenity();
        return View(amenity);
    }


    [HttpGet("create-amenity")]
    public async Task<IActionResult> Create()
    {
        var amenity = await _amenityService.GetAllAmenity();
        if (amenity.Success)
        {

            ViewBag.Rooms = amenity.Data;
            return View();
            // return View(amenity);
        }
        return BadRequest();
    }


    [HttpPost("create-amenity")]
    public async Task<IActionResult> CreateAmenity(CreateAmenityRequestModel request, int Id)
    {
        var amenity = await _amenityService.CreateAmenity(request);
        if (amenity.Success)
        {
            return RedirectToAction("Index");
        }
        return BadRequest();
    }


    [HttpGet("edit-amenity/{id}")]
    public async Task<IActionResult> EditAmenity([FromRoute] Guid id)
    {
        var amenity = await _amenityService.GetAmenityAsync(id);

        return View(amenity.Data);
    }


    [HttpPost("edit-amenity/{id}")]
    public async Task<IActionResult> EditAmenity(UpdateAmenity request)
    {

        var amenity = await _amenityService.UpdateAmenity(request.Id, request);
        if (amenity.Success)
        {
            return RedirectToAction("Index" , "Amenity");
        }
        return View(request);
    }


   

    [HttpGet("delete-amenity/{id}")]
    public async Task<IActionResult> DeleteAmenity([FromRoute] Guid id)
    {
        var amenity = await _amenityService.DeleteAmenity(id);
        if (amenity.Success)
        {
            return RedirectToAction("Index", "Amenity");
        }

        return BadRequest(amenity);

    }


    [HttpGet("get-all-amenity-created")]
    public async Task<IActionResult> GetAmenityById(Guid id) 
    {
        var amenity = await _amenityService.GetAmenityBYId(id);
        if (amenity.Success == false)
        {
            return View(amenity);
        }
        else
        {
            return BadRequest(amenity);
        }


    }

    [HttpGet("get-booking-by-id/{id}")]
    public async Task<IActionResult> GetAllAmenity() 
    {
        var amenity = await _amenityService.GetAllAmenity();
        if (amenity.Success == false)
        {
            return Ok(amenity);
        }
        else
        {
            return BadRequest(amenity);
        }
    }


}
