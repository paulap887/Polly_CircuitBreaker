using Polly;
using Polly.CircuitBreaker;

HttpClient client = new HttpClient();
HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://api.example.com/data");


var circuitBreaker = Policy
    .Handle<HttpRequestException>()
    .CircuitBreakerAsync(
        exceptionsAllowedBeforeBreaking: 2,
        durationOfBreak: TimeSpan.FromSeconds(30)
    );

try
{
    HttpResponseMessage response = await circuitBreaker.ExecuteAsync(() => client.SendAsync(request));
    string data = await response.Content.ReadAsStringAsync();
    Console.WriteLine(data);
}
catch (BrokenCircuitException)
{
    Console.WriteLine("The circuit is open - requests to the API are blocked.");
}
