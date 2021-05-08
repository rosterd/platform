//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Caching.Distributed;
//using Microsoft.Extensions.Options;
//using Rosterd.Admin.Api.Infrastructure.Configs;
//using Rosterd.Admin.Api.Services;
//using Rosterd.Domain.Exceptions;
//using Rosterd.Domain.Models.StaffModels;
//using Rosterd.Domain.Models.Tenants;
//using Rosterd.Web.Infra.Extensions;

//namespace Rosterd.Admin.Api.Services
//{
//    /// <summary>
//    /// The context provides various bits of information about the logged in user, their roles and claims
//    /// The tenant this current user belongs to is cached for the specified duration
//    /// </summary>
//    public class UserContext : IUserContext
//    {
//        private readonly AppSettings _settings;
//        private readonly IDistributedCache _cache;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly ITenantService _tenantService;

//        /// <summary>
//        /// Default constructor
//        /// </summary>
//        /// <param name="settings">
//        ///     The settings.
//        /// </param>
//        /// <param name="cache">
//        ///     The cache.
//        /// </param>
//        /// <param name="httpContextAccessor"></param>
//        /// <param name="tenantService"></param>
//        public UserContext(IOptions<AppSettings> settings, IDistributedCache cache, IHttpContextAccessor httpContextAccessor, ITenantService tenantService)
//        {
//            _settings = settings.Value;
//            _cache = cache;
//            _httpContextAccessor = httpContextAccessor;
//            _tenantService = tenantService;
//        }

//        /// <summary>
//        /// Checks if a tenant and user exists for the currently logged in user
//        /// </summary>
//        /// <returns></returns>
//        public async Task<bool> DoesTenantAndUserExist()
//        {
//            try
//            {
//                _ = await GetTenantIdForUser();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Gets the tenant of the current logged in user.
//        /// NB: This gets the full tenant object and can be a bit expensive if you just need the tenant id
//        /// then call the GetTenantIdForUser() method
//        /// </summary>
//        public async Task<long> GetTenantIdForUser()
//        {
//            var auth0Id = Auth0Id;
//            var cacheKey = Caching.CacheKeys.TenantIdToAuth0Id(auth0Id);

//            var TenantIdInCache = await _cache.GetStringAsync(cacheKey);
//            if (TenantIdInCache.IsNotNullOrEmpty())
//                return TenantIdInCache.ToLong();

//            var (tenant, _) = await GetTenantAndUser();
//            await _cache.SetStringAsync(cacheKey, tenant.TenantId.ToString());

//            return tenant.TenantId;
//        }

//        /// <summary>
//        /// Gets the tenant of the current logged in user.
//        /// NB: This gets the full tenant object & StaffToAuth0Id object from cache
//        /// can be a bit expensive due to the weight of the object (serializing/deserializing from cache)
//        /// if you just need the tenant id
//        /// then call the GetTenantIdForUser() method which is a lot light weight
//        /// </summary>
//        public async Task<(TenantModel Tenant, StaffModel Staff)> GetTenantAndUser()
//        {
//            var auth0Id = Auth0Id;
//            var tenantCacheKey = Caching.CacheKeys.TenantToAuth0Id(auth0Id);
//            var staffCacheKey = Caching.CacheKeys.StaffToAuth0Id(auth0Id);

//            //Get from cache
//            var cachedTenant = await _cache.GetAsJsonAsync<TenantModel>(tenantCacheKey);
//            var cachedStaff = await _cache.GetAsJsonAsync<StaffModel>(staffCacheKey);

//            //If not in cache then populate cache
//            if (cachedTenant == null || cachedStaff == null)
//            {
//                var (tenantModel, staffModel) = await _tenantService.GetTenantAndStaffForAuth0Id(auth0Id);
//                if (tenantModel == null || staffModel == null)
//                    throw new NoTenantFoundException($"Nothing found for Auth0Id {auth0Id}");

//                await _cache.SetAsJsonAsync(tenantCacheKey, tenantModel, _cache.GetCacheExpiry(_settings.StaticDataCacheDurationMinutes));
//                await _cache.SetAsJsonAsync(staffCacheKey, staffModel, _cache.GetCacheExpiry(_settings.StaticDataCacheDurationMinutes));

//                cachedTenant = tenantModel;
//                cachedStaff = staffModel;
//            }

//            return (cachedTenant, cachedStaff);
//        }

//        /// <summary>
//        /// Gets all the roles for the user
//        /// </summary>
//        public IEnumerable<string> Roles => _httpContextAccessor.HttpContext.User.Claims.Where(s => s.Type == ClaimTypes.Role).Select(s => s.Value);
        
//        /// <summary>
//        /// Gets the Auth0Id
//        /// </summary>
//        public string Auth0Id
//        {
//            get
//            {
//                var auth0Id = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
//                return auth0Id;
//            }
//        }

//        /// <summary>
//        /// Gets the raw access token received from Auth0,
//        /// This can be useful if we need to call profile in auth0 or anything external call we need to make for the user
//        /// </summary>
//        public string AccessToken
//        {
//            get
//            {
//                var accessToken = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token").Value;
//                return accessToken;
//            }   
//        }

//        public string UserName => _httpContextAccessor.HttpContext.User.Claims.First(s => s.Type == Strieq.Infrastructure.Security.RosterdConstants.UserName).Value;

//        public string UserNickname => _httpContextAccessor.HttpContext.User.Claims.First(s => s.Type == Strieq.Infrastructure.Security.RosterdConstants.UserNickname).Value;

//        public string? UserPictureHref => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(s => s.Type == Strieq.Infrastructure.Security.RosterdConstants.UserPicture)?.Value;

//        public string UserEmail => _httpContextAccessor.HttpContext.User.Claims.First(s => s.Type == Strieq.Infrastructure.Security.RosterdConstants.UserEmailAddress).Value;

//        public bool IsUserEmailVerified => _httpContextAccessor.HttpContext.User.Claims.First(s => s.Type == Strieq.Infrastructure.Security.RosterdConstants.UserEmailVerified).Value.ToBooleanOrDefault();
//    }
//}
