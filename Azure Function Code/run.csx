#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

using Microsoft.Azure.Devices.Shared;               // For TwinCollection
using Microsoft.Azure.Devices.Provisioning.Service; // For TwinState

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    // Get request body
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);

    log.LogInformation("Request.Body:...");

    log.LogInformation(requestBody);

    var client = new HttpClient();
    const string subscriptionKey = "ReplaceMeWithAzureMapsSubKey";
    string ipAddress = data?.deviceRuntimeContext["payload"]["IpV4"];

    log.LogInformation($"IP Address: {ipAddress}");

    string gUri = $"https://atlas.microsoft.com/geolocation/ip/json?subscription-key={subscriptionKey}&api-version=1.0&ip={ipAddress}";

    HttpResponseMessage response = await client.GetAsync(gUri);
    string responseBody = await response.Content.ReadAsStringAsync();
    dynamic locationInfo = JsonConvert.DeserializeObject(responseBody);

    log.LogInformation($"Response: {responseBody}");

    // Get registration ID of the device
    string regId = data?.deviceRuntimeContext?.registrationId;

    // Get country code
    string countryCode = locationInfo["countryRegion"]["isoCode"]?.ToString();
    log.LogInformation($"Country Code: {countryCode}");

    string message = "Uncaught error";
    bool fail = false;
    ResponseObj obj = new ResponseObj();

    string[] hubs = data?.linkedHubs.ToObject<string[]>();

    obj.iotHubHostName = hubs.FirstOrDefault();

    if (regId == null)
    {
        message = "Registration ID not provided for the device.";
        log.LogInformation("Registration ID : NULL");
        fail = true;
    }
    else if (countryCode == null)
    {
        message = "IP address of device is not correct";
        log.LogInformation($"Country Code: {countryCode}");
        fail = true;
    }

    

    log.LogInformation("\nResponse");
    log.LogInformation((obj.iotHubHostName != null) ? JsonConvert.SerializeObject(obj) : message);

    return (fail)
        ? new BadRequestObjectResult(message) 
        : (ActionResult)new OkObjectResult(obj);
}

public class ResponseObj
{
    public string iotHubHostName {get; set;}
    public TwinState initialTwin {get; set;}
}