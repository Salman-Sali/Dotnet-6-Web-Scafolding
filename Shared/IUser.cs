using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using System.Security.Claims;

namespace Shared
{
    public interface IUser
    {
        public int UserId { get; }
        public Domain.Entities.UserType UserType { get; }
    }

    public class User : IUser
    {
        public int UserId => GetUserId();

        public Domain.Entities.UserType UserType => GetUserType();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public User(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId()
        {
            var claim = _httpContextAccessor.HttpContext.User.Claims.Where(x=>x.Type == ClaimTypes.Sid).FirstOrDefault();
            if(claim == null)
            {
                throw new HhcmException("Something went wrong.", System.Net.HttpStatusCode.InternalServerError);
            }
            return int.Parse(claim.Value);
        }

        private Domain.Entities.UserType GetUserType()
        {
            var claim = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.AuthorizationDecision).FirstOrDefault();
            if (claim == null)
            {
                throw new HhcmException("Something went wrong.", System.Net.HttpStatusCode.InternalServerError);
            }
            return (Domain.Entities.UserType)Enum.Parse(typeof(Domain.Entities.UserType),claim.Value);
        }
    }
}
