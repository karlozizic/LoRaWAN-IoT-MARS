# Application of the MARS IoT platform for connecting and visualizing data from devices on the LoRaWAN network
This repository contains the source code and documentation for the MARS IoT Data Collection Service, a service designed to collect data from IoT device via the LoRaWAN network and send it to the MARS IoT platform. This project is part of the "Development of Software Solutions for Industrial IoT Systems" course for the academic year 2023/24.

# Overview
The MARS IoT Data Collection Service is a standalone service responsible for:

Continuous collection of data from an ELSYS ERS EYE v1 sensor registered on a LoRaWAN network server.
Parsing received data frames.
Sending parsed information and measurements to the MARS IoT platform.

# Prerequisites
1. .NET SDK - Install the .NET SDK on your machine. Make sure to have the version compatible with the target framework specified in the project file (net8.0).
2. Dependencies - The project relies on the following NuGet packages. You can use the dotnet restore command to fetch dependencies automatically.
 
# Configuration Steps
1. Clone the repository 
git clone https://github.com/karlozizic/LoRaWAN-IoT-MARS.git
2. Open IoT Solution using your preferred code editor. 
3. Click on "ClientWebSocket" Configuration -> Edit Configuration -> configure Environmental variables if needed
4. Run "ClientWebSocket" Configuration