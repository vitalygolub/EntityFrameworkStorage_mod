// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System;
using SPLoyaltyUser.Models;

namespace IdentityServer4.SPLoyalty
{
    /// <summary>
    /// Store for test users
    /// </summary>
    public class CustomerStore
    {
        private readonly LoyaltyContext _context;



        private Customer ConstructCustomer(Clients cl, ClientsPublic pub )
        {
            var customer = new Customer();
            customer.SubjectId = cl.Id.ToString();
            customer.Username = cl.Fname;
            customer.IsActive = true;
            customer.PwdHash = pub.PwdHash;
            customer.Claims = new Claim[]
            {
                    new Claim(JwtClaimTypes.Name, string.Format("{0} {1}",cl.Fname,cl.Lname)),
                    new Claim(JwtClaimTypes.GivenName, cl.Fname),
                    new Claim(JwtClaimTypes.FamilyName, cl.Lname??"null"),
                    new Claim(JwtClaimTypes.Email, pub.Email),
                    new Claim(JwtClaimTypes.EmailVerified, pub.IsEmailVerified?"1":"0", ClaimValueTypes.Boolean),
                //new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                //new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
            };
            return customer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerStore"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        public CustomerStore(Microsoft.EntityFrameworkCore.DbContextOptions<LoyaltyContext>  options)
        {
            _context =  new LoyaltyContext(options); 
        }

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                
                var sha1 = System.Security.Cryptography.SHA1.Create();
                var hash = sha1.ComputeHash(System.Text.Encoding.GetEncoding(1257).GetBytes(password));
                return user.PwdHash.SequenceEqual(hash);  
                                  
            }

            return false;
        }

        /// <summary>
        /// Finds the user by subject identifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public Customer FindBySubjectId(string subjectId)
        {
            Customer customer=null;
            Clients cl=_context.Clients.FirstOrDefault(c => c.Id.ToString() == subjectId);
            if (cl != null)
            {
                ClientsPublic pub = _context.ClientsPublic.FirstOrDefault(c => c.ClientsId ==cl.Id);
                customer = ConstructCustomer(cl, pub); 
                
            }
            return customer;
        }

        /// <summary>
        /// Finds the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public Customer FindByUsername(string username)
        {
            //
            // actually will find by mail
            //

            Customer customer = null;
            ClientsPublic pub = _context.ClientsPublic.FirstOrDefault(c => c.Email == username);
            
            if (pub != null)
            {
                Clients cl = _context.Clients.FirstOrDefault(c => c.Id  == pub.ClientsId);
                customer = ConstructCustomer(cl, pub);
            }
            return customer;
        }

        /// <summary>
        /// Finds the user by external provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public Customer FindByExternalProvider(string provider, string userId)
        {
            return null;
            /*return _users.FirstOrDefault(x =>
                x.ProviderName == provider &&
                x.ProviderSubjectId == userId);*/
        }

        /// <summary>
        /// Automatically provisions a user.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        public Customer AutoProvisionUser(string provider, string userId, List<Claim> claims)
        {
            // create a list of claims that we want to transfer into our store
            var filtered = new List<Claim>();

            foreach (var claim in claims)
            {
                // if the external system sends a display name - translate that to the standard OIDC name claim
                if (claim.Type == ClaimTypes.Name)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, claim.Value));
                }
                // if the JWT handler has an outbound mapping to an OIDC claim use that
                else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claim.Type))
                {
                    filtered.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claim.Type], claim.Value));
                }
                // copy the claim as-is
                else
                {
                    filtered.Add(claim);
                }
            }

            // if no display name was provided, try to construct by first and/or last name
            if (!filtered.Any(x => x.Type == JwtClaimTypes.Name))
            {
                var first = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value;
                var last = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value;
                if (first != null && last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
                }
                else if (first != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first));
                }
                else if (last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, last));
                }
            }

            // create a new unique subject id
            var sub = CryptoRandom.CreateUniqueId();

            // check if a display name is available, otherwise fallback to subject id
            var name = filtered.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value ?? sub;

            // create new user
            var user = new Customer
            {
                SubjectId = sub,
                Username = name,
                ProviderName = provider,
                ProviderSubjectId = userId,
                Claims = filtered
            };

            // add user to in-memory store
            //_users.Add(user);

            return user;
        }
    }
}