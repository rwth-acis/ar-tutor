﻿using i5.Toolkit.Core.ExperienceAPI;
using i5.Toolkit.Core.Utilities;
using System;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.ExperienceAPI
{
    /// <summary>
    /// Tests the <see cref="i5.Toolkit.Core.ExperienceAPI.ExperienceAPIClient"/>
    /// </summary>
    public class ExperienceAPITester : MonoBehaviour
    {
        // for security reasons, the credentials of the client,
        // e.g. its authorization token should be stored outside of the source control
        [SerializeField]
        private ExperienceAPIClientCredentials credentials;
        [SerializeField] private string xApiEndpoint = "https://lrs.tech4comp.dbis.rwth-aachen.de/data/xAPI";
        
        private void OnEnable()
        {
            // Register to event manager events
            EventManager.OnPostStatement += Post;
        }

        private void OnDisable()
        {
            // Unregister from event manager events
            EventManager.OnPostStatement -= Post;
        }

        private async void Post(string agent, string verb, string obj)
        {
            // create the client.
            // It needs a version, authorization token and the URI of the xAPI endpoint
            ExperienceAPIClient client = new ExperienceAPIClient()
            {
                Version = "1.0.3",
                AuthorizationToken = credentials.authToken,
                XApiEndpoint = new Uri(xApiEndpoint)
            };

            // create the statement that you want to send
            //Statement statement = new Statement(agent, verb, obj);
            //Statement statement = new Statement("mailto:danylo.bekhter@rwth-aachen.de", verb, obj);
            Statement statement = new Statement("mailto:danylo.bekhter@rwth-aachen.de", "http://www.example.org/test", "http://www.example.org/xApiClient");

            // send the statement
            WebResponse<string> resp = await client.SendStatementAsync(statement);

            // you can look up if the result was successful and get the API's response
            if (resp.Successful)
            {
                Debug.Log(resp.Content);
            }
            else
            {
                Debug.LogError(resp.ErrorMessage);
                Debug.LogError(resp.Content);
            }
        }

        private void Update()
        {

        }
    }
}