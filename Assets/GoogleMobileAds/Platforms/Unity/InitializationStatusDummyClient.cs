// Copyright (C) 2020 Google LLC.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Unity
{
    public class InitializationStatusDummyClient : IInitializationStatusClient
    {
        public AdapterStatus getAdapterStatusForClassName(string className)
        {
            AdapterState adapterState = AdapterState.Ready;
            string testDescription = "Ready";
            int testLatency = 0;
            return new AdapterStatus(adapterState, testDescription, testLatency);
        }

        public Dictionary<string, AdapterStatus> getAdapterStatusMap()
        {
            Dictionary<string, AdapterStatus> dummyDictionary = new Dictionary<string,AdapterStatus>();
            dummyDictionary.Add("ExampleClass", getAdapterStatusForClassName("ExampleClass"));
            return dummyDictionary;
        }
    }
}
