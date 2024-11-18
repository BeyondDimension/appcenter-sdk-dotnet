// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using BD.AppCenter.Ingestion.Models;
using BD.AppCenter.Ingestion.Models.Serialization;
using Newtonsoft.Json;

namespace BD.AppCenter.Test
{
    [JsonObject(JsonIdentifier)]
    public class TestLog : LogWithProperties
    {
        internal const string JsonIdentifier = "testlog";

        private static int DebugId;
        
        private int _debugId = DebugId++;

        static TestLog()
        {
            LogSerializer.AddLogType(JsonIdentifier, typeof(TestLog));
        }

        public static TestLog CreateTestLog()
        {
            var properties = new Dictionary<string, string> {{"key1", "value1"}, {"key2", "value2"}, {"key3", "value3"}};
            return new TestLog(properties) {Sid = Guid.NewGuid()};
        }

        public TestLog()
        {
            Properties = new Dictionary<string, string>();
        }

        public TestLog(IDictionary<string, string> properties)
        {
            Properties = properties;
        }

        public override bool Equals(object obj)
        {
            var that = obj as TestLog;
            if (that == null)
            {
                return false;
            }

            var thisSerialized = LogSerializer.Serialize(this);
            var thatSerialized = LogSerializer.Serialize(that);
            return thisSerialized == thatSerialized;
        }

        public override int GetHashCode()
        {
            return LogSerializer.Serialize(this).GetHashCode();
        }

        public override string ToString()
        {
            return _debugId.ToString();
        }
    }
}
