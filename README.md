https://github.com/emelhu/MemoryCache_DropOutToken

#Microsoft.Extensions.Caching.Memory --- Extended with DropOutToken

Realization of idea I had written at https://github.com/aspnet/Caching/issues/187

by eMeL   (emel@emel.hu  www.emel.hu)

#See test and example program: MemoryCacheTestApp.exe

##My idea for remove cached object when user change origin/source of it, so cached object is obsolete.

Like CancellationToken at thread, we can attach a token object to cached object.
This can be common for a group of stored objects too. :)

And when (for example) a user modify the source/origin of cached data, the AspNetCore program set a 'cancel' and when the MemoryCache make a cache-cleaning (or retrieve this cached object) drop this object with this token.