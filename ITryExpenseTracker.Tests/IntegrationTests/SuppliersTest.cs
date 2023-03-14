using ITryExpenseTracker.Core.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ITryExpenseTracker.Tests.IntegrationTests {
    public class SuppliersTest : BaseIntegrationTest {

        ITestOutputHelper _testOutputHelper;

        public SuppliersTest(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory) {
            _testOutputHelper = testOutputHelper;
        }

        #region cannot_update_not_owned_supplier_test
        [Fact]
        public async Task cannot_update_not_owned_supplier_test() {

            //add a user
            var fooUserId = await _addUser(role: ITryExpenseTracker.Core.Constants.UserRoles.USER, 
                                           userName: "fooUser", 
                                           password: "fooUserPassword");

            var fooUser2Id = await _addUser(role: ITryExpenseTracker.Core.Constants.UserRoles.USER,
                                            userName: "fooUser2", 
                                            password: "fooUserPassword");

            //add bearertoken for fooUserId
            await AddBearerTokenHeader("fooUser", "fooUserPassword");
            //add a supplier for fooUserId
            var model = new SupplierInputModel {
                Name = "fooSupplier"
            };
            var response = await HttpClient.PostAsync(GetSuppliersRoute(), new StringContent(GetJsonFromModel(model), Encoding.UTF8, "application/json"));

            Assert.True(response.IsSuccessStatusCode);


            //add bearertoken for fooUserId2
            await AddBearerTokenHeader("fooUser2", "fooUserPassword");
            //try to update a supplier not owned by fooUser2Id
            model = new SupplierInputModel {
                Name = "fooSupplier-mod",
                Id = Guid.NewGuid() //not relevant, test has to fail
            };

            response = await HttpClient.PutAsync(GetSuppliersRoute() + "/" + model.Id, new StringContent(GetJsonFromModel(model), Encoding.UTF8, "application/json"));

            var errorModel = GetErrorResponseModel(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            Assert.True((int)System.Net.HttpStatusCode.BadRequest == errorModel.StatusCode, 
                            $"Server status code response {response.StatusCode}");

            Assert.True(typeof(ITryExpenseTracker.Core.Features.Suppliers.UpdateSupplierException).FullName == errorModel.ExceptionType, $"ExceptionType: {errorModel.ExceptionType}");

            Assert.True(typeof(ITryExpenseTracker.Core.Exceptions.SupplierNotFoundException).FullName == errorModel.InnerExceptionType, $"InnerExceptionType: {errorModel.InnerExceptionType}");

            await ClearDbData()
                .ConfigureAwait(false);
        }
        #endregion

        private async Task<Guid> _addUser(string role, string userName=null, string password=null) {

            var user = userName ?? "testUser";
            var pwd = password ?? "testPassword";
            var id = Guid.NewGuid();

            //add a new user
            await AddUserWithRole(userId: id, 
                                  username: user, 
                                  password: pwd, 
                                  role: role);

            return id;

        }
    }
}
