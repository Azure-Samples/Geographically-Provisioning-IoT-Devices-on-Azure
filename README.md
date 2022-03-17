# Geographically Provisioning IoT Devices on Azure

The goal of this repository is to articulate how you can implement IoT device geo-locking with Azure IoT Hub, Device Provisioning Service (DPS), Functions and Maps. Using these Azure services, you will learn how to control an IoT device’s provisioning based on where they are connecting from.

The corresponding MS Tech Community blog that fully explains this architecture can be found <a href="https://techcommunity.microsoft.com/t5/internet-of-things-blog/geographically-provisioning-iot-devices-on-azure/ba-p/3256823">here</a>.

## Getting Started

### Prerequisites

1. A provisioned [Azure IoT Hub](https://docs.microsoft.com/azure/iot-hub/iot-hub-create-through-portal)
2. A provisioned [Azure IoT Hub Device Provisioning Service (DPS)](https://docs.microsoft.com/azure/iot-dps/quick-setup-auto-provision#create-a-new-iot-hub-device-provisioning-service)
    - Ensure the IoT Hub from step 1 has been [linked](https://docs.microsoft.com/azure/iot-dps/quick-setup-auto-provision#link-the-iot-hub-and-your-device-provisioning-service) to this DPS
3. A provisioned [Azure Function App](https://docs.microsoft.com/azure/azure-functions/functions-create-function-app-portal#create-a-function-app)
    - Ensure the <b>Runtime Stack</b> and <b>Version</b> settings have been set to <b>.NET</b> and <b>6</b>
4. A provisioned [Azure Maps account](https://docs.microsoft.com/azure/azure-maps/how-to-manage-account-keys#create-a-new-account)


## Demo
To run the demo, follow these steps:

1. Obtain your Azure Maps account [Primary Key](https://docs.microsoft.com/en-us/azure/azure-maps/how-to-manage-authentication#view-authentication-details)
    - You can find the value of this key in the Azure Portal under the Authentication blade of your Azure Maps account or by using the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/maps?view=azure-cli-latest) script below:
        ```Shell
        az maps account keys list -n "mapsAccountName" -g "resourceGroupName" -o tsv --query primaryKey
        ```
2. Update the subscriptionKey variable's value in the run.csx file, under the Azure Function Code folder, with the Primary Key value from step 1.
3. From the Azure portal, create an [HTTP Trigger function](https://docs.microsoft.com/azure/azure-functions/functions-create-function-app-portal#create-function) under your Azure Function App. 
4. Replace the HTTP Trigger function's code with the code in the run.csx file.
5. Create an Individual Enrollment with a [Custom Allocation Policy](https://docs.microsoft.com/azure/iot-dps/tutorial-custom-allocation-policies?tabs=azure-cli#create-the-enrollment) that points to your Azure Function App and the HTTP Trigger function from step 4.
6. 

## Resources

- [Azure IoT Hub Device Provisioning Service | Overview](https://docs.microsoft.com/azure/iot-dps/about-iot-dps)

- [Azure IoT Hub Device Provisioning Service | Custom Allocation Policy](https://docs.microsoft.com/azure/iot-dps/tutorial-custom-allocation-policies?tabs=azure-cli)

- [Azure Functions | Overview](https://docs.microsoft.com/azure/azure-functions/functions-overview)

- [Azure Maps | Overview](https://docs.microsoft.com/azure/azure-maps/about-azure-maps)

- [Azure Maps | Geolocation - Get IP To Location](https://docs.microsoft.com/rest/api/maps/geolocation/get-ip-to-location)

- [Azure Maps | Search - Get Search Address Reverse](https://docs.microsoft.com/rest/api/maps/search/get-search-address-reverse)

- [Azure IoT Hub | Overview](https://docs.microsoft.com/azure/iot-hub/iot-concepts-and-iot-hub)

- [Azure IoT Hub | Deployment Stamps](https://docs.microsoft.com/azure/architecture/example-scenario/iot/application-stamps)
