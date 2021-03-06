﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.ApplicationInsights;
using System;
using Erazer.Framework.FrontEnd;
using Erazer.Shared;

namespace Erazer.Infrastructure.Websockets
{
    public class WebsocketEmittor : IWebsocketEmittor
    {
        // TODO DI HttpClient!?
        // Use Singleton => https://softwareengineering.stackexchange.com/questions/330364/correct-way-of-using-httpclient
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly IOptions<WebsocketSettings> _settings;
        private readonly TelemetryClient _telemeteryClient;

        public WebsocketEmittor(IOptions<WebsocketSettings> settings, TelemetryClient telemeteryClient)
        {
            _settings = settings;
            _telemeteryClient = telemeteryClient;
        }

        public async Task Emit<T>(ReduxAction<T> action) where T : IViewModel
        {
            var jsonString = JsonConvert.SerializeObject(action, JsonSettings.CamelCaseSerializer);
            var now = DateTime.Now;

            try
            {
                await HttpClient.PostAsync(_settings.Value.ConnectionString, new StringContent(jsonString, Encoding.UTF8, "application/json"));
                _telemeteryClient.TrackDependency("Websocket NodeJS API", $"ReduxEmit - {action.Type}", now, DateTime.Now - now, true);
            }
            catch
            {
                _telemeteryClient.TrackDependency("Websocket NodeJS API", $"ReduxEmit - {action.Type}", now, DateTime.Now - now, false);
                throw;
            }
        }
    }
}
