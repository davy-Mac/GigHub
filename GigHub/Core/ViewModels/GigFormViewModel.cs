using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;
using GigHub.Controllers;
using GigHub.Core.Models;

namespace GigHub.Core.ViewModels
{
    public class GigFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public string Heading { get; set; }

        public string Action // if the Id exist "Update" otherwise "Create"
        {
            get
            {
                Expression<Func<GigsController, ActionResult>> update = (c => c.Update(this)); // Func is a delegate that takes a GigsController & returns ActionResult
                Expression<Func<GigsController, ActionResult>> create = (c => c.Create(this)); // Func is a delegate that takes a GigsController & returns ActionResult

                var action = (Id != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;

                //return (Id != 0) ? "Update" : "Create"; // hard coded cases for Update or Create
            }
        }

        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));

        }
    }
}
