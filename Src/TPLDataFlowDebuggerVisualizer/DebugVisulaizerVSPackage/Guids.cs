// Guids.cs
// MUST match guids.h
using System;

namespace oshvartz.DebugVisulaizerVSPackage
{
    static class GuidList
    {
        public const string guidDebugVisulaizerVSPackagePkgString = "bd4939ed-12bd-4ff1-ae6b-a8ca2de05e97";
        public const string guidDebugVisulaizerVSPackageCmdSetString = "8d65214d-3f4f-4bbb-9bbf-24ac06bcf3bc";

        public static readonly Guid guidDebugVisulaizerVSPackageCmdSet = new Guid(guidDebugVisulaizerVSPackageCmdSetString);
    };
}