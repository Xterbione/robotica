namespace testAPI
{

    public class RosSubscriber
    {
        private readonly HttpClient httpClient;
        private readonly string rosbridgeUrl;
        private readonly string topic;

        public RosSubscriber(string rosbridgeUrl)
        {
            this.httpClient = new HttpClient();
            this.rosbridgeUrl = rosbridgeUrl;
        }

        public async Task<string> GetLatestMessage(string topic)
        {
            try
            {
                var requestUrl = $"{rosbridgeUrl}/{topic}";
                var requestBody = new
                {
                    op = "subscribe",
                    topic = topic,
                    throttle_rate = 0
                };

                var response = await httpClient.PostAsJsonAsync(requestUrl, requestBody);
                response.EnsureSuccessStatusCode();

                var message = await response.Content.ReadAsStringAsync();
                return message;
            }
            catch (Exception ex)
            {
                // Handle any errors
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
