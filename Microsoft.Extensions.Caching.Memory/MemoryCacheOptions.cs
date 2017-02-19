// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Caching.Memory
{
    public class MemoryCacheOptions : IOptions<MemoryCacheOptions>
    {
        public ISystemClock Clock { get; set; }

        public bool CompactOnMemoryPressure { get; set; } = DefaultCompactOnMemoryPressure;

        public TimeSpan ExpirationScanFrequency { get; set; } = DefaultExpirationScanFrequency;

        MemoryCacheOptions IOptions<MemoryCacheOptions>.Value
        {
            get { return this; }
        }
      
        /// <summary>
        /// Default value of CompactOnMemoryPressure property
        /// </summary>
        public static bool DefaultCompactOnMemoryPressure = true;

        /// <summary>
        /// Default value of ExpirationScanFrequency property
        /// </summary>
        public static TimeSpan DefaultExpirationScanFrequency = TimeSpan.FromMinutes(1);
    }
}