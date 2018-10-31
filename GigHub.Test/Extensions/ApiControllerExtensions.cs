using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.Test.Extensions
{
    public static class ApiControllerExtensions 
    {
        public static void MockCurrentUser(this ApiController controller, string userId, string username) // this is the extension method
        {
            var identity = new GenericIdentity("username"); // user to mock

            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", username)); // using the Claim class
            identity.AddClaim(
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId)); // the user ID we give to the user

            var principal = new GenericPrincipal(identity, null); // this the generic principal object first parameter is identity the second is the role in this case null

            controller.User = principal; // sets User to principal
        }
    }
}
