using API.DTOs;
using System.Collections.Generic;

namespace XUnitTests
{
    class TestCaseSource
    {
        public static IEnumerable<object[]> Ids =>
            new List<object[]>
            {
                new object[] { 1 },
                new object[] { 5 }
            };

        public static IEnumerable<object[]> Usernames =>
            new List<object[]>
            {
                new object[] { "Lisa" },
                new object[] { "Davis" },
                new object[] { "Karen" }
            };

        public static IEnumerable<object[]> PhotoIds =>
            new List<object[]>
            {
                new object[] { 1, "Karen", 1 },
                new object[] { 5, "Todd", 1 },
                new object[] { 11, "Davis", 3 }
            };

        public static IEnumerable<object[]> RegisterDtoData =>
            new List<object[]>
            {
                new object[] {"misha", "lol", new UserDto { UserName = "misha", Token = "token"} },
                new object[] {"misha", "lol", new UserDto { UserName = "misha", Token = "token"} }
            };
    }
}
