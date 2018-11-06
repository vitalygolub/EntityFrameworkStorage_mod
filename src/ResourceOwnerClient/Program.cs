// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResourceOwnerClient
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var apiurl= "http://localhost:5001/PublicWebService.asmx";

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("vitalygolub@hotmail.com", "1234567", "api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync(apiurl+"?wsdl");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
            }
            Console.WriteLine(new string('-',80));

            var xml = "<soapenv:Envelope xmlns:soapenv = \"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem = \"http://tempuri.org/\" >\n" +
   "<soapenv:Header />\n" +
   "<soapenv:Body >\n" +
      "<tem:SubmitRequest >\n" +
        "<tem:NonPosRequest CardNumber = \"9440385200600010995\" PwdHash = \"YQBhAGEAYQAxADEAMQAxAA==\" >\n" +
          "<tem:RequestRecords TableName = \"ClientsKids_VW\" Operation = \"SELECT\" />\n" +
        "</tem:NonPosRequest >\n" +
      "</tem:SubmitRequest >\n" +
   "</soapenv:Body >\n" +
"</soapenv:Envelope >";

            var body = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml");
            response = await client.PostAsync(apiurl,body);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                try
                {


                    var doc=System.Xml.Linq.XDocument.Parse(content);
                    content =  doc.ToString() ;
                }
                catch
                {

                }
                Console.WriteLine(content);
            }
            Console.WriteLine(new string('-', 80));

        }
    }
}