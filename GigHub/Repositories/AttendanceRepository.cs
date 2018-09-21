using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.Repositories
{
    public class AttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Attendance> GetFutureAttendances(string userId) // returns a list of attendances
        {
            return _context.Attendances // loads all the user's attendances 
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList();
        }

        public Attendance GetAttendance(int gigId, string userId)
        {
            return _context.Attendances //to initialize the attendance property and
                .SingleOrDefault(a => a.GigId == gigId && a.AttendeeId == userId);  //check for matching objects with this criteria
        }
    }
}