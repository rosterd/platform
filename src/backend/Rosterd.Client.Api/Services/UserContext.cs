using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rosterd.Data.TableStorage.Models;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Exceptions;
using Rosterd.Services.Staff.Interfaces;

namespace Rosterd.Client.Api.Services
{
    /// <summary>
    ///     The context provides various bits of information about the logged in user, their roles and claims
    ///     The tenant this current user belongs to is cached for the specified duration
    /// </summary>
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRosterdAppUserService _appUserService;
        private readonly IStaffService _staffService;

        public UserContext(IHttpContextAccessor httpContextAccessor, IRosterdAppUserService appUserService, IStaffService staffService)
        {
            _httpContextAccessor = httpContextAccessor;
            _appUserService = appUserService;
            _staffService = staffService;
        }

        public async Task<RosterdAppUser> GetRosterdAppUserOrCreateIfNotExists()
        {
            if (UserAuth0Id.IsNullOrEmpty())
                throw new EntityNotFoundException("Forbidden.");

            var rosterdAppUser = await _appUserService.GetStaffAppUser(UserAuth0Id);
            if (rosterdAppUser == null)
            {
                //Grab all necessary mapping details for a staff
                var (staffId, organizationId, auth0OrganizationId) = await _staffService.GetStaff(UserAuth0Id);

                //Save the details to table storage
                await _appUserService.CreateOrUpdateStaffAppUser(UserAuth0Id, staffId, auth0OrganizationId, organizationId);

                rosterdAppUser = new RosterdAppUser(UserAuth0Id) { Auth0OrganizationId = auth0OrganizationId, StaffId = staffId, OrganizationId = organizationId };
            }

            return rosterdAppUser;
        }

        public string UserAuth0Id
        {
            get
            {
                var auth0Id = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return auth0Id;
            }
        }

        public long UserStaffId { get; set; }
        public string UsersAuth0OrganizationId { get; set; }
        public long UsersOrganizationId { get; set; }

        public string AccessToken
        {
            get
            {
                var accessToken = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == RosterdConstants.AccessTokenFields.AccessToken).Value;
                return accessToken;
            }
        }

        //TODO Not working, need to debug why roles are not available
        public IEnumerable<string> Roles => _httpContextAccessor.HttpContext.User.Claims.Where(s => s.Type == RosterdConstants.AccessTokenFields.Roles).Select(s => s.Value);

        public bool IsUserInRole(RosterdRoleEnum rosterdRole) => !Roles.IsNullOrEmpty() && Roles.Contains(rosterdRole.ToString());

        public bool IsUserRosterdAdmin() => IsUserInRole(RosterdRoleEnum.RosterdAdmin);
        public bool IsUserFacilityAdmin() => IsUserInRole(RosterdRoleEnum.FacilityAdmin);
        public bool IsUserOrganizationAdmin() => IsUserInRole(RosterdRoleEnum.OrganizationAdmin);
        public bool IsUserAStaff() => IsUserInRole(RosterdRoleEnum.Staff);

        public string UserEmailAddress =>
            throw new NotImplementedException(
                "Not yet implemented, when we need this we can add it to the access token and grab it from there, for now the access token does not have this.");

        public string UsersFirstName =>
            throw new NotImplementedException(
                "Not yet implemented, when we need this we can add it to the access token and grab it from there, for now the access token does not have this.");

        public string UsersLastName =>
            throw new NotImplementedException(
                "Not yet implemented, when we need this we can add it to the access token and grab it from there, for now the access token does not have this.");

        public string UsersPhoneNumber =>
            throw new NotImplementedException(
                "Not yet implemented, when we need this we can add it to the access token and grab it from there, for now the access token does not have this.");

    }
}
