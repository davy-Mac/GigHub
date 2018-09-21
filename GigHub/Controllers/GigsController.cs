﻿using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Repositories;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly GigRepository _gigRepository;
        private readonly FollowingRepository _followingRepository;
        private readonly GenreRepository _genreRepository;

        public GigsController() // this is the constructor
        {
            _context = new ApplicationDbContext(); // initializes the object
            _attendanceRepository = new AttendanceRepository(_context);  // initializes the object
            _gigRepository = new GigRepository(_context);   // initializes the object
            _followingRepository = new FollowingRepository(_context);
            _genreRepository = new GenreRepository(_context);
        }

        public ActionResult Details(int id)  // this action returns a view with the details of the particular gig
        {
            var gig = _gigRepository.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel { Gig = gig }; // initializes the viewModel

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId(); // gets the Id of the current logged in user

                viewModel.IsAttending = _attendanceRepository.GetAttendance(gig.Id, userId) != null;

                viewModel.IsFollowing = _followingRepository.GetFollowing(userId, gig.ArtistId) != null;
            }

            return View("Details", viewModel);
        }

        [Authorize]
        public ActionResult Mine() // this method returns a list of my gigs
        {
            var userId = User.Identity.GetUserId();
            var gigs = _gigRepository.GetUpcomingGigsByArtist(userId);

            return View(gigs);
        }

        [Authorize]
        public ActionResult Attending() // loads all Gigs attended by the logged in user
        {
            var userId = User.Identity.GetUserId();


            var viewModel = new GigsViewModel() // initializes the GigsViewModel
            {
                UpcomingGigs = _gigRepository.GetGigsUserAttending(userId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                Attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId)
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
                Genres = _genreRepository.GetGenres(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var gig = _gigRepository.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

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

            var gig = _gigRepository.GetGigWithAttendees(viewModel.Id);

            if (gig == null)   // checks if the gig is null
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId()) 
                return new HttpUnauthorizedResult();

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);
            //gig.Venue = viewModel.Venue;
            //gig.DateTime = viewModel.GetDateTime();
            //gig.GenreId = viewModel.Genre;

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }
    }
}
