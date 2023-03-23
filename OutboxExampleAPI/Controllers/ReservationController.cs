using Microsoft.AspNetCore.Mvc;
using OutboxExample.Contracts.Persistence.Models;
using OutboxExample.Contracts.Services;
using OutboxExampleAPI.DTOs;

namespace OutboxExampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }
        [HttpPost]
        [Route("submit")]
        public async Task<IActionResult> SubmitReservation(ReservationCreateModel reservation)
        {
            await reservationService.Submit(
                reservation.CustomerId, 
                reservation.TableNo,
                reservation.StartFrom,
                reservation.EndIn, 
                reservation.PeopleCount);

            return Ok();
        }
    }
}
