﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealWorld.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RealWorld.IntegrationTests
{
    public class SliceFixture : IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly string DbName = Guid.NewGuid() + ".db";

        public SliceFixture()
        {
            AutoMapper.ServiceCollectionExtensions.UseStaticRegistration = false;

            var startup = new Startup();

            var services = new ServiceCollection();

            services.AddSingleton(new AppDbContext(DbName));

            startup.ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<AppDbContext>()
                .Database.EnsureCreated();

            _scopeFactory = provider.GetService<IServiceScopeFactory>();

        }
        public void Dispose()
        {
            File.Delete(DbName);
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider,Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider,Task<T>> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                return await action(scope.ServiceProvider);
            }
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(
                sp =>
                {
                    var mediator = sp.GetService<IMediator>();
                    return mediator.Send(request);
                });
        }

        public Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(
                sp =>
                {
                    var mediator = sp.GetService<IMediator>();
                    return mediator.Send(request);
                });
        }

        public Task ExecuteDbContextAsync(Func<AppDbContext,Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>()));
        }

        public Task<T> ExecuteDbContextAsync<T>(Func<AppDbContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>()));
        }

        public Task InsertAsync(params object[] entities)
        {
            return ExecuteDbContextAsync(db =>
            {
                foreach (var entity in entities)
                {
                    db.Add(entity);
                }
                return db.SaveChangesAsync();
            });
        }


    }
}
