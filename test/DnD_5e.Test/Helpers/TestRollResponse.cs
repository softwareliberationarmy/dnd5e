using System.Text.Json;

namespace DnD_5e.Test.Helpers
{
    /// <summary>
    /// Represents what we receive from the API for a roll request
    /// </summary>
    public class TestRollResponse
    {
        public int Result { get; set; }
        public int[] Rolls { get; set; }

        public static TestRollResponse FromJson(string json)
        {
            return (TestRollResponse)JsonSerializer.Deserialize(json, typeof(TestRollResponse),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

    }
}