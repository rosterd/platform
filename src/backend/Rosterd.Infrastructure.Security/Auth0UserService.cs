using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Rosterd.Domain;
using Rosterd.Domain.Enums;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.AdminUserModels;
using Rosterd.Infrastructure.Extensions;
using Rosterd.Infrastructure.Security.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Rosterd.Infrastructure.Security
{
    public class Auth0UserService : IAuth0UserService
    {
        private readonly IAuth0AuthenticationService _auth0AuthenticationService;
        private readonly IRolesService _rolesService;

        public Auth0UserService(IAuth0AuthenticationService auth0AuthenticationService, IRolesService rolesService)
        {
            _auth0AuthenticationService = auth0AuthenticationService;
            _rolesService = rolesService;
        }

        /// <inheritdoc />
        public async Task<Auth0UserModel> AddOrganizationAdminToAuth0(string auth0OrganizationId, Auth0UserModel adminUserModel) =>
            await AddUserToAuth0(auth0OrganizationId, adminUserModel, RosterdRoleEnum.OrganizationAdmin);

        /// <inheritdoc />
        public async Task<Auth0UserModel> UpdateOrganizationAdminInAuth0(string auth0OrganizationId, Auth0UserModel adminUserModel)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();

            //Validate if the provided user belongs to the given organization
            var allOrganizationsForUser = await auth0ApiManagementClient.Users.GetAllOrganizationsAsync(adminUserModel.UserAuth0Id, new PaginationInfo());
            var usersOrganization = allOrganizationsForUser.AlwaysList().FirstOrDefault(s => s.Id == auth0OrganizationId);
            if (usersOrganization == null)
                throw new EntityNotFoundException($"Auth0-userid {adminUserModel.UserAuth0Id} does not belong to auth0-organization-id {auth0OrganizationId}");

            await auth0ApiManagementClient.Users.UpdateAsync(adminUserModel.UserAuth0Id,
            new UserUpdateRequest
            {
                FirstName = adminUserModel.FirstName, LastName = adminUserModel.LastName,
                //PhoneNumber = adminUserModel.MobilePhoneNumber TODO: sort out formatting errors with phone numbers in auth0
            });

            return adminUserModel;
        }

        /// <inheritdoc />
        public async Task<Auth0UserModel> AddFacilityAdminToAuth0(string auth0OrganizationId, Auth0UserModel adminUserModel) =>
            await AddUserToAuth0(auth0OrganizationId, adminUserModel, RosterdRoleEnum.FacilityAdmin);

        public async Task<Auth0UserModel> AddStaffToAuth0(string auth0OrganizationId, string firstName, string lastName, string email, string mobilePhoneNumber,
            string usersAuth0OrganizationId) =>
            await AddUserToAuth0(auth0OrganizationId,
                new Auth0UserModel { Email = email, FirstName = firstName, LastName = lastName, MobilePhoneNumber = mobilePhoneNumber }, RosterdRoleEnum.Staff);

        public async Task RemoveUserFromAuth0(string auth0Id)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();
            await auth0ApiManagementClient.Users.DeleteAsync(auth0Id);
        }

        public async Task<Auth0UserModel> AddUserToAuth0(string auth0OrganizationId, Auth0UserModel adminUserModel, RosterdRoleEnum roleToAddForUser)
        {
            var auth0ApiManagementClient = await _auth0AuthenticationService.GetAuth0ApiManagementClient();

            //1. Create the user in auth0 with a random password and add this user to the organization
            var userCreatedInAuth0 = await _auth0AuthenticationService.CreateUserAndAddToOrganization(auth0OrganizationId, adminUserModel.Email,
                adminUserModel.FirstName, adminUserModel.LastName, adminUserModel.MobilePhoneNumber);

            //2. Add the right role to this user
            var roleToAdd = await _rolesService.GetRole(roleToAddForUser);
            if (roleToAdd == null)
                throw new RoleDoesNotExistException("The role specified does not exist");

            await auth0ApiManagementClient.Organizations.AddMemberRolesAsync(auth0OrganizationId, userCreatedInAuth0.UserId,
                new OrganizationAddMemberRolesRequest { Roles = new List<string> { roleToAdd.RoleId } });

            //By this point we have created the user, add them to the to the organization and assigned them the role org-admin
            //4. Now, final thing we need to do is trigger a password-change event so the user will get an email
            await _auth0AuthenticationService.SendPasswordResetEmailToUser(adminUserModel.Email);

            adminUserModel.UserAuth0Id = userCreatedInAuth0.UserId;
            return adminUserModel;
        }

        public async Task<string> GetPasswordResetLink(string auth0UserId) => await _auth0AuthenticationService.GetPasswordResetLink(auth0UserId);

        public async Task SendWelcomeEmailToStaff(string welcomeEmailLinkToSetPassword, string sendGridEmailApiKey, string staffName, string email, string organizationName)
        {
            var sendGridClient = new SendGridClient(sendGridEmailApiKey);

            var sendGridMessage = new SendGridMessage();
            sendGridMessage.SetFrom(new EmailAddress(RosterdConstants.Email.AdminFromEmailAddress, RosterdConstants.Email.AdminFromEmailName));
            sendGridMessage.AddTo(new EmailAddress(email));
            sendGridMessage.SetSubject(RosterdConstants.Email.WelcomeEmailSubject);
            sendGridMessage.SetTemplateId(RosterdConstants.Email.WelcomeEmailTemplateId);
            sendGridMessage.SetTemplateData(new
            {
                //These property-names should match exactly to the email template variables (be care its case-sensitive)
                name = staffName,
                organization = organizationName,
                registrationUrl = welcomeEmailLinkToSetPassword
            });

            var response = await sendGridClient.SendEmailAsync(sendGridMessage);
            if ((int) response.StatusCode > 400)
                throw new Exception($"Email could not be sent, got response from SendGrid of {response.StatusCode}");
        }
    }
}
