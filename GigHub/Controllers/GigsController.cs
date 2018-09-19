﻿using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Details(int id)  // this action returns a view with the details of the particular gig
        {
            var gig = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == id); // to get the gig Artist and Genre with that GigId

            if (gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel { Gig = gig }; // initializes the viewModel

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId(); // gets the Id of the current logged in user

                viewModel.IsAttending = _context.Attendances //to initialize the attendance property and
                    .Any(a => a.GigId == gig.Id && a.AttendeeId == userId);  //check for matching objects with this criteria

                viewModel.IsFollowing = _context.Followings // to initialize the Following property and
                    .Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userId); // check for matching objects with this criteria
            }

            return View("Details", viewModel);
        }

        [Authorize]
        public ActionResult Mine() // this method returns a list of my gigs
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Where(g =>
                    g.ArtistId == userId &&
                    g.DateTime > DateTime.Now &&
                    !g.IsCanceled) // this checks for canceled gigs to not be displayed 
                .Include(g => g.Genre)
                .ToList();

            return View(gigs);
        }

        [Authorize]
        public ActionResult Attending() // loads all Gigs attended by the logged in user
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            var attendances = _context.Attendances // loads all the user's attendances 
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);


            var viewModel = new GigsViewModel() // initializes the GigsViewModel
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                Attendances = attendances
            };

            return View("Gigs", viewModel);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new {query = viewModel.SearchTerm});
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            var viewModel = new GigFormViewModel
            {
                Heading = "Edit a Gig",
                Id = gig.Id,
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)  // this will be the view that will be posted when we hit the create action
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(), // parses the DateTime object and formats it using 2 placeholders Date & Time
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)  // this will be the view that will be posted when we hit the create action
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))  // Eager loads the attendees
                .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);
            //gig.Venue = viewModel.Venue;
            //gig.DateTime = viewModel.GetDateTime();
            //gig.GenreId = viewModel.Genre;

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }
    }
}
