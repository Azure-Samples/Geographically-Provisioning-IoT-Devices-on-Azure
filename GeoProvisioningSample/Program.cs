// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// This sample is based on the C# Azure IoT Samples Thermostat project at https://github.com/Azure-Samples/azure-iot-samples-csharp/tree/main/iot-hub/Samples/device/PnpDeviceSamples/Thermostat

using CommandLine;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeoProvisioningSample.Sample
{
    public class Program
    {
        private static ILogger s_logger;

        public static async Task Main(string[] args)
        {
            // Parse application parameters
            Parameters parameters = null;
            ParserResult<Parameters> result = Parser.Default.ParseArguments<Parameters>(args)
                .WithParsed(parsedParams =>
                {
                    parameters = parsedParams;
                })
                .WithNotParsed(errors =>
                {
                    Environment.Exit(1);
                });

            s_logger = InitializeConsoleDebugLogger();
            if (!parameters.Validate(s_logger))
            {
                throw new ArgumentException("Required parameters are not set. Please recheck required variables by using \"--help\"");
            }

            s_logger.LogDebug($"Setting up the device client.");
            using DeviceClient deviceClient = await SetupDeviceClientAsync(parameters);

            if(deviceClient != null){

                await deviceClient.CloseAsync();

            }

        }

        private static ILogger InitializeConsoleDebugLogger()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                .AddFilter(level => level >= LogLevel.Debug)
                .AddSystemdConsole(options =>
                {
                    options.TimestampFormat = "[MM/dd/yyyy HH:mm:ss]";
                });
            });

            return loggerFactory.CreateLogger<GeoProvisioningSample>();
        }

        private static async Task<DeviceClient> SetupDeviceClientAsync(Parameters parameters)
        {
            DeviceClient deviceClient;
            s_logger.LogDebug($"Initializing via DPS");

            DeviceRegistrationResult dpsRegistrationResult = await ProvisionDeviceAsync(parameters);

            if(dpsRegistrationResult.ErrorCode != 0){

                s_logger.LogDebug($"Failed to provision device. DPS Error code: {dpsRegistrationResult.ErrorCode}");
                s_logger.LogDebug($"DPS Error message: {dpsRegistrationResult.ErrorMessage}");
                return null;

            }

            

            var authMethod = new DeviceAuthenticationWithRegistrySymmetricKey(dpsRegistrationResult.DeviceId, parameters.DeviceSymmetricKey);

            s_logger.LogDebug($"Assigned Hub found: {dpsRegistrationResult.AssignedHub}");

            deviceClient = DeviceClient.Create(dpsRegistrationResult.AssignedHub, authMethod, TransportType.Mqtt);


            return deviceClient;
        }


        private static async Task<DeviceRegistrationResult> ProvisionDeviceAsync(Parameters parameters)
        {
            SecurityProvider symmetricKeyProvider = new SecurityProviderSymmetricKey(parameters.DeviceId, parameters.DeviceSymmetricKey, null);
            ProvisioningTransportHandler mqttTransportHandler = new ProvisioningTransportHandlerMqtt();
            ProvisioningDeviceClient pdc = ProvisioningDeviceClient.Create(parameters.DpsEndpoint, parameters.DpsIdScope,
                symmetricKeyProvider, mqttTransportHandler);

            var ipAddressPayload = new ProvisioningRegistrationAdditionalData
            {
                JsonData = $"{{ \"IpV4\": \"{parameters.PublicIpAddress}\" }}",
            };
            return await pdc.RegisterAsync(ipAddressPayload);
        }


    }
}
