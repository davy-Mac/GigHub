using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistance;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();

            if (_context.Attendances.Any(a =>
                a.AttendeeId == userId && a.GigId == dto.GigId))
                return BadRequest("The attendance already exist.");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };
            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {
            var userId = User.Identity.GetUserId(); // gets the id of the currently logged in user

            var attendance = _context.Attendances // checks for attendances with matching attendeeId equals userId
                .SingleOrDefault(a => a.AttendeeId == userId && a.GigId == id); // & GigId equals to the "id" argument received in this action

            if (attendance == null) // if attendance is not found
                return NotFound();

            _context.Attendances.Remove(attendance); // if found remove from attendances
            _context.SaveChanges();    

            return Ok(id); // returns ok and puts the id in the response by RESTFUL convention
        }
    }
}
