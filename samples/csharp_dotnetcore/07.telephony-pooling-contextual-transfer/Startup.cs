﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.BotBuilderSamples.Bots;
using Microsoft.Extensions.Hosting;
using Microsoft.BotFramework.Telephony.Broker;
using Newtonsoft.Json.Linq;


namespace Microsoft.BotBuilderSamples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            //Section for pooling
            //Requires app settings "PhoneNumberPool" and "BrokerEndpoint"
            //Register the object that handles latching keys for us
            services.AddSingleton<IKeyPoolAsync>(new LatchingKeyPool(
                    Configuration.GetSection("PhoneNumberPool").GetChildren().Select(c => c?.Value).ToList()
            ));
            //Register our object that handles storing key value state for us
            services.AddSingleton<IDictionary<string, JObject>>(new Dictionary<string, JObject>());

            //register our broker service client the bot uses to call the broker service.
            services.AddSingleton(new BrokerClient(Configuration.GetValue<string>("BrokerEndpoint")));



            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, EchoBot>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}
