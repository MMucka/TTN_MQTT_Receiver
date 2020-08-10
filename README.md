# LOW ENERGY GPS TRACKER USING LORA NETWORK
Author: [Milan Múčka](mailto:xmucka02@stud.fit.vutbr.cz)
Release date: 30.7.2020

Project was created for Master Thesis on Faculthy of Information Technology in Brno University of Technology. 
This part implements cloud service for receiving messages from platform **TheThingsNetwork** using **MQTT** and storing them in database. Also include RestApi interface for providing information to user.

.NET Core application for communication with TTN MQTT Broker, PostgreSQL and RestAPI connection.


## SETTINGS
Connection string to database should be inserted in **\Services\BaserService.cs**

Message structure can be change in **\MQTT\MQTTParser.cs**

WEB pages are inserter in **\wwwroot**

## Other parts

WEB Page for User interface - [https://github.com/MMucka/TTN_LoRaTracker_MAP]: https://github.com/MMucka/TTN_LoRaTracker_MAP
STM32 HW application - [https://github.com/MMucka/STM32_LoRa_GPStracker]: https://github.com/MMucka/STM32_LoRa_GPStracker