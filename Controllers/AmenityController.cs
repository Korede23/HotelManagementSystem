using AspNetCoreHero.ToastNotification.Abstractions;
using HMS.Implementation.Interface;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize]
public class AmenityController : Controller
{

    private readonly IAmenityService _amenityService;
    private readonly INotyfService _notyf;

    public AmenityController(IAmenityService amenityService ,INotyfService notyf)
    {
        _amenityService = amenityService;
        _notyf = notyf;
    }

    [HttpGet("get-amenity")]
    public async Task<IActionResult> Amenities()
    {
        var amenity = await _amenityService.GetAmenity();
        return View(amenity);
    }


    [HttpGet("create-amenity")]
    public async Task<IActionResult> CreateAmenity()
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
            _notyf.Success(amenity.Message, 3);
            return RedirectToAction("Amenities");
        }
        _notyf.Error(amenity.Message);
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
            _notyf.Success(amenity.Message, 3);
            return RedirectToAction("Amenities", "Amenity");
        }
        _notyf.Error(amenity.Message);
        return View(request);
    }


   

    [HttpGet("delete-amenity/{id}")]
    public async Task<IActionResult> DeleteAmenity([FromRoute] Guid id)
    {
        var amenity = await _amenityService.DeleteAmenity(id);
        if (amenity.Success)
        {
            _notyf.Success(amenity.Message, 3);
            return RedirectToAction("Amenities", "Amenity");
        }
        _notyf.Error(amenity.Message);
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

    [HttpGet("get-amenity-by-id/{id}")]
    public async Task<IActionResult> GetAmenityBYId(Guid Id) 
        {
        var amenity = await _amenityService.GetAmenityBYId( Id);
        if (amenity.Success)
        {
            _notyf.Success(amenity.Message, 3);
            return View(amenity.Data);
        }
        _notyf.Error(amenity.Message);
        return RedirectToAction("Amenities");
    }


}
