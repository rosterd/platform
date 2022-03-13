using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Rosterd.Admin.Api.Requests.Staff;
using Rosterd.ComponentTests.Fixture;
using Rosterd.ComponentTests.Helpers;
using Rosterd.Domain.Models;
using Rosterd.Domain.Models.StaffModels;
using Xunit;


namespace Rosterd.ComponentTests.Tests.Staff
{
    public class StaffComponentTests
    {
        // private readonly ApplicationFixture _appFixture = new ApplicationFixture();
        //
        //
        // [Fact]
        // public async Task GivenStaffssWhenGetAllStaffsThenPagedListWithNumberOfStaffsRequested()
        // {
        //     // ENDPOINT URL
        //     var url = ApiConstants.STAFF_ENDPOINT + "?PageNumber=1&PageSize=1";
        //
        //     // GET STAFFS
        //     var response = await _appFixture.HttpClient.GetAsync(url);
        //
        //     // ASSERT
        //     response.EnsureSuccessStatusCode();
        //
        //     var responsePagedList = JsonConvert.DeserializeObject<PagedList<StaffModel>>(response.Content.ReadAsStringAsync().Result);
        //
        //     responsePagedList.PageSize.Should().Be(1);
        //     responsePagedList.Items.Count.Should().Be(1);
        // }
        //
        // [Fact]
        // public async Task GivenValidStaffWhenPostedThenStaffIsAdded()
        // {
        //     var staffId = createStaffAsync().Result;
        //     // ENDPOINT URL
        //     var url = ApiConstants.STAFF_ENDPOINT + "/" + staffId;
        //
        //     // GET STAFFS
        //     var response = await _appFixture.HttpClient.GetAsync(url);
        //
        //     // ASSERT
        //     response.EnsureSuccessStatusCode();
        //
        //     var staffModel = JsonConvert.DeserializeObject<StaffModel>(response.Content.ReadAsStringAsync().Result);
        //
        //     await deleteStaffAsync(staffId);
        // }
        //
        //
        // [Fact]
        // public async Task GivenStaffWhenGetStaffByIdThenStaffIsReturned()
        // {
        //     var staffId = createStaffAsync().Result;
        //     // ENDPOINT URL
        //     var url = ApiConstants.STAFF_ENDPOINT + "/" + staffId;
        //
        //     // GET STAFFS
        //     var response = await _appFixture.HttpClient.GetAsync(url);
        //
        //     // ASSERT
        //     response.EnsureSuccessStatusCode();
        //
        //     var staff = JsonConvert.DeserializeObject<StaffModel>(response.Content.ReadAsStringAsync().Result);
        //
        //     await deleteStaffAsync(staffId);
        // }
        //
        //
        // [Fact]
        // public async Task GivenStaffWhenDeleteStaffThenStaffIsDeleted()
        // {
        //     var staffId = await createStaffAsync();
        //     // ENDPOINT URL
        //     var url = ApiConstants.STAFF_ENDPOINT + "/" + staffId;
        //
        //     // GET STAFFS
        //     var response = await _appFixture.HttpClient.GetAsync(url);
        //
        //     // ASSERT
        //     response.EnsureSuccessStatusCode();
        //
        //     await deleteStaffAsync(staffId);
        // }
        //
        //
        // private async Task<int> createStaffAsync()
        // {
        //     var staffId = new Random().Next(1000);
        //     var addUpdateStaffRequest = new AddStaffRequest
        //     {
        //
        //     };
        //     var stringContent = new StringContent(JsonConvert.SerializeObject(addUpdateStaffRequest), Encoding.UTF8, "application/json");
        //     var response = await _appFixture.HttpClient.PostAsync(ApiConstants.STAFF_ENDPOINT, stringContent);
        //     response.EnsureSuccessStatusCode();
        //     return staffId;
        // }
        //
        // private async Task deleteStaffAsync(int staffId)
        // {
        //     var response = await _appFixture.HttpClient.DeleteAsync(ApiConstants.STAFF_ENDPOINT + "/" + staffId);
        //     response.EnsureSuccessStatusCode();
        // }
    }
}
