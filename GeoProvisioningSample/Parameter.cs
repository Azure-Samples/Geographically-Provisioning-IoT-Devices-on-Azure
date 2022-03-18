// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// This sample is based on the C# Azure IoT Samples Thermostat project at https://github.com/Azure-Samples/azure-iot-samples-csharp/tree/main/iot-hub/Samples/device/PnpDeviceSamples/Thermostat


using CommandLine;
using Microsoft.Extensions.Logging;
using System;

namespace GeoProvisioningSample.Sample
{
    /// <summary>
    /// Parameters for the application supplied via command line arguments.
    /// </summary>
    internal class Parameters
    {

        [Option(
            'e',
            "DpsEndpoint",
            Default = "global.azure-devices-provisioning.net",
            Required = false,
            HelpText = "The DPS endpoint to use during device provisioning.")]
        public string DpsEndpoint { get; set; }

        [Option(
            'i',
            "DpsIdScope",
            Required = true,
            HelpText = "The DPS ID Scope to use during device provisioning.")]
        public string DpsIdScope { get; set; }

        [Option(
            'd',
            "DeviceId",
            Required = true,
            HelpText = "The device registration Id to use during device provisioning.")]
        public string DeviceId { get; set; }

        [Option(
            'k',
            "DeviceSymmetricKey",
            Required = true,
            HelpText = "The device symmetric key to use during device provisioning.")]
        public string DeviceSymmetricKey { get; set; }

        [Option(
            'p',
            "IPAddress",
            Required = true,
            HelpText = "The device's public IPv4 or IPv6 address.")]
        public string PublicIpAddress { get; set; }

        public bool Validate(ILogger logger)
        {
            const string DeviceSecurityType = "dps";

            return (DeviceSecurityType.ToLowerInvariant()) switch
            {
                "dps" => !string.IsNullOrWhiteSpace(DpsEndpoint)
                        && !string.IsNullOrWhiteSpace(DpsIdScope)
                        && !string.IsNullOrWhiteSpace(DeviceId)
                        && !string.IsNullOrWhiteSpace(DeviceSymmetricKey),
                _ => throw new ArgumentException($"Unrecognized value for device provisioning received: {DeviceSecurityType}." +
                        $" It should be either \"dps\" or \"connectionString\" (case-insensitive)."),
            };
        }
    }
}
