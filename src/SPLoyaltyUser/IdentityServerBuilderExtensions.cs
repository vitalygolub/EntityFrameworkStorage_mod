// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4.SPLoyalty;
using Microsoft.EntityFrameworkCore;
using SPLoyaltyUser.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the IdentityServer builder
    /// </summary>
    public static class IdentityServerBuilderExtensions
    {
        /// <summary>
        /// Adds test users.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="users">The users.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddCustomers(this IIdentityServerBuilder builder, string ConnString)
        {
            var services = builder.Services;
            var optionsBuilder = new  DbContextOptionsBuilder<LoyaltyContext>();
            optionsBuilder.UseSqlServer(ConnString);

            //LoyaltyContext context = new LoyaltyContext(optionsBuilder.Options);
            
            
            services.AddSingleton(new CustomerStore(optionsBuilder.Options));

            builder.AddProfileService<CustomerProfileService>();
            builder.AddResourceOwnerValidator<CustomerResourceOwnerPasswordValidator>();

            return builder;
        }
    }
}