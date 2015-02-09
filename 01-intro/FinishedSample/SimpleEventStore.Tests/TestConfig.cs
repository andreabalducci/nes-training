using System;
using System.IO;

namespace SimpleEventStore.Tests
{
    public static class TestConfig
    {
        public static readonly string TestStore = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests");
        public static readonly string PreloadedStore = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PreloadedStore");
        public const string Id = "4b8c2f8f-bdf3-47a7-b316-8ce5efa3b33e";
    }
}