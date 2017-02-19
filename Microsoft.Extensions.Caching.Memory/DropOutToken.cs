using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Memory
{
  /// <summary>
  /// A token object can attached to cached object to initiate drop out cached object. 
  /// Cached objects can grouped by one common token.
  /// </summary>
  public class DropOutToken : IDropOutToken
  {
    private bool _dropOut;

    public  bool DropOutRequest { get { return _dropOut; } }
        
    public DropOutToken()
    {
      _dropOut = false;
    }

    /// <summary>
    /// Call it for drop out from cache that object(s) which attached with this token 
    /// </summary>
    public void DropOut()
    {
      _dropOut = true;
    }
  }
}
