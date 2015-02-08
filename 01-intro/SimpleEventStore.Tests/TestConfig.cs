using System;
using System.IO;

namespace SimpleEventStore.Tests
{
    public static class TestConfig
    {
        public static readonly string EvenstStoreFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests");
        public const string Id = "4B8C2F8F-BDF3-47A7-B316-8CE5EFA3B33E";
    }
}