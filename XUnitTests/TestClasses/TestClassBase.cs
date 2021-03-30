using API.Data;
using System;

using static XUnitTests.SeedInMemoryDatabase;

namespace XUnitTests
{
    class TestClassBase : IDisposable
    {

        public TestClassBase()
        {
            var options = GetOptions();

            using (var Context = new DataContext(options))
            {
                SeedUsersWithOnePhoto(Context);
            }    
        }

        public void Dispose()
        {
        }
    }
}
