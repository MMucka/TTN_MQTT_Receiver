using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTCloud.Models;
using MQTTCloud.Services;

namespace MQTTCloud.MQTT
{
    public class MQTTRunner : IHostedService
    {
        private readonly ApplicationsService _applicationsService;
        private readonly DevicesService _devicesService;
        private readonly BlockingDevices _blockingDevices;

        private readonly MQTTParser _parser;
        private readonly List<MQTTClient> _clients;
        
        public MQTTRunner(ApplicationsService applicationsService, MessagesService messagesService, 
            DevicesService devicesService, BlockingDevices blockingDevices, GatewaysService gatewaysService)
        {
            _applicationsService = applicationsService;
            _devicesService = devicesService;
            _blockingDevices = blockingDevices;

            _parser = new MQTTParser(messagesService, gatewaysService, devicesService);
            _clients = new List<MQTTClient>();
        }

        private void Run()
        {
            // Init Devices from database
            foreach (var device in _devicesService.List())       
            {
                NewDevice(device);
            }
            
            // New devices to register
            foreach (var device in _blockingDevices.NewDevices.GetConsumingEnumerable())
            {
                NewDevice(device);
            }
        }

        private void NewDevice(Device device)
        {
            try
            {
                foreach (var client in _clients)
                {
                    if (device.ApplicationId == client.Application.Id) //application running
                    {
                        Subscribe(client, device);
                        return;
                    }
                }

                Application newApp = _applicationsService.Find(device.ApplicationId);
                if (newApp == null)
                {
                    throw new Exception("Application not exist ! Register application first");
                }

                MQTTClient newClient = new MQTTClient(_parser, newApp);
                Subscribe(newClient, device);
                _clients.Add(newClient);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("NewDevice Error: " + ex.Message);
            }
        }

        private void Subscribe(MQTTClient client, Device device)
        {
            if (device.Active)
            {
                client.SubscribeMqtt(device.DevID);
                Console.WriteLine("Subscribed to " + device.DevID);
            }
            else
            {
                client.UnsubscribeMqtt(device.DevID);
                Console.WriteLine("Unsubscribed to " + device.DevID);
            }
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(Run, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var client in _clients)
            {
                client.Stop();
            }
            return Task.CompletedTask;
        }
    }
}