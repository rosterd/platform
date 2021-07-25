using System;
using System.Threading.Tasks;
using Rosterd.Data.SqlServer.Context;
using Rosterd.Data.SqlServer.Helpers;
using Rosterd.Data.SqlServer.Models;
using Rosterd.Domain.Exceptions;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.OrganizationModels;
using Rosterd.Services.Organizations.Interfaces;
using Rosterd.Services.Mappers;
using RestSharp;
using RestSharp.Serializers.SystemTextJson;
using System.Text.Json;

namespace Rosterd.Services.Organizations
{
    public class TokenResponse 
    {
        public string access_token { set; get; }
        public int expires_in { set; get; }
        public string scope { set; get; }
        public string token_type { set; get; }
    }

    public class CreateOrganizationRequest {
        public string name {set; get;}
        public string display_name {set; get;}
    }

    public class OrganizationBrand {
        public string logo_url { set; get;}
    }

    public class Organization {
        public string id {get; set;}
        public string name {get; set;}
        public string display_name {get; set;}
        public OrganizationBrand branding {get; set;}
    }

    public class OrganizationsService : IOrganizationsService
    {
        private readonly IRosterdDbContext _context;

        public OrganizationsService(IRosterdDbContext context) => _context = context;

        public string accessToken;

        public RestClient client;


        public async Task<PagedList<OrganizationModel>> GetAllOrganizations(PagingQueryStringParameters pagingParameters)
        {
            var query = _context.Organizations;
            
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", $"Bearer {this.accessToken}");
            IRestResponse response = this.client.Execute(request);
            Organization[] organizations = JsonSerializer.Deserialize<Organization[]>(response.Content);

            var pagedList = await PagingList<Data.SqlServer.Models.Organization>.ToPagingList(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var domainModels = pagedList.ToDomainModels();
            return new PagedList<OrganizationModel>(domainModels, pagedList.TotalCount, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalPages);
        }


        public async Task<OrganizationModel> GetOrganization(long organizationId)
        {
            var organization = await _context.Organizations.FindAsync(organizationId);
            return organization?.ToDomainModel();
        }

        private string GetAccessToken() {
            var client = new RestClient("https://rosterd-dev1.au.auth0.com/oauth/token");
            client.UseSystemTextJson();
            var request = new RestRequest(Method.POST);
            var clientId = "gCwuvFSdacLuQ86jd3eStnChowVpw2cc";
            var secret ="mM4svpI6f53vJMoD06ZKrjvKx5FdKnDfV0lWhBWRrPmj7vfbc58mEBtgVwKPCVw2";
            var domain = "rosterd-dev1.au.auth0.com";
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={clientId}&client_secret={secret}&audience=https://{domain}/api/v2/", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(response.Content);
            return tokenResponse.access_token;
        }

        public async Task<OrganizationModel> CreateOrganization(OrganizationModel organizationModel)
        {
            var accessToken = GetAccessToken();
            var organizationToCreate = organizationModel.ToNewOrganization();
            var client = new RestClient("https://rosterd-dev1.au.auth0.com/api/v2/organizations");
            client.UseSystemTextJson();
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", $"Bearer {accessToken}");

            var createOrganizationRequest = new CreateOrganizationRequest {
                name = $"organization-{organizationModel.OrganizationName.ToLower()}",
                display_name = organizationModel.OrganizationName
            };

            request.AddJsonBody(createOrganizationRequest);

            IRestResponse response = client.Execute(request);

            await _context.Organizations.AddAsync(organizationToCreate);
            await _context.SaveChangesAsync();

            return organizationToCreate.ToDomainModel();
        }

        public async Task RemoveOrganization(long organizationId)
        {
            var organization = await _context.Organizations.FindAsync(organizationId);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<OrganizationModel> UpdateOrganization(OrganizationModel organizationModel)
        {
            if (organizationModel.OrganizationId == null)
                throw new ArgumentNullException();

            var organizationFromDb = await _context.Organizations.FindAsync(organizationModel.OrganizationId);
            if (organizationFromDb == null)
                throw new EntityNotFoundException();

            var organizationModelToUpdate = organizationModel.ToDataModel(organizationFromDb);

            _context.Organizations.Update(organizationModelToUpdate);
            await _context.SaveChangesAsync();

            return organizationModelToUpdate.ToDomainModel();
        }
    }
}
